using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace APSConnector.Controllers
{
    public class Config
    {
        public ILogger logger { get; set; }
        public IConfiguration config{ get; set; }
    }
}