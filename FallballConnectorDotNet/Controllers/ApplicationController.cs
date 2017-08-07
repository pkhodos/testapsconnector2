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

    [Produces("application/json")]
    [Route("/v1/app")]
    public class ApplicationController : Controller
    {
        private Config _config;

        public ApplicationController(ILogger<ApplicationController> logger, IConfiguration config)
        {
            _config = new Config { logger = logger, config = config };
        }

        [HttpPost]
        public IActionResult Create([FromBody] dynamic oaReseller)
        {
            if (oaReseller == null)
            {
                return BadRequest();
            }

            // Call Models
            string id = Models.Application.Create(_config, Request, oaReseller);

            _config.logger.LogInformation("\n\nON APP CREATE RESPONSE: appID=" + id + "\n\n");

            return CreatedAtRoute(
                routeName: "Root",
                routeValues: null,
                value: new { appID = id }
                );
        }

        [HttpPost("{id}/upgrade")]
        public IActionResult Upgrade([FromBody] dynamic appData)
        {
            // appData is supposed to be empty
            // Not yet supported

            return new ObjectResult(new { });
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            // Not yet supported

            return new ObjectResult(new { });
        }


        [HttpPost("{id}/tenants")]
        public IActionResult NotifyAppCreate(string id)
        {
            return new ObjectResult(new { });
        }

        [HttpDelete("{id}/tenants/{tenantid}")]
        public IActionResult NotifyAppDelete(string id, string tenantid )
        {
            return new ObjectResult(new { });
        }
    }
}