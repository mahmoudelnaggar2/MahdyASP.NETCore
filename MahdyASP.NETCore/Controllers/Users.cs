using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MahdyASP.NETCore.Authentication;
using MahdyASP.NETCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MahdyASP.NETCore.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(JwtOptions jwtOptions,
    ApplicationDBContext dbContect) : ControllerBase
{

    [HttpPost]
    [Route("auth")]
    public ActionResult<string> AuthenticationUser(AuthenticationRequest request)
    {
        var user = dbContect.Set<User>().FirstOrDefault(x=> x.Name == request.UserName &&
        x.Password == request.Password);

        if (user == null)
        {
            return Unauthorized();
        }

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
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name)
            })
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        var accessToken = tokenHandler.WriteToken(securityToken);

        return Ok(accessToken);
    }
}
