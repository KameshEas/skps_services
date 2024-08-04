using Acr.UserDialogs;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using skps_services.Constants;
using skps_services.Models;
using skps_services.Views;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace skps_services.ViewModels
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private readonly string webApiKey = AppConstant.WebApiKey;
        private readonly string uri = AppConstant.FirebaseUri;
        private FirebaseClient _firebaseClient;
        private INavigation _navigation;
        private string email;
        private string password;
        private string firstName;
        private string lastName;
        private string displayName;
        private string mobileNumber;

        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public string DisplayName
        {
            get => displayName;
            set
            {
                displayName = value;
                RaisePropertyChanged(nameof(DisplayName));
                UpdateNameFromDisplayName();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        public string MobileNumber
        {
            get => mobileNumber;
            set
            {
                mobileNumber = value;
                RaisePropertyChanged(nameof(MobileNumber));
            }
        }

        public Command SignUp { get; }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateNameFromDisplayName()
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                FirstName = string.Empty;
                LastName = string.Empty;
                return;
            }

            var names = displayName.Split(' ', 2);
            FirstName = names[0];
            LastName = names.Length > 1 ? names[1] : string.Empty;
        }

        public async Task PostDataAsync(string uid, string firstName, string lastName, string displayName, string email, string mobileNumber)
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException(nameof(uid), "UID cannot be null or empty.");
            }

            var user = new UserNew
            {
                LocalId = uid,
                FederatedId = string.Empty,
                FirstName = firstName,
                LastName = lastName,
                DisplayName = displayName,
                Email = email,
                EmailVerified = false,
                MobileNumber = mobileNumber
            };

            await _firebaseClient.Child("User").Child(uid).PutAsync(JsonConvert.SerializeObject(user));
        }

        public SignUpViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _firebaseClient = new FirebaseClient(uri);
            SignUp = new Command(async () => await SignUpUserTappedAsync());
        }

        private async Task SignUpUserTappedAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Signing Up...");
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                string uid = auth.User.LocalId;
                await PostDataAsync(uid, FirstName, LastName, DisplayName, Email, MobileNumber);

                Console.WriteLine("Data written to the Firebase successfully");

                UserDialogs.Instance.HideLoading();

                await App.Current.MainPage.DisplayAlert("Successful", "User Registered successfully", "OK");
                await _navigation.PushModalAsync(new LoginView());
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.EmailExists)
                {
                    UserDialogs.Instance.HideLoading();
                    // Account already exists, navigate to login page
                    await App.Current.MainPage.DisplayAlert("Exist", "Account already exists. Please Sign In.", "OK");
                    await _navigation.PushModalAsync(new LoginView());
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    // Other authentication errors
                    await App.Current.MainPage.DisplayAlert("Alert", "Error: " + ex.Reason.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                // Other non-authentication errors
                await App.Current.MainPage.DisplayAlert("Alert", "Error: " + ex.Message, "OK");
            }
        }
    }
}
