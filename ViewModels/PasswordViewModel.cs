using Acr.UserDialogs;
using Firebase.Auth;
using skps_services.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace skps_services.ViewModels
{
    public class PasswordViewModel
    {
        public ICommand SendPasswordResetCommand { get; private set; }
        private INavigation _navigation;
        public string Email { get; set; }

        public PasswordViewModel(INavigation navigation)
        {
            this._navigation = navigation;
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
                UserDialogs.Instance.Alert("Reset mail sent to your mail","Password Reset", "Ok");
                _navigation.PopAsync();

            }
            catch (FirebaseAuthException e)
            {
                Console.WriteLine("Error: " + e.Reason);
            }
        }
    }
}
