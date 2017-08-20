using System.Collections.Generic;
using FallballConnectorDotNet.Controllers;
using FallballConnectorDotNet.Fallball;
using FallballConnectorDotNet.Models.Aps;
using FallballConnectorDotNet.OA;
using Microsoft.AspNetCore.Http;

namespace FallballConnectorDotNet.Models
{
    public class Tenant
    {
        public Application App;
        public string ApsId;
        public string CompanyName;

        public static Tenant GetObject(Setting setting, HttpRequest request, string oaTenantId)
        {
            var oaTenant = Oa.GetResource<OaTenant>(setting, request, oaTenantId);
            return GetObject(setting, request, oaTenant);
        }

        public static Tenant GetObject(Setting setting, HttpRequest request, OaTenant oaTenant)
        {
            var oaApplication = Oa.GetResource<OaApplication>(setting, request, oaTenant.AppLink.ApsLink.Id);
            var oaAccount = Oa.GetResource<OaAccount>(setting, request, oaTenant.AccountLink.ApsLink.Id);

            return new Tenant
            {
                ApsId = oaTenant.Aps.Id,
                CompanyName = oaAccount.CompanyName,
                App = Application.GetObject(oaApplication)
            };
        }

        public static string Create(Setting setting, HttpRequest request, OaTenant oaTenant)
        {
            var tenant = GetObject(setting, request, oaTenant);

            // call external service
            return FbClient.Create(setting, tenant);
        }

        public static string GetAdminLogin(Setting setting, HttpRequest request, string oaTenantId)
        {
            var tenant = GetObject(setting, request, oaTenantId);

            // call external service
            var url = FbClient.GetAdminLogin(setting, tenant);

            return url;
        }
        
        public static Usage GetUsage(Setting setting, HttpRequest request, string oaTenantId)
        {
            var tenant = GetObject(setting, request, oaTenantId);

            // call external service
            return FbClient.GetUsage(setting, tenant); 
        }
    }

    public class Usage : Dictionary<string, double>
    {
    }
}