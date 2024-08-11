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
using Newtonsoft.Json.Linq;
using Android.Locations;

namespace skps_services.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private const string WebApiKey = AppConstant.WebApiKey;
        private readonly INavigation _navigation;
        private string email;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command ResetNowBtn { get; }
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
                RaisePropertyChanged(nameof(IsFormValid)); // Trigger validation

            }
        }

        public bool IsFormValid =>
                          !string.IsNullOrEmpty(Email) &&
                          !string.IsNullOrEmpty(Password);
        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            ResetNowBtn = new Command(async () => await ResetNowBtnTappedAsync());
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
            if (!IsFormValid)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Invalid","Please enter all fields", "Ok");
                return;
            }
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("No Internet", "Internet connection is required to login.", "OK");
                return;
            }
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

                UserDialogs.Instance.ShowLoading("Logging In");
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                var content = await auth.GetFreshAuthAsync();

                var serializedContent = JsonConvert.SerializeObject(content);
                var expiryDate = DateTime.UtcNow.AddSeconds(content.ExpiresIn);

                // Check and save the token
                if (!string.IsNullOrEmpty(AppConstant.FirebaseTokenKey) && !string.IsNullOrEmpty(serializedContent))
                {
                    await SecureStorage.SetAsync(AppConstant.FirebaseTokenKey, serializedContent);
                }

                // Check and save the expiry date
                if (!string.IsNullOrEmpty(AppConstant.TokenExpiryKey) && !string.IsNullOrEmpty(expiryDate.ToString("o")))
                {
                    await SecureStorage.SetAsync(AppConstant.TokenExpiryKey, expiryDate.ToString("o"));
                }

                var jsonObject = JObject.Parse(serializedContent);
                string idToken = jsonObject["idToken"]?.ToString();
                string expiresIn = jsonObject["expiresIn"]?.ToString();

                AppConstant.IdToken = idToken;
                AppConstant.Expiry = expiryDate.ToString("o");

                await SecureStorage.SetAsync(AppConstant.IdToken, idToken);
                await SecureStorage.SetAsync(AppConstant.Expiry, expiryDate.ToString("o"));

                Console.WriteLine("Token stored: " + idToken);
                Console.WriteLine("Token expiry: " + expiryDate);

                var userClient = new FirebaseClient(AppConstant.FirebaseUri);
                var userDetails = await userClient
                    .Child("User")
                    .Child(content.User.LocalId)
                    .OnceSingleAsync<UserNew>();

                if (userDetails != null)
                {
                    Console.WriteLine($"User Details Retrieved: {JsonConvert.SerializeObject(userDetails)}");
                    userDetails.PhotoUrl = userDetails.PhotoUrl ?? "profile_1.png";
                    if (userDetails.PhotoUrl == null)
                    {
                        Console.WriteLine("PhotoUrl is null");
                    }
                }
                else
                {
                    Console.WriteLine("User details are null");
                }

                if (userDetails != null)
                {
                    var serializedUserDetails = JsonConvert.SerializeObject(userDetails);
                    AppConstant.UserDetailsKey = serializedUserDetails; 

                    await SecureStorage.SetAsync(AppConstant.UserDetailsKey, serializedUserDetails);
                    StoreUserDetails(userDetails);
                }
                else
                {
                    Console.WriteLine("userDetails is null");
                    await App.Current.MainPage.DisplayAlert("Details not found", "User details could not be retrieved.", "OK");
                }


                AppConstant.UserName = userDetails.DisplayName;
                AppConstant.UserEmail = userDetails.Email;
                AppConstant.UserMobileNumber = userDetails.MobileNumber;

                await _navigation.PushModalAsync(new HomeView());
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Invalid Credentials", "Invalid username or password", "OK");
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
        private async Task ResetNowBtnTappedAsync()
        {
            await _navigation.PushAsync(new ForgotPasswordView());
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
            UserStore.PhotoUrl = user.PhotoUrl ?? "profile_1.png";
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
