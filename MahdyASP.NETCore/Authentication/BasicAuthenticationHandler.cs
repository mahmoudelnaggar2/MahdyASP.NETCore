using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace MahdyASP.NETCore.Authentication;

public class BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
    ILoggerFactory logger, UrlEncoder encoder) : 
    AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out StringValues value))
            return Task.FromResult(AuthenticateResult.NoResult());


        if (!AuthenticationHeaderValue.TryParse(value, out var authenticationHeader))
            return Task.FromResult(AuthenticateResult.Fail("UnKnown Scheme"));

        if (!authenticationHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.Fail("UnKnown Scheme"));

        var encodedCredentials = authenticationHeader.Parameter;

        var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

        var userNameAndPassword = decodedCredentials.Split(":");

        if (userNameAndPassword[0] != "admin" || userNameAndPassword[1] != "password")
            return Task.FromResult(AuthenticateResult.Fail("Invalid user name or password"));

        var identity = new ClaimsIdentity(
            [
                    new(ClaimTypes.NameIdentifier, userNameAndPassword[0]),
                new(ClaimTypes.Name, userNameAndPassword[0])
            ],
            "Basic"
        );

        var princiable = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(princiable, "Basic");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}