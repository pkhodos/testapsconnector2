using System;
using FallballConnectorDotNet.Controllers;
using FallballConnectorDotNet.Fallball;
using FallballConnectorDotNet.OA;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FallballConnectorDotNet.Models
{
    public class User
    {
        public string ApsId;
        public string Email;
        public Tenant Tenant;
        public string UserId;

        public static User GetObject(Setting setting, HttpRequest request, string oaUserId)
        {
            string sUser = Oa.GetResource(setting, request, oaUserId);

            OaUser oaUser = JsonConvert.DeserializeObject<OaUser>(sUser);

            return GetObject(setting, request, oaUser);
        }

        public static User GetObject(Setting setting, HttpRequest request, OaUser oaUser)
        {
            var sTenant    = Oa.GetResource(setting, request, oaUser.TenantLink.ApsLink.Id);
            var sAdminUser = Oa.GetResource(setting, request, oaUser.AdminUserLink.ApsLink.Id);
            
            OaTenant    oaTenant    = JsonConvert.DeserializeObject<OaTenant>(sTenant);
            OaAdminUser oaAdminUser = JsonConvert.DeserializeObject<OaAdminUser>(sAdminUser);

            var user = new User
            {
                ApsId = Convert.ToString(oaUser.AdminUserLink.ApsLink.Id),
                UserId = Convert.ToString(oaUser.UserId),
                Email = Convert.ToString(oaAdminUser.Email),
                Tenant = Tenant.GetObject(setting, request, oaTenant)
            };

            return user;
        }

        public static string Create(Setting setting, HttpRequest request, OaUser oaUser)
        {
            User user = GetObject(setting, request, oaUser);

            // call external service
            var userId = FbUser.Create(setting, user);

            return Convert.ToString(userId);
        }

        public static void Delete(Setting setting, HttpRequest request, string oaUserId)
        {
            var user = GetObject(setting, request, oaUserId);

            // call external service
            FbUser.Delete(setting, user);
        }

        public static string GetUserLogin(Setting setting, HttpRequest request, string oaUserId)
        {
            var user = GetObject(setting, request, oaUserId);

            // call external service
            var url = FbUser.GetUserLogin(setting, user);

            return url;
        }
    }
}