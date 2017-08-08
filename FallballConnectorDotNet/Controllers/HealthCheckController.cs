using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace APSConnector.Controllers
{
    [Route("/v1/")]
    public class HealthCheckController : Controller
    {
        private Config _config;

        public HealthCheckController(ILogger<HealthCheckController> logger, IConfiguration config)
        {
            _config = new Config { logger = logger, config = config };
        }

        // GET /v1/
        [HttpGet(Name = "Root")]
        public JsonResult Get()
        {
            return Json(new
            {
                status = "ok",
                version = _config.config["version"],
            });
        }
    }
}