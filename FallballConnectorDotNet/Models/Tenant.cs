using System;
using FallballConnectorDotNet.Controllers;
using FallballConnectorDotNet.Fallball;
using FallballConnectorDotNet.OA;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FallballConnectorDotNet.Models
{
    public class Tenant
    {
        public Application App;
        public string ApsId;
        public string CompanyName;

        public static Tenant GetObject(Setting setting, HttpRequest request, string oaTenantId)
        {
            string sTenant = Oa.GetResource(setting, request, Convert.ToString(oaTenantId));
            OaTenant oaTenant = JsonConvert.DeserializeObject<OaTenant>(sTenant);
            Tenant tenant = GetObject(setting, request, oaTenant);

            return tenant;
        }

        public static Tenant GetObject(Setting setting, HttpRequest request, OaTenant oaTenant)
        {
            string app =  Oa.GetResource(setting, request, Convert.ToString(oaTenant.AppLink.ApsLink.Id));
            string account = Oa.GetResource(setting, request, Convert.ToString(oaTenant.AccountLink.ApsLink.Id));
            
            OaApplication oaApplication = JsonConvert.DeserializeObject<OaApplication>(app);
            OaAccount         oaAccount = JsonConvert.DeserializeObject<OaAccount>    (account);

            var tenant = new Tenant
            {
                ApsId = Convert.ToString(oaTenant.Aps.Id),
                CompanyName = Convert.ToString(oaAccount.CompanyName),
                App = Application.GetObject(oaApplication)
            };

            return tenant;
        }

        public static string Create(Setting setting, HttpRequest request, OaTenant oaTenant)
        {
            Tenant tenant = GetObject(setting, request, oaTenant);

            // call external service
            var tenantName = FbClient.Create(setting, tenant);

            return tenantName;
        }

        public static string GetAdminLogin(Setting setting, HttpRequest request, string oaTenantId)
        {
            var tenant = GetObject(setting, request, oaTenantId);

            // call external service
            var url = FbClient.GetAdminLogin(setting, tenant);

            return url;
        }
    }
}