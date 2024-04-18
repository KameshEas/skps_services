namespace skps_services.Views;

public partial class HomeView : ContentPage
{
	public HomeView()
	{
		InitializeComponent();
	}
    protected override bool OnBackButtonPressed()
    {
        return false;
    }

    private async void BookService_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ServicesView());

    }
    private async void Profile_tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ProfileView());

    }
}