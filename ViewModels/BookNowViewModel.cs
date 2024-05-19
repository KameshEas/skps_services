using Acr.UserDialogs;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using skps_services.Views;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using BookingDetails = skps_services.Models.BookingDetails;

namespace skps_services.ViewModels
{
    public class BookNowViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyC8q_AFMR9VeYAKJ0ld6CQNLPTscbdgP0s";
        public string Uri = "https://skps-66b64-default-rtdb.firebaseio.com";
        private FirebaseClient _firebaseClient;
        private INavigation _navigation;
        private string name;
        private string email;
        private string mobileNumber;
        private string address;
        private string city;
        private string state;
        private string pincode;
        private string service;
        private string selectedService;

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
        public string Service
        {
            get => service; set
            {
                service = value;
                RaisePropertyChanged("Service");
            }
        }
        public string SelectedService
        {
            get { return selectedService; }
            set
            {
                selectedService = value;
                RaisePropertyChanged(nameof(SelectedService));
            }
        }


        public Command BookNow { get; }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public async Task PostDataAsync(string name, string email, string mobileNumber, string address, string city, string state, string pincode, string selectedService)
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
        }

        private async void BookNowTappedAsync(object obj)
        {
            try
            {
                // Create a new BookingDetails object with the provided information
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
                UserDialogs.Instance.Loading();


                Console.WriteLine("Booking Details Written to the Firebase successfully");
                // Send email with the booking details
                await SendEmailAsync(Email, "Booking Confirmation", bookingDetails);
                
                UserDialogs.Instance.HideLoading();


                await App.Current.MainPage.DisplayAlert("Alert", "Booking successful. Check the mail", "OK");
            }
            catch (FirebaseAuthException ex)
            {
                // Handle authentication errors
                await App.Current.MainPage.DisplayAlert("Alert", "Error: " + ex.Reason.ToString(), "OK");
            }
            catch (Exception ex)
            {
                // Handle other errors
                await App.Current.MainPage.DisplayAlert("Alert", "Error: " + ex.Message, "OK");
            }
        }


        private async Task SendEmailAsync(string to, string subject, BookingDetails bookingDetails)
        {
            try
            {
                // Construct the email body with all the booking details
                string body = "<h2>" + "Dear Customer your booking details are as follows" + "</h2>" +
                              "<p><strong>Name:</strong> " + bookingDetails.Name + "</p>" +
                              "<p><strong>Email:</strong> " + bookingDetails.Email + "</p>" +
                              "<p><strong>Mobile Number:</strong> " + bookingDetails.MobileNumber + "</p>" +
                              "<p><strong>Address:</strong> " + bookingDetails.Address + "</p>" +
                              "<p><strong>City:</strong> " + bookingDetails.City + "</p>" +
                              "<p><strong>State:</strong> " + bookingDetails.State + "</p>" +
                              "<p><strong>Pincode:</strong> " + bookingDetails.Pincode + "</p>" +
                              "<p><strong>Service:</strong> " + bookingDetails.SelectedService + "</p>" +
                              "<p>Thanks for choosing Sri Kannigaparameshwari Services. Share us your valuable reviews</p>";

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
