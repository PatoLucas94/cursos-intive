using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Spv.Usuarios.Bff.Test.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public sealed class FakePolicyEvaluator : IPolicyEvaluator
    {
        public async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var token = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Access token is needed"));
            }

            var testScheme = "FakeScheme";
            var principal = new ClaimsPrincipal();
            principal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "UserTest")
            }, testScheme));
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal,
                new AuthenticationProperties(), testScheme)));
        }

        public async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
            AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            var authorizeResult = authenticationResult.Succeeded
                ? PolicyAuthorizationResult.Success()
                : PolicyAuthorizationResult.Challenge();

            return await Task.FromResult(authorizeResult);
        }
    }
}
