using System;
using APSConnector.Controllers;
using Microsoft.AspNetCore.Http;

namespace APSConnector.Models
{
    public class User
    {
        public string apsID;
        public string userId;
        public string email;
        public Tenant tenant;

        static public User getObject(Config config, HttpRequest request, string oaUserID)
        {
            dynamic oaUser = OA.GetResource(config, request, oaUserID );

            return User.getObject(config, request, oaUser);
        }

        static public User getObject(Config config, HttpRequest request, dynamic oaUser )
        {
            dynamic oaTenant  = OA.GetResource( config, request, Convert.ToString(oaUser.tenant.aps.id ));
            dynamic oaAdminUser  = OA.GetResource( config, request, Convert.ToString(oaUser.user.aps.id ));

            User user = new User
            {
                apsID = Convert.ToString(oaUser.user.aps.id),
                userId = Convert.ToString(oaUser.userId),
                email =  Convert.ToString(oaAdminUser.email),
                tenant = Tenant.getObject(config, request, oaTenant)
            };

            return user;
        }

        public static string Create(Config config, HttpRequest request, dynamic oaUser)
        {
            User user = User.getObject(config, request, oaUser);

            // call external service
            string userId = Fallball.FBUser.Create(config, user);

            return Convert.ToString(userId);
        }

        public static void Delete(Config config, HttpRequest request, string oaUserID)
        {
            User user = User.getObject(config, request, oaUserID);
            
            // call external service
            Fallball.FBUser.Delete(config, user);
        }

        public static string GetUserLogin(Config config, HttpRequest request, string oaUserID )
        {
            User user = User.getObject(config, request, oaUserID);

            // call external service
            string url = Fallball.FBUser.GetUserLogin(config, user);

            return url;
        }
    }
}