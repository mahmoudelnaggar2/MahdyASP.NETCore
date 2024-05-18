using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MahdyASP.NETCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsSnapshot<AttachmentOptions> _attachmentOptions;

        public ConfigController(IConfiguration configuration,
            IOptionsSnapshot<AttachmentOptions> attachmentOptions)
        {
            _configuration = configuration;
            _attachmentOptions = attachmentOptions;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetConfig()
        {

            var config = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                LogLevel = _configuration["Logging:LogLevel:Default"],
                SigningKey = _configuration["SigningKey"],
                AttachmentsOptions = _attachmentOptions.Value
            };

            return Ok(config);
        }
    }
}
