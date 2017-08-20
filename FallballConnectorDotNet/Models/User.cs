using FallballConnectorDotNet.Controllers;
using FallballConnectorDotNet.Fallball;
using FallballConnectorDotNet.Models.Aps;
using FallballConnectorDotNet.OA;
using Microsoft.AspNetCore.Http;

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
            var oaUser = Oa.GetResource<OaUser>(setting, request, oaUserId);

            return GetObject(setting, request, oaUser);
        }

        public static User GetObject(Setting setting, HttpRequest request, OaUser oaUser)
        {
            var    oaTenant    = Oa.GetResource<OaTenant>(setting, request, oaUser.TenantLink.ApsLink.Id);
            var oaAdminUser = Oa.GetResource<OaAdminUser>(setting, request, oaUser.AdminUserLink.ApsLink.Id);

            return new User
            {
                ApsId =  oaUser.AdminUserLink.ApsLink.Id,
                UserId = oaUser.UserId,
                Email =  oaAdminUser.Email,
                Tenant = Tenant.GetObject(setting, request, oaTenant)
            };
        }

        public static string Create(Setting setting, HttpRequest request, OaUser oaUser)
        {
            var user = GetObject(setting, request, oaUser);

            return FbUser.Create(setting, user);
        }

        public static void Delete(Setting setting, HttpRequest request, string oaUserId)
        {
            var user = GetObject(setting, request, oaUserId);

            FbUser.Delete(setting, user);
        }

        public static string GetUserLogin(Setting setting, HttpRequest request, string oaUserId)
        {
            var user = GetObject(setting, request, oaUserId);

            return FbUser.GetUserLogin(setting, user);
        }
    }
}