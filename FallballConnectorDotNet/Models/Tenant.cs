using System;
using APSConnector.Controllers;
using Microsoft.AspNetCore.Http;

namespace APSConnector.Models
{
    public class Tenant
    {
        public string apsID;
        public string companyName;
        public Application app;

        static public Tenant getObject(Config config, HttpRequest request, string oaTenantID)
        {
            dynamic oaTenant = OA.GetResource(config, request, Convert.ToString(oaTenantID));

            Tenant tenant = Tenant.getObject(config, request, oaTenant );

            return tenant;
        }

        static public Tenant getObject(Config config, HttpRequest request, dynamic oaTenant)
        {
            dynamic app     = OA.GetResource(config, request, Convert.ToString(oaTenant.app.aps.id));
            dynamic account = OA.GetResource(config, request, Convert.ToString(oaTenant.account.aps.id));

            Tenant tenant = new Tenant
            {
                apsID       = Convert.ToString(oaTenant.aps.id),
                companyName = Convert.ToString(account.companyName),
                app         = Application.getObject(app)
            };

            return tenant;
        }

        public static string Create(Config config, HttpRequest request, dynamic oaTenant )
        {
            Tenant tenant = Tenant.getObject(config, request, oaTenant);

            // call external service
            string tenantName = Fallball.FBClient.Create(config, tenant);

            return tenantName;
        }

        public static string GetAdminLogin(Config config, HttpRequest request,  string oaTenantID )
        {
            Tenant tenant = Tenant.getObject(config, request, oaTenantID);

            // call external service
            string url = Fallball.FBClient.GetAdminLogin(config, tenant);

            return url;
        }
    }
}