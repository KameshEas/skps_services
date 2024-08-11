using Acr.UserDialogs;
using Firebase.Auth;
using skps_services.Constants;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace skps_services.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public ICommand SendPasswordResetCommand { get; private set; }
        public string Email { get; set; }

        public ForgotPasswordViewModel()
        {
            Email = AppConstant.UserEmail;
            SendPasswordResetCommand = new Command(async () => await SendPasswordResetEmail(Email));
        }

        public async Task SendPasswordResetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email cannot be null or empty.");
                return;
            }
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("No Internet", "Internet connection is required to sign up.", "OK");
                return;
            }
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(AppConstant.WebApiKey));
                UserDialogs.Instance.ShowLoading("Reset Password");
                await authProvider.SendPasswordResetEmailAsync(email);
                UserDialogs.Instance.HideLoading();
                Console.WriteLine("Password reset email sent successfully.");
                UserDialogs.Instance.Toast("Reset mail sent to your mail", TimeSpan.FromSeconds(2));

            }
            catch (FirebaseAuthException e)
            {
                Console.WriteLine("Error: " + e.Reason);
            }
        }
    }
}
