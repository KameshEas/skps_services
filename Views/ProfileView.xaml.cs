using Java.Security;
using System.Windows.Input;

namespace skps_services.Views;

public partial class ProfileView : ContentPage
{
    public ProfileView()
	{
		InitializeComponent();

	}

    private async void ContactUs_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ContactUsView());
    }

    private async void UserDetails_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new UserDetailsView());
    }

    private async void Password_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new PasswordView());
    }

    private async void Services_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ServicesView());

    }

    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }
}