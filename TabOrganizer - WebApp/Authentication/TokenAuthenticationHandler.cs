using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TabOrganizer___WebApp.Authentication
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider)
    : base(options, logger, encoder, clock)
        {
           // ServiceProvider = serviceProvider;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Request.Headers;
            StringValues value;

            string token;
            bool potentialToken;
            if (potentialToken = headers.TryGetValue("Authorization", out value))
            {
                token = value.ToString();


                if (string.IsNullOrEmpty(token))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Token is null"));
                }

                bool isValidToken = true; // check token here

                if (!isValidToken)
                {

                    return Task.FromResult(AuthenticateResult.Fail($"Balancer not authorize token : for token={token}"));
                }

                var claims = new[] { new Claim("token", token) };
                var identity = new ClaimsIdentity(claims, nameof(TokenAuthenticationHandler));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            return Task.FromResult(AuthenticateResult.Fail("Token is null"));
        }
    }
}
