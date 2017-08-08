using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace APSConnector.Controllers
{
    [Produces("application/json")]
    [Route("/v1/tenant")]
    public class TenantController : Controller
    {
        private Config _config;

        public TenantController(ILogger<TenantController> logger, IConfiguration config)
        {
            _config = new Config { logger = logger, config = config };
        }

        [HttpPost]
        public IActionResult Create([FromBody] dynamic oaTenant)
        {
            if ( oaTenant == null)
            {
                return BadRequest();
            }

            // Call Models
            string tenantId = Models.Tenant.Create(_config, Request, oaTenant);

            return CreatedAtRoute(
                routeName: "Root",
                routeValues: null,
                value: new { tenantId = tenantId }
                );
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] dynamic data)
        {
            if ( data== null)
            {
                return BadRequest();
            }
            // Not yet implemented

            return new ObjectResult(new { });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if ( id== null)
            {
                return BadRequest();
            }

            // Not yet implemented

            return new ObjectResult(new { });
        }


        [HttpGet("{id}")]
        public IActionResult GetUsage(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            _config.logger.LogError("\n\n ===== GET USAGE ===== \n\n ");

            return new ObjectResult(
                new
                {
                    DISKSPACE = new { usage = 2 },
                    USERS = new { usage = 2 },
                    DEVICES = new { usage = 3 }
                }
                );
        }

        [HttpGet("{id}/adminlogin")]
        public IActionResult AdminLogin(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            // Call Models
            string url = Models.Tenant.GetAdminLogin(_config, Request, id);
 
            return new ObjectResult(
                new
                {
                    redirectUrl = url
                }
                );
        }

        [HttpPut("{id}/disable")]
        public IActionResult DisableSubscription(string id, [FromBody] dynamic data)
        {
            if (data == null)
            {
                return BadRequest();
            }

            // Not implemented yet

            return new ObjectResult(new { });
        }

        [HttpPut("{id}/enable")]
        public IActionResult EnableSubscription(string id, [FromBody] dynamic data)
        {
            if (data == null)
            {
                return BadRequest();
            }

            // Not implemented yet

            return new ObjectResult(new { });
        }


        [HttpPost("{id}/users")]
        public IActionResult NotifyCreate(string id)
        {
            return Ok();
        }

        [HttpDelete("{id}/users/{userid}")]
        public IActionResult NotifyDelete(string id, string userid)
        {
            return Ok();
        }

        [HttpPost("{id}/onUsersChange")]
        public IActionResult NotifyChange(string id )
        {
            return Ok();
        }
    }
}