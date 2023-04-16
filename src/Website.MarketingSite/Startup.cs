using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Website.MarketingSite.Configurations;
using Website.MarketingSite.Filters.Actions;
using Website.MarketingSite.Middlewares.Common;
using Website.MarketingSite.Services;

namespace Website.MarketingSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnv = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnv { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (HostEnv.IsDevelopment() || HostEnv.EnvironmentName.StartsWith("Dev"))
            {
                services.AddControllersWithViews().AddRazorRuntimeCompilation();
            }
            else
            {
                services.AddControllersWithViews(options =>
                {
                    options.Filters.Add(typeof(HttpExceptionFilter));
                });
            }

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.Cookie.Name = "id_cookie";
                    options.LoginPath = "/login";
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                });

            services.AddHttpClient<ProductService>();
            services.AddHttpClient<AuthService>();
            services.AddHttpClient<CartService>();
            services.AddHttpClient<OrderService>();
            services.AddHttpClient<PaymentService>();

            services.AddSingleton<ApiEndpointConfiguration>();
            services.Configure<IdentityClientConfiguration>(Configuration.GetSection("Id4Credentials"));
            services.Configure<PaymentConfiguration>(Configuration.GetSection("Payment"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<CheckAndRefreshTokenMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
