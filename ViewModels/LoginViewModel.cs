using Acr.UserDialogs;
using Firebase.Auth;
using Newtonsoft.Json;
using skps_services.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace skps_services.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyC8q_AFMR9VeYAKJ0ld6CQNLPTscbdgP0s";
        private INavigation _navigation;
        private string email;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;
        // public event Action UserLoggedIn;  // Event to be raised when the user is logged in

        public Command SignUpBtn { get; }
        public Command LoginBtn { get; }

        public string Email
        {
            get => email; set
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

        public LoginViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            SignUpBtn = new Command(RegisterBtnTappedAsync);
            LoginBtn = new Command(LoginBtnTappedAsync);
        }

        private async void LoginBtnTappedAsync(object obj)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
            try
            {
                UserDialogs.Instance.ShowLoading();
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                var content = await auth.GetFreshAuthAsync();

                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FreshFirebaseToken", serializedContent);

                var expiryDate = DateTime.UtcNow.AddSeconds(content.ExpiresIn);
                Preferences.Set("TokenExpiry", expiryDate.ToString());

                // Debug logs
                Console.WriteLine("Token stored: " + serializedContent);
                Console.WriteLine("Token expiry: " + expiryDate.ToString());

                await _navigation.PushModalAsync(new HomeView()); // Navigate to the main page after successful login
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await App.Current.MainPage.DisplayAlert("Invalid", "Incorrect Login Credentials. Please try again!!", "OK");
            }
        }

        private async void RegisterBtnTappedAsync(object obj)
        {
            await this._navigation.PushAsync(new SignUpView());
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
