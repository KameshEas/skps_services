using Java.Security;
using System.Windows.Input;

namespace skps_services.Views;

public partial class ProfileView : ContentPage
{
    private string _uid;
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
        if (string.IsNullOrEmpty(_uid))
        {
            await DisplayAlert("Error", "User ID is missing", "OK");
            return;
        }
        await Navigation.PushModalAsync(new UserDetailsView(_uid));
    }

    private async void Password_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPasswordView());
    }

    private async void Services_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ServicesView());

    }

    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }

    private async void Logout_Clicked(object sender, EventArgs e)
    {
        Preferences.Remove("FreshFirebaseToken");
        Preferences.Remove("TokenExpiry");
        await Navigation.PushModalAsync(new LoginView());
    }
}