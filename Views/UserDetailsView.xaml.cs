namespace skps_services.Views;

public partial class UserDetailsView : ContentPage
{
	public UserDetailsView()
	{
		InitializeComponent();
	}
    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }
}