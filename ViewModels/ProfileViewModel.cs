using Firebase.Auth;
using Java.Security;
using skps_services.Views;
using System.Windows.Input;

namespace skps_services.ViewModels
{
    public class ProfileViewModel
    {
        private string webApiKey = "AIzaSyC8q_AFMR9VeYAKJ0ld6CQNLPTscbdgP0s";
        private FirebaseAuthProvider authProvider;
        private INavigation _navigation;


        public ProfileViewModel()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
        }

        // Command to handle logout
        //public ICommand LogoutCommand => new Command(LogoutAsync);

        // Method to handle user logout
        //private async void LogoutAsync()
        //{
        //    try
        //    {
        //        // Sign out the user from Firebase Authentication
        //        await authProvider.SignOut();

        //        // Optionally, navigate to the login page or perform any other necessary actions
        //        // For example, you might navigate back to the login page after logout
        //        await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
        //    }
        //    catch (FirebaseAuthException ex)
        //    {
        //        // Handle authentication errors, if any
        //        Console.WriteLine("Error logging out: " + ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle other errors
        //        Console.WriteLine("Error logging out: " + ex.Message);
        //    }
        //}
    }
}
