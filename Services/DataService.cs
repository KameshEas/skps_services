using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;

namespace skps_services.Services
{
    public class DataService
    {
        private FirebaseClient _firebaseClient;

        public DataService()
        {
            _firebaseClient = new FirebaseClient("https://skps-66b64-default-rtdb.firebaseio.com");
        }

        public async Task SaveUser(User user)
        {
            await _firebaseClient
                .Child("Users")
                .PostAsync(user);
        }
        public async Task StoreUserData(string name, string email, string mobileNumber)
        {
            var user = new { Name = name, Email = email, MobileNumber = mobileNumber };
            try
            {
                await _firebaseClient.Child("Users").PostAsync(user);
                Console.WriteLine("User data stored successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error storing user data: " + ex.Message);
            }
        }
    }
}
