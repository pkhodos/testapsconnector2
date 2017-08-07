using APSConnector.Controllers;
using APSConnector.Fallball;
using Microsoft.AspNetCore.Http;
using System;

namespace APSConnector.Models
{
    public class Application
    {
        public string apsID;
        
        public static Application getObject(dynamic oaApplication)
        {
            return new Application { apsID = Convert.ToString(oaApplication.aps.id) };
        }

        public static string Create(Config config, HttpRequest request, dynamic oaReseller )
        {
            Application app = Application.getObject(oaReseller);

            // call external service
            string appId = FBReseller.Create(config, app);

            return appId;
        }
    }
}