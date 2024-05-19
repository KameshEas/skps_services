using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
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

            // Debug log to indicate app startup
            Console.WriteLine("App initialized.");

            // Check if there is a stored login token
            var token = Preferences.Get("FreshFirebaseToken", null);
            var tokenExpiry = Preferences.Get("TokenExpiry", null);

            // Debug logs to show token and expiry date
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
                        // Debug log to indicate valid token
                        Console.WriteLine("Token is valid. Navigating to HomeView.");
                        MainPage = new NavigationPage(new HomeView()); // Navigate to the main page if token is valid
                    }
                    else
                    {
                        // Debug log to indicate expired token
                        Console.WriteLine("Token has expired. Removing token and navigating to LoginView.");
                        Preferences.Remove("FreshFirebaseToken");
                        Preferences.Remove("TokenExpiry");
                        MainPage = new NavigationPage(new LoginView()); // Navigate to login page if token expired
                    }
                }
                else
                {
                    // Debug log to indicate parsing failure
                    Console.WriteLine("Failed to parse token expiry date. Navigating to LoginView.");
                    MainPage = new NavigationPage(new LoginView()); // Navigate to login page if parsing failed
                }
            }
            else
            {
                // Debug log to indicate no token found
                Console.WriteLine("No token found. Navigating to LoginView.");
                MainPage = new NavigationPage(new LoginView()); // Navigate to GetStartedView if no token
            }
        }
    }
}
