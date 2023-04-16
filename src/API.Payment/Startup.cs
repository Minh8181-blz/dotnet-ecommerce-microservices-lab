using API.Payment.Application;
using API.Payment.Application.StripeIdempotency;
using API.Payment.BackgroundServices;
using API.Payment.Domain.Interfaces;
using API.Payment.Infrastructure;
using API.Payment.Infrastructure.Repositories;
using Application.Base.SeedWork;
using Base.API.Filters;
using Domain.Base.SeedWork;
using Infrastructure.Base;
using Infrastructure.Base.Database;
using Infrastructure.Base.EventBus;
using Infrastructure.Base.EventLog;
using Infrastructure.Base.MessageQueue;
using Infrastructure.Base.RequestManager;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Plugin.Stripe;
using Plugin.Stripe.StripeLibrary;
using System;
using System.Reflection;

namespace API.Payment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());

            // db services
            services.AddDbContext<PaymentContext>(options =>
            {
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.GetConnectionString("MaktaShop"),
                        x => x.MigrationsHistoryTable("_MigrationHistory", PaymentContext.Schema));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork<PaymentContext>>();
            services.AddScoped<IPaymentOperationRepository, PaymentOperationRepository>();
            services.AddScoped<IIntegrationEventService, IntegrationEventService<PaymentContext>>();
            services.AddScoped<IRequestManager, RequestManager<PaymentContext>>();

            // queue services
            services.AddSingleton<IQueueConnectionFactory, QueueConnectionFactory>(
                services => new QueueConnectionFactory(Configuration.GetConnectionString("QueueConnection")));
            services.AddSingleton<IQueueProcessor, QueueProcessor>();
            services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();
            services.AddSingleton<IIntegrationEventBus, IntegrationEventBus>();
            services.AddSingleton<IIntegrationEventTopicMapping, IntegrationEventTopicMapping>();

            // stripe payment services
            services.UseStripePlugin(Configuration.GetValue<string>("Payment:Stripe:ApiSecret"));

            // stripe payment idempotency
            services.UseStripeIdempotencyWithDatabase<PaymentContext>();

            // background services
            services.AddHostedService<IntegrationRetryBackgroundService>();

            // authorization service
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = Configuration.GetValue<string>("IdentityOrigin");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
                options.RequireHttpsMetadata = false;
            });

            // swagger
            services.AddSwaggerGen(
            c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Payment API");
                c.RoutePrefix = "api/swagger";
            });

            EventSubscriber.RegisterIntegrationEventSubscription(app);
        }
    }
}
