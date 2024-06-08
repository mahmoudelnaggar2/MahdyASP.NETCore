using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MahdyASP.NETCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MahdyASP.NETCore.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(JwtOptions jwtOptions) : ControllerBase
{

    [HttpPost]
    [Route("auth")]
    public ActionResult<string> AuthenticationUser(AuthenticationRequest request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audiance,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, request.UserName),
                new(ClaimTypes.Email, "x@x.com")
            })
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        var accessToken = tokenHandler.WriteToken(securityToken);

        return Ok(accessToken);
    }
}
