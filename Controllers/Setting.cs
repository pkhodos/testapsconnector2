using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FallballConnectorDotNet.Controllers
{
    public class Setting
    {
        public ILogger Logger { get; set; }
        public IConfiguration Config { get; set; }
    }
}