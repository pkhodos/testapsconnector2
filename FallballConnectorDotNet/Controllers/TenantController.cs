using FallballConnectorDotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FallballConnectorDotNet.Controllers
{
    [Produces("application/json")]
    [Route("/v1/tenant")]
    public class TenantController : Controller
    {
        private readonly Setting _setting;

        public TenantController(ILogger<TenantController> logger, IConfiguration config)
        {
            _setting = new Setting
            {
                Logger = logger, 
                Config = config
            };
        }

        [HttpPost]
        public IActionResult Create([FromBody] OaTenant oaTenant)
        {
            if (oaTenant == null)
                return BadRequest();

            // Call Models
            var tenantId = Tenant.Create(_setting, Request, oaTenant);

            return CreatedAtRoute(
                "Root",
                null,
                new {tenantId}
            );
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] OaTenant data)
        {
            if (data == null)
                return BadRequest();
            // Not yet implemented

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (id == null)
                return BadRequest();

            // Not yet implemented

            return Ok();
        }


        [HttpGet("{id}")]
        public IActionResult GetUsage(string id)
        {
            if (id == null)
                return BadRequest();

            _setting.Logger.LogError("\n\n ===== GET USAGE ===== \n\n ");

            return new ObjectResult(
                new
                {
                    DISKSPACE = new {usage = 2},
                    USERS = new {usage = 2},
                    DEVICES = new {usage = 3}
                }
            );
        }

        [HttpGet("{id}/adminlogin")]
        public IActionResult AdminLogin(string id)
        {
            if (id == null)
                return BadRequest();

            // Call Models
            var url = Tenant.GetAdminLogin(_setting, Request, id);

            return new ObjectResult(
                new
                {
                    redirectUrl = url
                }
            );
        }

        [HttpPut("{id}/disable")]
        public IActionResult DisableSubscription(string id)
        {
            // Not implemented yet

            return Ok();
        }

        [HttpPut("{id}/enable")]
        public IActionResult EnableSubscription(string id)
        {
            // Not implemented yet

            return Ok();
        }


        [HttpPost("{id}/users")]
        public IActionResult NotificationUserCreated(string id)
        {
            return Ok();
        }

        [HttpDelete("{id}/users/{userid}")]
        public IActionResult NotificationUserDeleted(string id, string userid)
        {
            return Ok();
        }

        [HttpPost("{id}/onUsersChange")]
        public IActionResult NotificationUserChanged(string id)
        {
            return Ok();
        }
    }
}