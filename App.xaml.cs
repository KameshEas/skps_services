using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using skps_services.Views;

namespace skps_services
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Light;

            if (Preferences.ContainsKey("HasLaunchedBefore"))
            {
                MainPage = new LoginView();
            }
            else
            {
                MainPage = new GetStartedView();
                Preferences.Set("HasLaunchedBefore", true);
            }
        }
    }
}
