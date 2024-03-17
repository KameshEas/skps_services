using Firebase.Auth;
using Newtonsoft.Json;
using skps_services.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace skps_services.ViewModels
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyC8q_AFMR9VeYAKJ0ld6CQNLPTscbdgP0s";
        private INavigation _navigation;
        private string email;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action UserLoggedIn;  // Event to be raised when the user is logged in

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

            if (IsUserLoggedIn())
            {
                // Raise the UserLoggedIn event
                UserLoggedIn?.Invoke();
            }
        }

        private async void LoginBtnTappedAsync(object obj)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FreshFirebaseToken", serializedContent);

                // Raise the UserLoggedIn event
                UserLoggedIn?.Invoke();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Incorrect Login Credentials. Please try again!!", "OK");
            }
        }

        private bool IsUserLoggedIn()
        {
            // Get the stored token
            var token = Preferences.Get("FreshFirebaseToken", string.Empty);

            // If there is no token, the user is not logged in
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            // TODO: Validate the token with your backend service

            // If the token is valid, the user is logged in
            return true;
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
