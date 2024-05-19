using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace skps_services.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public ICommand ChangePasswordCommand { get; private set; }

        // Add properties for email and current password
        public string Email { get; set; }
        public string CurrentPassword { get; set; }

        public ForgotPasswordViewModel()
        {
            ChangePasswordCommand = new Command<string>(async (newPassword) => await ChangePassword(Email, CurrentPassword, newPassword));
        }

        public async Task ChangePassword(string email, string currentPassword, string newPassword)
        {
            // Sign in the user with their current password
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig("your-api-key"));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, currentPassword);

            // Change the password
            try
            {
                // await authProvider.ChangeUserPasswordAsync(auth.FirebaseToken, newPassword);
                Console.WriteLine("Password updated successfully.");
            }
            catch (FirebaseAuthException e)
            {
                Console.WriteLine("Error: " + e.Reason);
                // Handle exceptions
            }
        }
    }
}
