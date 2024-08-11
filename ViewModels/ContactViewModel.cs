using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace skps_services.ViewModels;

public class ContactOption
{
    public string Title { get; set; }
    public string Detail { get; set; }
    public string Icon { get; set; }
    public string Font { get; set; }
}

public class ContactViewModel
{
    public ObservableCollection<ContactOption> ContactOptions { get; set; }

    public ContactViewModel()
    {
        if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ta")
        {
            ContactOptions = new ObservableCollection<ContactOption>
            {
                new ContactOption() { Title = "எங்களை அழைக்க", Detail = "+91 7708589128", Icon = "mobile.png", Font = "Vanavil-Regular" },
                new ContactOption() { Title = "மின்னஞ்சல் அனுப்ப", Detail = "skpsmobapp@gmail.com", Icon = "email.png", Font = "Vanavil-Regular" },
                new ContactOption() { Title = "எங்களுடன் பேசுங்கள்", Detail = "+91 7708589128", Icon = "chat.png", Font = "Vanavil-Regular" },
            };
        }
        else
        {
            ContactOptions = new ObservableCollection<ContactOption>
            {
                new ContactOption() { Title = "Call us", Detail = "+91 7708589128", Icon = "mobile.png" },
                new ContactOption() { Title = "Email us", Detail = "skpsmobapp@gmail.com", Icon = "email.png" },
                new ContactOption() { Title = "Chat with us", Detail = "+91 7708589128", Icon = "chat.png" },
            };
        }
    }

   

}