using API.Identity.Domain;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Identity.IdentityServer
{
    public class IdentityServerProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public IdentityServerProfileService(
            UserManager<AppUser> userManager,
            IUserClaimsPrincipalFactory<AppUser> claimsFactory,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _configuration = configuration;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            
            claims.Add(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));

            var roles = await _userManager.GetRolesAsync(user);

            if (roles != null && roles.Any())
            {
                foreach(string role in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, role));
                }
            }
            
            claims.Add(new Claim(JwtClaimTypes.Scope, "test-scope"));

            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));

            string audience = _configuration.GetValue<string>("Jwt:Issuer");
            claims.Add(new Claim(JwtClaimTypes.Audience, audience));

            context.IssuedClaims = claims;
            
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
