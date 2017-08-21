using FallballConnectorDotNet.Controllers;
using FallballConnectorDotNet.Fallball;
using FallballConnectorDotNet.Models.Aps;

namespace FallballConnectorDotNet.Models
{
    public class Application
    {
        public string Id;
        public string Type;
        public string Status;

        public static Application GetObject(OaApplication oaApplication)
        {
            return new Application {Id = oaApplication.Aps.Id, Type = oaApplication.Aps.Type, Status = oaApplication.Aps.Status};
        }

        public static string Create(Setting setting, OaApplication oaApplication)
        {

            var app = GetObject(oaApplication);

            // call external service
            return FbReseller.Create(setting, app);
        }
    }
}