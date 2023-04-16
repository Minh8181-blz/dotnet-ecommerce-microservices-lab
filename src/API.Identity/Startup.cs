using API.Identity.Config;
using API.Identity.Domain;
using API.Identity.IdentityServer;
using API.Identity.Infrastructure;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Infrastructure.Base;
using Infrastructure.Base.Database;
using Infrastructure.Base.RequestManager;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace API.Identity
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
            string mainDbConnectionString = Configuration.GetConnectionString("MaktaShop");

            services.AddControllers();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(mainDbConnectionString,
                    x => x.MigrationsHistoryTable("_MigrationHistory", IdentityDbContext.Schema));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork<IdentityDbContext>>();
            //services.AddScoped<IIntegrationEventService, IntegrationEventService<IdentityDbContext>>();
            services.AddScoped<IRequestManager, RequestManager<IdentityDbContext>>();

            services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<IdentityDbContext>();

            services.AddIdentityServer(options =>
            {
                options.IssuerUri = Configuration.GetValue<string>("Jwt:Issuer");
                options.EmitStaticAudienceClaim = true;
            })
            //.AddInMemoryClients(IdentityConfig.Clients)
            //.AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
            //.AddInMemoryApiResources(IdentityConfig.ApiResources)
            //.AddInMemoryApiScopes(IdentityConfig.ApiScopes)
            //.AddTestUsers(IdentityConfig.TestUsers)
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<AppUser>()
            .AddProfileService<IdentityServerProfileService>()
            .AddConfigurationStore(options =>
            {
                // add-migration context=--context=ConfigurationDbContext
                // update-database --context=ConfigurationDbContext
                options.DefaultSchema = "ms_identity__ids_conf";
                options.ConfigureDbContext = b => b.UseSqlServer(mainDbConnectionString,
                    sql =>
                    {
                        sql.MigrationsAssembly(migrationsAssembly);
                        sql.MigrationsHistoryTable("_MigrationHistory", options.DefaultSchema);
                    });
            })
            .AddOperationalStore(options =>
            {
                // Add-Migration AddIds4Persisted -c PersistedGrantDbContext -o Migrations/Ids4PersistedGrantContext
                // update-database --context=PersistedGrantDbContext
                options.DefaultSchema = "ms_identity__ids_oper";
                options.ConfigureDbContext = b => b.UseSqlServer(mainDbConnectionString,
                    sql =>
                    {
                        sql.MigrationsAssembly(migrationsAssembly);
                        sql.MigrationsHistoryTable("_MigrationHistory", options.DefaultSchema);
                    });
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
                options.Lockout.MaxFailedAccessAttempts = 8;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetValue<string>("IdentityOrigin");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetValue<string>("Jwt:Issuer"),
                    ValidAudience = string.Format("{0}/resources", Configuration.GetValue<string>("Jwt:Issuer")),
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:SecretKey")))
                };
            });

            services.AddTransient<AuthService>();

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
            new IdentityInitializer().InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseIdentityServer();

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
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Identity API");
                c.RoutePrefix = "api/swagger";
            });
        }
    }
}
