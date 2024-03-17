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

    private async void BookServices_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ServicesView());


    }
}