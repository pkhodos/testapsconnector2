using FallballConnectorDotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FallballConnectorDotNet.Controllers
{
    [Route("/v1/user")]
    public class UserController : Controller
    {
        private readonly Setting _setting;

        public UserController(ILogger<UserController> logger, IConfiguration config)
        {
            _setting = new Setting
            {
                Logger = logger,
                Config = config
            };
        }

        [HttpPost]
        public IActionResult Create([FromBody] OaUser oaUser)
        {
            if (oaUser == null)
                return BadRequest();

            // Call Models
            string userId = Models.User.Create(_setting, Request, oaUser);

            return CreatedAtRoute(
                "Root",
                null,
                new {userId}
            );
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] OaUser oaUser)
        {
            if (oaUser == null)
                return BadRequest();

            // Not yet implemented

            return new ObjectResult(new {userId = "user-id-1"});
        }

        [HttpGet("{id}/userlogin")]
        public IActionResult UserLogin(string id)
        {
            if (id == null)
                return BadRequest();

            // Call Models
            var url = Models.User.GetUserLogin(_setting, Request, id);

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
            if (id == null)
                return BadRequest();

            Models.User.Delete(_setting, Request, id);

            return Ok();
        }
    }
}