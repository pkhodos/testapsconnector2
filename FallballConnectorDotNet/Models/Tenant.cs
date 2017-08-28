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
        public string Id;
        public string CompanyName;
        
        public int UsersLimit;
        public int DiskspaceLimit;
        public int DevicesLimit;

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
                Id = oaTenant.Aps.Id,
                CompanyName = oaAccount.CompanyName,
                App = Application.GetObject(oaApplication),
                
                // resources
                UsersLimit     =  oaTenant.UsersLimit.Limit,
                DiskspaceLimit =  oaTenant.DiskspaceLimit.Limit,
                DevicesLimit   =  oaTenant.DevicesLimit.Limit
            };
        }

        public static string Create(Setting setting, HttpRequest request, OaTenant oaTenant)
        {
            var tenant = GetObject(setting, request, oaTenant);

            return FbClient.Create(setting, tenant);
        }
        
        public static void Update(Setting setting, HttpRequest request, OaTenant oaTenant)
        {
            var tenant = GetObject(setting, request, oaTenant);

            FbClient.Update(setting, tenant);
        }
        
        public static void Delete(Setting setting, HttpRequest request, string oaTenantId)
        {
            var tenant = GetObject(setting, request, oaTenantId);

            FbClient.Delete(setting, tenant);
        }

        public static string GetAdminLogin(Setting setting, HttpRequest request, string oaTenantId)
        {
            var tenant = GetObject(setting, request, oaTenantId);

            return FbClient.GetAdminLogin(setting, tenant);
        }
        
        public static Usage GetUsage(Setting setting, HttpRequest request, string oaTenantId)
        {
            var tenant = GetObject(setting, request, oaTenantId);

            return FbClient.GetUsage(setting, tenant); 
        }
    }

    public class Usage : Dictionary<string, int?>
    {
    }
}