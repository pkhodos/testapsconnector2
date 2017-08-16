using FallballConnectorDotNet.Controllers;
using FallballConnectorDotNet.Fallball;
using Microsoft.AspNetCore.Http;

namespace FallballConnectorDotNet.Models
{
    public class Application
    {
        public string ApsId;

        public static Application GetObject(OaApplication oaApplication)
        {
            return new Application {ApsId = oaApplication.Aps.Id};
        }

        public static string Create(Setting setting, HttpRequest request, OaApplication oaApplication)
        {
            Application app = GetObject(oaApplication);

            // call external service
            var appId = FbReseller.Create(setting, app);

            return appId;
        }
    }
}