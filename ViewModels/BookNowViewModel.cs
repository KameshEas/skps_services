using Acr.UserDialogs;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using skps_services.Constants;
using skps_services.Views;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using BookingDetails = skps_services.Models.BookingDetails;

namespace skps_services.ViewModels
{
    public class BookNowViewModel : INotifyPropertyChanged
    {
        public string webApiKey = AppConstant.WebApiKey;
        public string Uri = AppConstant.FirebaseUri;
        private FirebaseClient _firebaseClient;
        private INavigation _navigation;
        private string name;
        private string email;
        private string mobileNumber;
        private string address;
        private string city;
        private string state;
        private string pincode;
        private int service;
        private int selectedService;
        //private const double ShopLatitude = 12.60471248626709; // Replace with actual shop latitude        
        private const double ShopLatitude = 13.043585387863738; // Replace with actual Home latitude
        //private const double ShopLongitude = 80.05631256103516; // Replace with actual shop longitude
        private const double ShopLongitude = 80.1242771242679; // Replace with actual Home longitude
        private const double MaxDistanceKm = 10.0;

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
        public string Address
        {
            get => address; set
            {
                address = value;
                RaisePropertyChanged("Address");
                RaisePropertyChanged(nameof(IsFormValid));
            }
        }        
        public string City
        {
            get => city; set
            {
                city = value;
                RaisePropertyChanged("City");
            }
        }        
        public string State
        {
            get => state; set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }        
        public string Pincode
        {
            get => pincode; set
            {
                pincode = value;
                RaisePropertyChanged("Pincode");
            }
        }        
        public int Service
        {
            get => service;
            set
            {
                service = value;
                RaisePropertyChanged("Service");
            }
        }
        private int _selectedService;
        public int SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                RaisePropertyChanged(nameof(SelectedService));
                RaisePropertyChanged(nameof(IsFormValid)); // Trigger validation
            }
        }

        public bool IsFormValid => 
                           !string.IsNullOrEmpty(Address) &&
                           SelectedService>=0;

        public Command BookNow { get; }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public async Task PostDataAsync(string name, string email, string mobileNumber, string address, string city, string state, string pincode, int selectedService)
        {
            await _firebaseClient.Child("BookingDetails").PostAsync(new BookingDetails
            {
                Name = name,
                Email = email,
                MobileNumber = mobileNumber,
                Address = address,
                City = city,
                State = state,
                Pincode = pincode,
                Service = selectedService,
            });
        }

        public BookNowViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            _firebaseClient = new FirebaseClient(Uri);
            BookNow = new Command(BookNowTappedAsync);

            // Pre-fill with data from UserStore
            Name = AppConstant.UserName;
            Email = AppConstant.UserEmail;
            MobileNumber = AppConstant.UserMobileNumber;
        }

        private async void BookNowTappedAsync(object obj)
        {
            if (!IsFormValid)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast("Please enter all fields", TimeSpan.FromSeconds(2));
                return;
            }
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert("No Internet", "Internet connection is required to Book service.", "OK");
                return;
            }
            try
            {
                // Get the user's location
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));
                }

                if (location == null)
                {
                    await App.Current.MainPage.DisplayAlert("Turn On Location", "Unable to get your location. Please try again.", "OK");
                    return;
                }

                // Calculate the distance to the shop
                double distanceToShop = CalculateDistance(location.Latitude, location.Longitude, ShopLatitude, ShopLongitude);

                // Check if the user is within the 10 km radius
                if (distanceToShop > MaxDistanceKm)
                {
                    await App.Current.MainPage.DisplayAlert("Booking Failed", "Service is only available within 10 km of the shop.", "OK");
                    return;
                }

                var bookingDetails = new BookingDetails
                {
                    Name = name,
                    Email = email,
                    MobileNumber = mobileNumber,
                    Address = address,
                    City = city,
                    State = state,
                    Pincode = pincode,
                    Service = SelectedService
                };

                // Post the booking details to Firebase
                await PostDataAsync(Name, Email, MobileNumber, Address, City, State, Pincode, SelectedService);
                UserDialogs.Instance.ShowLoading("Booking Service");


                Console.WriteLine("Booking Details Written to the Firebase successfully");
                // Send email with the booking details
                await SendEmailAsync(Email, "Confirmation of Your Service Booking", bookingDetails);
                
                UserDialogs.Instance.HideLoading();


                await App.Current.MainPage.DisplayAlert("Booking successful", "Please Check the mail for Booking Confirmation", "OK");
            }
            catch (FirebaseAuthException ex)
            {
                // Handle authentication errors
                Console.WriteLine("Error: " + ex.Reason.ToString());
            }
            catch (Exception ex)
            {
                // Handle other errors
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Haversine formula to calculate distance between two points on the Earth
        private double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var d1 = latitude1 * (Math.PI / 180.0);
            var num1 = longitude1 * (Math.PI / 180.0);
            var d2 = latitude2 * (Math.PI / 180.0);
            var num2 = longitude2 * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6371.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }


        private async Task SendEmailAsync(string to, string subject, BookingDetails bookingDetails)
        {
            try
            {
                // Construct the email body with all the booking details
                string body = "<h2>" + "Dear "+ bookingDetails.Name +"</h2>" +
                              "<p>We are pleased to inform you that your service booking has been successfully confirmed!</p>" +
                              "<p><strong>Name:</strong> " + bookingDetails.Name + "</p>" +
                              "<p><strong>Email:</strong> " + bookingDetails.Email + "</p>" +
                              "<p><strong>Mobile Number:</strong> " + bookingDetails.MobileNumber + "</p>" +
                              "<p><strong>Address:</strong> " + bookingDetails.Address + "</p>" +
                              "<p><strong>City:</strong> " + bookingDetails.City + "</p>" +
                              "<p><strong>State:</strong> " + bookingDetails.State + "</p>" +
                              "<p><strong>Pincode:</strong> " + bookingDetails.Pincode + "</p>" +
                              "<p><strong>Service:</strong> " + bookingDetails.SelectedService + "</p>" +
                              "<p>If you need to make any changes or have any questions, please do not hesitate to contact us at "+AppConstant.ShopNumber+" or" + AppConstant.ShopEmail + ". We look forward to providing you with the best service experience.</p>" +
                              "<p>Thank you for choosing Sri Kannigaparameshwari Services. We appreciate your business and look forward to your valuable feedback.</p>";

                // Configure SMTP client
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential("skpsmobapp@gmail.com", "dohfbifkrjoilhoj");
                    smtpClient.EnableSsl = true;

                    // Create email message
                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("skpsmobapp@gmail.com");
                        mailMessage.To.Add(to);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;

                        // Send email
                        await smtpClient.SendMailAsync(mailMessage);

                        Console.WriteLine("Mail Sent Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error sending email: " + ex.Message);
                throw; // Re-throw the exception to propagate it to the caller
            }
        }



    }
}
