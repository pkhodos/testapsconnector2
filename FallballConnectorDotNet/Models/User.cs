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
            OaUser oaUser = Oa.GetResource<OaUser>(setting, request, oaUserId);

            return GetObject(setting, request, oaUser);
        }

        public static User GetObject(Setting setting, HttpRequest request, OaUser oaUser)
        {
            OaTenant    oaTenant    = Oa.GetResource<OaTenant>(setting, request, oaUser.TenantLink.ApsLink.Id);
            OaAdminUser oaAdminUser = Oa.GetResource<OaAdminUser>(setting, request, oaUser.AdminUserLink.ApsLink.Id);

            var user = new User
            {
                ApsId =  oaUser.AdminUserLink.ApsLink.Id,
                UserId = oaUser.UserId,
                Email =  oaAdminUser.Email,
                Tenant = Tenant.GetObject(setting, request, oaTenant)
            };

            return user;
        }

        public static string Create(Setting setting, HttpRequest request, OaUser oaUser)
        {
            User user = GetObject(setting, request, oaUser);

            // call external service
            string userId = FbUser.Create(setting, user);

            return userId;
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