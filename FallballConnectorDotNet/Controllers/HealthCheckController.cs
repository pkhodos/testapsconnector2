using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FallballConnectorDotNet.Controllers
{
    [Route("/v1/")]
    public class HealthCheckController : Controller
    {
        private readonly Setting _setting;

        public HealthCheckController(ILogger<HealthCheckController> logger, IConfiguration config)
        {
            _setting = new Setting
            {
                Logger = logger,
                Config = config
            };
        }

        [AllowAnonimousAttribute]
        [HttpGet(Name = "Root")]
        public JsonResult Get()
        {
            return Json(new
            {
                status = "ok",
                version = _setting.Config["version"]
            });
        }
    }
}