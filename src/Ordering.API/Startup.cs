using System;
using System.Reflection;
using API.Ordering.Application;
using API.Ordering.Application.IDaos;
using API.Ordering.Configuration;
using API.Ordering.Infrastructure.Daos;
using Application.Base.SeedWork;
using Base.API.Filters;
using BB.EventBus.RabbitMQ;
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
using Ordering.API.Application;
using Ordering.API.BackgroundServices;
using Ordering.API.Domain.Interfaces;
using Ordering.API.Domain.Services;
using Ordering.API.Infrastructure;

namespace Ordering.API
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

            // domain services
            services.AddTransient<IOrderDomainService, OrderDomainService>();

            // db services
            services.AddDbContext<OrderingContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MaktaShop"),
                    x => x.MigrationsHistoryTable("_MigrationHistory", OrderingContext.Schema));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork<OrderingContext>>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IIntegrationEventService, IntegrationEventService<OrderingContext>>();
            services.AddScoped<IRequestManager, RequestManager<OrderingContext>>();

            services.AddTransient<IOrdersDao, OrdersDao>();

            // queue services
            services.AddSingleton<IQueueConnectionFactory, QueueConnectionFactory>(
                services => new QueueConnectionFactory(Configuration.GetConnectionString("QueueConnection")));
            services.AddSingleton<IQueueProcessor, QueueProcessor>();
            services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();
            services.AddSingleton<IIntegrationEventBus, IntegrationEventBus>();
            services.AddSingleton<IIntegrationEventTopicMapping, IntegrationEventTopicMapping>();

            // event bus rabbitmq
            services.UseRabbitMqEventBus(RabbitMqEventBusOptions.GetOptions(Configuration));

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

            // background services
            services.AddHostedService<IntegrationRetryBackgroundService>();

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
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Ordering API");
                c.RoutePrefix = "api/swagger";
            });

            EventSubscriber.RegisterIntegrationEventSubscription(app);
            app.RegisterIntegrationEventSubscription();
        }
    }
}
