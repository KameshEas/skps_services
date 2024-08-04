using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using skps_services.Constants;
using skps_services.Views;

namespace skps_services
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Light;

            MainPage = new NavigationPage(new GetStartedView());
            Console.WriteLine("App initialized.");

            var token = Preferences.Get("FreshFirebaseToken", null);
            var tokenExpiry = Preferences.Get("TokenExpiry", null);

            Console.WriteLine("Stored token: " + token);
            Console.WriteLine("Stored token expiry: " + tokenExpiry);

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(tokenExpiry))
            {
                DateTime expiryDate;
                if (DateTime.TryParse(tokenExpiry, out expiryDate))
                {
                    Console.WriteLine("Parsed token expiry date: " + expiryDate.ToString());

                    if (expiryDate > DateTime.UtcNow)
                    {
                        Console.WriteLine("Token is valid. Navigating to HomeView.");
                        MainPage = new NavigationPage(new HomeView());
                    }
                    else
                    {
                        Console.WriteLine("Token has expired. Removing token and navigating to LoginView.");
                        Preferences.Remove("FreshFirebaseToken");
                        Preferences.Remove("TokenExpiry");
                        MainPage = new NavigationPage(new LoginView());
                    }
                }
                else
                {
                    // Debug log to indicate parsing failure
                    Console.WriteLine("Failed to parse token expiry date. Navigating to LoginView.");
                    MainPage = new NavigationPage(new LoginView());
                }
            }
            else
            {
                // Debug log to indicate no token found
                Console.WriteLine("No token found. Navigating to LoginView.");
                MainPage = new NavigationPage(new LoginView()); // Navigate to GetStartedView if no token
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task.Run(async () => await CheckUserAuthenticationAsync());
        }
        private async Task CheckUserAuthenticationAsync()
        {
            bool isLoggedIn = await IsUserAuthenticatedAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!isLoggedIn)
                {
                    MainPage = new NavigationPage(new LoginView());
                }
                else
                {
                    MainPage = new NavigationPage(new HomeView());
                }
            });
        }
        private async Task<bool> IsUserAuthenticatedAsync()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(AppConstant.WebApiKey));
                var token = Preferences.Get("FreshFirebaseToken", null);

                if (!string.IsNullOrEmpty(token))
                {
                    // Attempt to verify token
                    var auth = await authProvider.GetUserAsync(token);

                    if (auth != null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
