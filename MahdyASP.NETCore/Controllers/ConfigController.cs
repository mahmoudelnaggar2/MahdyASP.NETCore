using Microsoft.AspNetCore.Mvc;

namespace MahdyASP.NETCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetConfig()
        {

            var config = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                LogLevel = _configuration["Logging:LogLevel:Default"],
                SigningKey = _configuration["SigningKey"]
            };

            return Ok(config);
        }
    }
}
