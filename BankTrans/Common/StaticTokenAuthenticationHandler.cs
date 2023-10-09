using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

public class StaticTokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string StaticToken = "YourStaticTokenHere";

    public StaticTokenAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if the provided token matches the static token.
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader) ||
            !authorizationHeader.ToString().StartsWith("Bearer "))
        {
            return AuthenticateResult.Fail("Invalid token");
        }

        var providedToken = authorizationHeader.ToString().Substring("Bearer ".Length);

        if (providedToken != StaticToken)
        {
            return AuthenticateResult.Fail("Invalid token");
        }

        // Token is valid; create a claims principal.
        var claims = new[] { new Claim(ClaimTypes.Name, "username") };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
