using FallballConnectorDotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FallballConnectorDotNet.Controllers
{
    [Produces("application/json")]
    [Route("/v1/app")]
    public class ApplicationController : Controller
    {
        private readonly Setting _setting;

        public ApplicationController(ILogger<ApplicationController> logger, IConfiguration config)
        {
            _setting = new Setting
            {
                Logger = logger,
                Config = config
            };
        }

        [HttpPost]
        public IActionResult Create([FromBody] OaApplication oaApplication)
        {
            if (oaApplication == null)
                return BadRequest();

            // Call Models
            string id = Application.Create(_setting, Request, oaApplication);

            _setting.Logger.LogInformation("ON APP CREATE RESPONSE: appID={0}", id);

            return CreatedAtRoute(
                "Root",
                null,
                new {appID = id}
            );
        }

        [HttpPost("{id}/upgrade")]
        public IActionResult Upgrade()
        {
            return Ok();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return new ObjectResult(new { });
        }


        [HttpPost("{id}/tenants")]
        public IActionResult NotifyAppCreate(string id)
        {
            return Ok();
        }

        [HttpDelete("{id}/tenants/{tenantid}")]
        public IActionResult NotifyAppDelete(string id, string tenantid)
        {
            return Ok();
        }
    }
}