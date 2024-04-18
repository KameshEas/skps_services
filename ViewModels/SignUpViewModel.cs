using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using skps_services.Services;
using skps_services.Views;
using System.ComponentModel;
using User = skps_services.Models.User;

namespace skps_services.ViewModels
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyC8q_AFMR9VeYAKJ0ld6CQNLPTscbdgP0s";
        public string Uri = "https://skps-66b64-default-rtdb.firebaseio.com";
        private FirebaseClient _firebaseClient;
        //private DataService _dataService;
        private INavigation _navigation;
        private string email;
        private string password;
        private string mobileNumber;
        private string name;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string MobileNumber
        {
            get => mobileNumber;
            set
            {
                mobileNumber = value;
                RaisePropertyChanged("MobileNumber");
            }
        }
        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string Password
        {
            get => password; set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        public Command SignUp { get; }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public async Task PostDataAsync(string name, string email, string mobileNumber)
        {
            await _firebaseClient.Child("User").PostAsync(new User
            {
                Name = name,
                Email = email,
                MobileNumber = mobileNumber,
            });
        }


        public SignUpViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            //_dataService = new DataService();  // Initialize the instance
            _firebaseClient = new FirebaseClient(Uri);
            SignUp = new Command(SignUpUserTappedAsync);
        }

        private async void SignUpUserTappedAsync(object obj)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);
                string token = auth.FirebaseToken;

                await PostDataAsync(Name, Email, MobileNumber);
                Console.WriteLine("Data written to the Firebase successfully");

                await App.Current.MainPage.DisplayAlert("Alert", "User Registered successfully", "OK");
                await this._navigation.PushModalAsync(new LoginView());
                await this._navigation.PopAsync();
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.EmailExists)
                {
                    // Account already exists, navigate to login page
                    await App.Current.MainPage.DisplayAlert("Alert", "Account already exists. Please Sign In.", "OK");
                    // Navigate to your login page here
                    await this._navigation.PushModalAsync(new LoginView());
                }
                else
                {
                    // Other authentication errors
                    await App.Current.MainPage.DisplayAlert("Alert", "Error: " + ex.Reason.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                // Other non-authentication errors
                await App.Current.MainPage.DisplayAlert("Alert", "Error: " + ex.Message, "OK");
            }
        }

    }
}
