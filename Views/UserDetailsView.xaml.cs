using skps_services.ViewModels;

namespace skps_services.Views;

public partial class UserDetailsView : ContentPage
{
	public UserDetailsView(string uid)
	{
		InitializeComponent();

        // Set the BindingContext to an instance of UserDetailsViewModel
        BindingContext = new UserDetailsViewModel(uid);
    }
    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }
}