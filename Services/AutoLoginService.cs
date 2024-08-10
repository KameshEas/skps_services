using Acr.UserDialogs;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using skps_services.Constants;
using skps_services.Models;
using skps_services.Views;
using System;
using System.Threading.Tasks;

namespace skps_services.Services
{
    internal class AutoLoginService
    {
        private readonly INavigation _navigation;

        public AutoLoginService(INavigation navigation)
        {
            _navigation = navigation;
        }

        public async Task AutoLoginAsync()
        {
            try
            {
                // Retrieve the token and expiry from SecureStorage
                var storedToken = await SecureStorage.GetAsync(AppConstant.IdToken);
                var storedExpiry = await SecureStorage.GetAsync(AppConstant.Expiry);

                if (!string.IsNullOrEmpty(storedToken) && !string.IsNullOrEmpty(storedExpiry))
                {
                    // Parse the expiry date
                    if (DateTime.TryParse(storedExpiry, out DateTime expiryDate) && DateTime.UtcNow < expiryDate)
                    {
                        // Token is valid, proceed with auto-login
                        AppConstant.IdToken = storedToken;
                        AppConstant.Expiry = expiryDate.ToString("o");

                        // Optionally, fetch user details again if needed
                        var userClient = new FirebaseClient(AppConstant.FirebaseUri);
                        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(AppConstant.WebApiKey));

                        // Check for token validity
                        try
                        {
                            var auth = await authProvider.SignInWithCustomTokenAsync(storedToken);
                            var userDetails = await userClient
                                .Child("User")
                                .Child(auth.User.LocalId)
                                .OnceSingleAsync<UserNew>();

                            StoreUserDetails(userDetails);

                            // Navigate to the home page
                            await _navigation.PushModalAsync(new HomeView());
                            return;
                        }
                        catch (Exception authEx)
                        {
                            Console.WriteLine($"SignInWithCustomTokenAsync failed: {authEx.Message}");
                        }
                    }
                }

                // If the token is invalid or expired, redirect to the login page
                await _navigation.PushModalAsync(new LoginView());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Auto-login failed: {ex.Message}");
                UserDialogs.Instance.Toast("Auto-login failed. Please log in manually.", TimeSpan.FromSeconds(2));
                await _navigation.PushModalAsync(new LoginView());
            }
        }

        private void StoreUserDetails(UserNew userDetails)
        {
            // Store the user details in your app constants or wherever you need them
            AppConstant.UserName = userDetails.DisplayName;
            AppConstant.UserEmail = userDetails.Email;
            AppConstant.UserMobileNumber = userDetails.MobileNumber;
        }
    }
}
