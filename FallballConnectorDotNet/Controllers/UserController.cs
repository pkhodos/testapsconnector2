using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace APSConnector.Controllers
{
    [Route("/v1/user")]
    public class UserController : Controller
    {
        private Config _config;

        public UserController(ILogger<UserController> logger, IConfiguration config)
        {
            _config = new Config { logger = logger, config = config };
        }

        [HttpPost]
        public IActionResult Create([FromBody] dynamic oaUser)
        {
            if (oaUser == null)
            {
                return BadRequest();
            }

            // Call Models
            string userId = Models.User.Create(_config, Request, oaUser);

            return CreatedAtRoute(
                routeName: "Root",
                routeValues: null,
                value: new { userId = userId }
                );
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] dynamic data)
        {
            if (data == null)
            {
                return BadRequest();
            }

            // Not yet implemented

            //return new NoContentResult();
            return new ObjectResult(new { userId = "user-id-1"});
        }

        [HttpGet("{id}/userlogin")]
        public IActionResult UserLogin(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            // Call Models
            string url = Models.User.GetUserLogin(_config, Request, id);
            
            return new ObjectResult(
                new
                {
                    redirectUrl = url
                }
                );
        }
        

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if ( id== null)
            {
                return BadRequest();
            }

            // Not yet implemented

            return Ok();
        }

    }
}