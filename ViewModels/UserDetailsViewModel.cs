using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using skps_services.Models;

namespace skps_services.ViewModels
{
    public class UserDetailsViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseClient _firebaseClient;
        private User _user;
        private string _uid;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public UserDetailsViewModel()
        {
            _firebaseClient = new FirebaseClient("https://skps-66b64-default-rtdb.firebaseio.com");
            LoadUserDetails();
        }

        public UserDetailsViewModel(string uid) : this()
        {
            _uid = uid;
        }

        private async Task<bool> NodeExists(string nodeName)
        {
            var data = await _firebaseClient
                .Child(nodeName)
                .OnceSingleAsync<object>();

            return data != null;
        }

        private async Task LoadUserDetails()
        {
            try
            {
                bool userNodeExists = await NodeExists("User");
                if (userNodeExists)
                {
                    User = await _firebaseClient
                        .Child("User")
                        .Child(_uid) // Load user data based on the provided uid
                        .OnceSingleAsync<User>();
                }
                else
                {
                    Console.WriteLine("The 'User' node does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}