using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using skps_services.Constants;
using skps_services.Models;
using static skps_services.Constants.AppConstant;

namespace skps_services.ViewModels
{
    public class UserDetailsViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseClient _firebaseClient;
        private UserNew _user;
        private string _uid;

        public UserNew User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
                if (_user != null)
                {
                    // Update UserStore with user details
                    UserStore.LocalId = _user.LocalId;
                    UserStore.FederatedId = _user.FederatedId;
                    UserStore.FirstName = _user.FirstName;
                    UserStore.LastName = _user.LastName;
                    UserStore.DisplayName = _user.DisplayName;
                    UserStore.Email = _user.Email;
                    UserStore.EmailVerified = _user.EmailVerified;
                    UserStore.PhotoUrl = _user.PhotoUrl;
                }
            }
        }

        public UserDetailsViewModel(string uid)
        {
            _firebaseClient = new FirebaseClient(AppConstant.FirebaseUri);
            _uid = UserStore.LocalId;
            LoadUserDetails();
        }

        private async Task<bool> NodeExists(string nodeName, string childId)
        {
            try
            {
                var data = await _firebaseClient
                    .Child(nodeName)
                    .Child(childId)
                    .OnceSingleAsync<object>();

                return data != null;
            }
            catch
            {
                return false;
            }
        }

        private async Task LoadUserDetails()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_uid))
                {
                    Console.WriteLine("User ID is missing.");
                    return;
                }

                bool userNodeExists = await NodeExists("UserNew", _uid);
                if (userNodeExists)
                {
                    User = await _firebaseClient
                        .Child("UserNew")
                        .Child(_uid)
                        .OnceSingleAsync<UserNew>();
                }
                else
                {
                    Console.WriteLine("The 'UserNew' node does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user details: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
