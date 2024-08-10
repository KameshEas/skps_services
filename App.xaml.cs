using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using skps_services.Constants;
using skps_services.Services;
using skps_services.Views;
using System.Globalization;

namespace skps_services
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set the MainPage with a NavigationPage
            MainPage = new NavigationPage(new GetStartedView());

            // Determine the first launch and set MainPage accordingly
            SetMainPageAsync().ConfigureAwait(false);

            UserAppTheme = AppTheme.Light;

            Console.WriteLine("App initialized.");
        }

        private async Task SetMainPageAsync()
        {
            // Check if the app has been launched before
            bool isFirstLaunch = Preferences.Get("IsFirstLaunch", true);

            if (isFirstLaunch)
            {
                // If it's the first launch, set MainPage to GetStartedView
                MainPage = new NavigationPage(new GetStartedView());

                // Set the flag to false so it doesn't show GetStartedView on next launches
                Preferences.Set("IsFirstLaunch", false);
            }
            else
            {
                // If it's not the first launch, attempt auto-login
                var autoLoginService = new AutoLoginService(MainPage.Navigation);
                await autoLoginService.AutoLoginAsync();

                // If auto-login fails or token is expired, set MainPage to LoginView
                bool isLoggedIn = await IsUserAuthenticatedAsync();
                if (isLoggedIn)
                {
                    MainPage = new NavigationPage(new HomeView());
                }
                else
                {
                    MainPage = new NavigationPage(new LoginView());
                }
            }
        }
        private async Task AutoLoginAsync()
        {
            // Ensure the MainPage is wrapped in a NavigationPage before calling AutoLoginService
            if (MainPage is NavigationPage navigationPage)
            {
                var autoLoginService = new AutoLoginService(navigationPage.Navigation);
                await autoLoginService.AutoLoginAsync();
            }
            else
            {
                Console.WriteLine("MainPage is not a NavigationPage, unable to perform auto-login.");
            }
        }

        protected async override void OnStart()
        {
            base.OnStart();

            if (MainPage is NavigationPage navigationPage)
            {
                var autoLoginService = new AutoLoginService(navigationPage.Navigation);
                await autoLoginService.AutoLoginAsync();
            }

            // Set the culture from the saved language
            var culture = new CultureInfo(AppConstant.SelectedLanguage);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        protected async override void OnResume()
        {
            base.OnResume();

            if (MainPage is NavigationPage navigationPage)
            {
                var autoLoginService = new AutoLoginService(navigationPage.Navigation);
                await autoLoginService.AutoLoginAsync();
            }

            // Set the culture from the saved language
            var culture = new CultureInfo(AppConstant.SelectedLanguage);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
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
