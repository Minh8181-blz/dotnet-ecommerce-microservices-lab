using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace API.Identity.Config
{
    public class IdentityInitializer
    {
        public void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                var migratedClients = context.Clients.ToList();
                var notYetMigratedClients = IdentityConfig.Clients.Where(x => !migratedClients.Any(c => x.ClientId == c.ClientId));
                
                if (notYetMigratedClients.Any())
                {
                    foreach (var client in notYetMigratedClients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                }

                var migratedIdentityResources = context.IdentityResources.ToList();
                var notYetMigratedIdentityResources = IdentityConfig.IdentityResources
                    .Where(x => !migratedIdentityResources.Any(r => x.Name == r.Name));

                if (notYetMigratedIdentityResources.Any())
                {
                    foreach (var resource in notYetMigratedIdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                }

                var migratedApiResources = context.ApiResources.ToList();
                var notYetMigratedApiResources = IdentityConfig.ApiResources.Where(x => !migratedApiResources.Any(r => x.Name == r.Name));

                if (notYetMigratedApiResources.Any())
                {
                    foreach (var resource in notYetMigratedApiResources)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                }

                var migratedApiScopes = context.ApiScopes.ToList();
                var notYetMigratedApiScopes = IdentityConfig.ApiScopes.Where(x => !migratedApiScopes.Any(r => x.Name == r.Name));

                if (notYetMigratedApiScopes.Any())
                {
                    foreach (var scope in notYetMigratedApiScopes)
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
