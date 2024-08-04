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
            SendPasswordResetCommand = new Command(async () => await SendPasswordResetEmail(Email));
        }

        public async Task SendPasswordResetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email cannot be null or empty.");
                return;
            }

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(AppConstant.WebApiKey));
                await authProvider.SendPasswordResetEmailAsync(email);

                Console.WriteLine("Password reset email sent successfully.");
            }
            catch (FirebaseAuthException e)
            {
                Console.WriteLine("Error: " + e.Reason);
            }
        }
    }
}
