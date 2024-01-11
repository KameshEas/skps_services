using skps_services.Views;

namespace skps_services
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginView();
        }
    }
}
