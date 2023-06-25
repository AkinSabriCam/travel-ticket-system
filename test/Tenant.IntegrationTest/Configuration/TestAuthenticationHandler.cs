using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tenant.IntegrationTest.Configuration;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{ }

public class TestAuthenticationHandler  : AuthenticationHandler<TestAuthHandlerOptions>
{
    private readonly IHttpContextAccessor _accessor;
    
    public TestAuthenticationHandler(IOptionsMonitor<TestAuthHandlerOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IHttpContextAccessor accessor) : base(options, logger, encoder, clock)
    {
        _accessor = accessor;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            string authorizationHeader = Request.Headers["Authorization"];
            var token = authorizationHeader?.Split("test")[1].Trim();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "test");

            var result = AuthenticateResult.Success(ticket);
            return Task.FromResult(result);
        }
        catch (Exception e)
        {
            Console.WriteLine($"TestAuthenticationHandler error : {e.Message}");
            throw;
        }
    }
}