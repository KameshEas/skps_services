using System.Collections.ObjectModel;
using System.Windows.Input;

namespace skps_services.ViewModels;

public class ContactOption
{
    public string Title { get; set; }
    public string Detail { get; set; }
    public string Icon { get; set; }
}

public class ContactViewModel
{
    public ObservableCollection<ContactOption> ContactOptions { get; set; }

    public ContactViewModel()
    {
        ContactOptions = new ObservableCollection<ContactOption>
        {
            new ContactOption() { Title = "Call us", Detail = "+91 7708589128", Icon = "mobile.png" },
            new ContactOption() { Title = "Email us", Detail = "skpsmobapp@gmail.com", Icon = "email.png" },
            new ContactOption() { Title = "Chat with us", Detail = "+91 7708589128", Icon = "chat.png" },
        };
    }

   

}