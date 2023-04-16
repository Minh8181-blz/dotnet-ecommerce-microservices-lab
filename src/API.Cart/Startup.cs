using API.Carts.Application;
using API.Carts.Application.Interfaces;
using API.Carts.BackgroundServices;
using API.Carts.Domain.Interfaces;
using API.Carts.Infrastructure;
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
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Carts
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

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<CartContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MaktaShop"),
                    x => x.MigrationsHistoryTable("_MigrationHistory", CartContext.Schema));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork<CartContext>>();
            services.AddScoped<IIntegrationEventService, IntegrationEventService<CartContext>>();
            services.AddScoped<IRequestManager, RequestManager<CartContext>>();

            // queue services
            services.AddSingleton<IQueueConnectionFactory, QueueConnectionFactory>(
                services => new QueueConnectionFactory(Configuration.GetConnectionString("QueueConnection")));
            services.AddSingleton<IQueueProcessor, QueueProcessor>();
            services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();
            services.AddSingleton<IIntegrationEventTopicMapping, IntegrationEventTopicMapping>();
            services.AddSingleton<IIntegrationEventBus, IntegrationEventBus>();
            services.AddScoped<ICartsRepository, CartsRepository>();
            services.AddScoped<IProductDao, ProductDao>();

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

            //// redis
            //services.AddSingleton(sp =>
            //{
            //    var connectionString = Configuration.GetValue<string>("ConnectionStrings:RedisConnection");
            //    var configuration = ConfigurationOptions.Parse(connectionString, true);

            //    configuration.ResolveDns = true;

            //    return ConnectionMultiplexer.Connect(configuration);
            //});

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
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Catalog API");
                c.RoutePrefix = "api/swagger";
            });

            EventSubscriber.RegisterIntegrationEventSubscription(app);
        }
    }
}
