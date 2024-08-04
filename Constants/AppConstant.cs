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
