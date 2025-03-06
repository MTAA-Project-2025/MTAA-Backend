using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MTAA_Backend.Domain.Resources.Customers;

namespace IntegrationTests.Config
{
    internal class FakePolicyEvaluator : IPolicyEvaluator
    {
        // Taken from my previous project. In that project it has been taken from some tutorial and there were modified. In this project it is also have been modified
        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var principal = new ClaimsPrincipal();

            principal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, UserRoles.User),
                new Claim(ClaimTypes.Role, UserRoles.Moderator),
                new Claim(ClaimTypes.Name, UserSettings.UserName),
                new Claim("Id",UserSettings.UserId)
            }, "FakeScheme"));

            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal,
                new AuthenticationProperties(), "FakeScheme")));
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
            AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            return await Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }
}
