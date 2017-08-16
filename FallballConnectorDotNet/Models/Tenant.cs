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
            OaTenant oaTenant = Oa.GetResource<OaTenant>(setting, request, oaTenantId);
            Tenant tenant = GetObject(setting, request, oaTenant);

            return tenant;
        }

        public static Tenant GetObject(Setting setting, HttpRequest request, OaTenant oaTenant)
        {
            OaApplication oaApplication = Oa.GetResource<OaApplication>(setting, request, oaTenant.AppLink.ApsLink.Id);
            OaAccount         oaAccount = Oa.GetResource<OaAccount>(setting, request, oaTenant.AccountLink.ApsLink.Id);

            var tenant = new Tenant
            {
                ApsId = oaTenant.Aps.Id,
                CompanyName = oaAccount.CompanyName,
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