using Acr.UserDialogs;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
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
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("No Internet", "Internet connection is required to auto login.", "OK");
                return;
            }
            try
            {
                // Retrieve the token and expiry from SecureStorage
                var storedToken = await SecureStorage.GetAsync(AppConstant.IdToken);
                var storedExpiry = await SecureStorage.GetAsync(AppConstant.Expiry);
                var storedUserDetails = await SecureStorage.GetAsync(AppConstant.UserDetailsKey);


                if (!string.IsNullOrEmpty(storedToken) && !string.IsNullOrEmpty(storedExpiry))
                {
                    // Parse the expiry date
                    if (DateTime.TryParse(storedExpiry, out DateTime expiryDate) && DateTime.UtcNow < expiryDate)
                    {
                        // Token is valid, proceed with auto-login
                        AppConstant.IdToken = storedToken;
                        AppConstant.Expiry = expiryDate.ToString("o");

                        // Verify the ID token or proceed with other logic
                        try
                        {
                            var userDetails = JsonConvert.DeserializeObject<UserNew>(storedUserDetails);
                            StoreUserDetails(userDetails);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to fetch user details: {ex.Message}");
                        }

                        await _navigation.PushModalAsync(new HomeView());
                        return;
                    }
                }

                // If the token is invalid or expired, redirect to the login page
                await _navigation.PushModalAsync(new LoginView());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Auto-login failed: {ex.Message}");
                Task.Delay(2000);
                UserDialogs.Instance.Toast("Auto-login failed. Please log in manually.", TimeSpan.FromSeconds(2));
                await _navigation.PushModalAsync(new LoginView());
            }
        }

        private void StoreUserDetails(UserNew userDetails)
        {
            AppConstant.UserName = userDetails.DisplayName;
            AppConstant.UserEmail = userDetails.Email;
            AppConstant.UserMobileNumber = userDetails.MobileNumber;
        }
    }
}
