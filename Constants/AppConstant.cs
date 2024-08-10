using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skps_services.Constants
{
    public static class AppConstant
    {
        public const string WebApiKey = "AIzaSyC8q_AFMR9VeYAKJ0ld6CQNLPTscbdgP0s";
        public const string FirebaseUri = "https://skps-66b64-default-rtdb.firebaseio.com";
        public const string ShopEmail = "+91 7708589128";
        public const string ShopNumber = "+91 7708589128";
        public const string FirebaseTokenKey = "FreshFirebaseToken";
        public const string TokenExpiryKey = "TokenExpiry";
        public static string UserMobileNumber { get; set; } = "";
        public static string UserName { get; set; }= "";
        public static string UserEmail { get; set; }= "";
        public static string IdToken { get; set; }= "";
        public static string Expiry { get; set; } = "Expiry";
        public static string SelectedLanguage { get; set; } = "en";
        public static readonly string[] Services =
        {
            "Mixie",
            "Grinder",
            "Light Fittings",
            "Heater - Service",
            "Heater - Installation",
            "Gas Stove",
            "Induction Stove",
            "Fan - Service",
            "Fan - Installation"
        };
    }
    public static class UserStore
    {
        public static string LocalId { get; set; }
        public static string FederatedId { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string DisplayName { get; set; }
        public static string Email { get; set; }
        public static bool EmailVerified { get; set; }
        public static string PhotoUrl { get; set; }
        public static string MobileNumber { get; set; }
    }
}
