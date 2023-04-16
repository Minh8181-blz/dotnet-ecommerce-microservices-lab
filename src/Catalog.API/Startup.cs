using API.Catalog.Application;
using API.Catalog.Application.DataAccess;
using API.Catalog.BackgroundServices;
using API.Catalog.Domain.Interfaces;
using API.Catalog.Infrastructure;
using API.Catalog.Infrastructure.DataAccess;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

namespace Catalog.API
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

            // db services
            services.AddSingleton(x => new QueryConnectionModel(Configuration.GetConnectionString("MaktaShop")));
            services.AddScoped<IProductDao, ProductDao>();
            services.AddDbContext<CatalogContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MaktaShop"),
                    x => x.MigrationsHistoryTable("_MigrationHistory", CatalogContext.Schema));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork<CatalogContext>>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IIntegrationEventService, IntegrationEventService<CatalogContext>>();
            services.AddScoped<IRequestManager, RequestManager<CatalogContext>>();

            // queue services
            services.AddSingleton<IQueueConnectionFactory, QueueConnectionFactory>(
                services => new QueueConnectionFactory(Configuration.GetConnectionString("QueueConnection")));
            services.AddSingleton<IQueueProcessor, QueueProcessor>();
            services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();
            services.AddSingleton<IIntegrationEventTopicMapping, IntegrationEventTopicMapping>();
            services.AddSingleton<IIntegrationEventBus, IntegrationEventBus>();

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
