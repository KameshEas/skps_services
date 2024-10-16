﻿using Acr.UserDialogs;
using Firebase.Auth;
using skps_services.Constants;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace skps_services.ViewModels
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private readonly INavigation _navigation;
        public ICommand SendPasswordResetCommand { get; private set; }
        private string email;
        public event PropertyChangedEventHandler PropertyChanged;


        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ForgotPasswordViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            SendPasswordResetCommand = new Command(async () => await SendPasswordResetEmail(Email));
        }

        public async Task SendPasswordResetEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email))
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
                await authProvider.SendPasswordResetEmailAsync(Email);
                UserDialogs.Instance.HideLoading();
                Console.WriteLine("Password reset email sent successfully.");
                UserDialogs.Instance.Alert("Reset mail sent to your mail", "Password Reset", "Ok");
                await _navigation.PopAsync();

            }
            catch (FirebaseAuthException e)
            {
                Console.WriteLine("Error: " + e.Reason);
            }
        }
    }
}
