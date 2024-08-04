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
using FirebaseAuthLinkFirebase = Firebase.Auth.FirebaseAuthLink;
using FirebaseAuthLinkCustom = skps_services.Models.FirebaseAuthLink;
using static skps_services.Constants.AppConstant;

namespace skps_services.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private const string WebApiKey = AppConstant.WebApiKey;
        private readonly INavigation _navigation;
        private string email;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command SignUpBtn { get; }
        public Command LoginBtn { get; }

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

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SignUpBtn = new Command(async () => await RegisterBtnTappedAsync());
            LoginBtn = new Command(async () => await LoginBtnTappedAsync());
        }

        public async Task InitializeAsync()
        {
            var serializedContent = await SecureStorage.GetAsync(AppConstant.FirebaseTokenKey);
            if (!string.IsNullOrEmpty(serializedContent))
            {
                var content = JsonConvert.DeserializeObject<FirebaseAuthLinkFirebase>(serializedContent);
                var expiryDateString = await SecureStorage.GetAsync(AppConstant.TokenExpiryKey);
                if (DateTime.TryParse(expiryDateString, out DateTime expiryDate) && DateTime.UtcNow < expiryDate)
                {
                    await _navigation.PushModalAsync(new HomeView());
                    return;
                }
            }
            await _navigation.PushAsync(new LoginView());
        }

        private async Task LoginBtnTappedAsync()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            try
            {
                UserDialogs.Instance.ShowLoading();
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                var content = await auth.GetFreshAuthAsync();

                var serializedContent = JsonConvert.SerializeObject(content);
                await SecureStorage.SetAsync(AppConstant.FirebaseTokenKey, serializedContent);

                var expiryDate = DateTime.UtcNow.AddSeconds(content.ExpiresIn);
                await SecureStorage.SetAsync(AppConstant.TokenExpiryKey, expiryDate.ToString());

                Console.WriteLine("Token stored: " + serializedContent);
                Console.WriteLine("Token expiry: " + expiryDate.ToString());

                var userClient = new FirebaseClient(AppConstant.FirebaseUri);
                var userDetails = await userClient
                    .Child("User")
                    .Child(content.User.LocalId)
                    .OnceSingleAsync<UserNew>();

                StoreUserDetails(userDetails);

                await _navigation.PushModalAsync(new HomeView());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Invalid", "Incorrect Login Credentials. Please try again!!", "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task RegisterBtnTappedAsync()
        {
            await _navigation.PushAsync(new SignUpView());
        }

        private void StoreUserDetails(UserNew user)
        {
            UserStore.LocalId = user.LocalId;
            UserStore.FederatedId = user.FederatedId;
            UserStore.FirstName = user.FirstName;
            UserStore.LastName = user.LastName;
            UserStore.DisplayName = user.DisplayName;
            UserStore.Email = user.Email;
            UserStore.EmailVerified = user.EmailVerified;
            UserStore.PhotoUrl = user.PhotoUrl;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
