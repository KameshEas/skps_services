using Acr.UserDialogs;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
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

        public async Task PostDataAsync(string uid, string name, string email, string mobileNumber)
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException(nameof(uid), "UID cannot be null or empty.");
            }

            var user = new User
            {
                Uid = uid,
                Name = name,
                Email = email,
                MobileNumber = mobileNumber,
            };

            await _firebaseClient.Child("User").Child(uid).PutAsync(JsonConvert.SerializeObject(user));
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
                UserDialogs.Instance.ShowLoading("Signing Up...");
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                string uid = auth.User.LocalId;
                await PostDataAsync(uid, Name, Email, MobileNumber);

                Console.WriteLine("Data written to the Firebase successfully");

                UserDialogs.Instance.HideLoading();

                await App.Current.MainPage.DisplayAlert("Successfull", "User Registered successfully", "OK");
                await this._navigation.PushModalAsync(new LoginView());
                await this._navigation.PopModalAsync();
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.EmailExists)
                {
                    UserDialogs.Instance.HideLoading();
                    // Account already exists, navigate to login page
                    await App.Current.MainPage.DisplayAlert("Exist", "Account already exists. Please Sign In.", "OK");
                    // Navigate to your login page here
                    await this._navigation.PushModalAsync(new LoginView());
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
