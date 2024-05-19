
namespace skps_services.Views;

public partial class GetStartedView : ContentPage
{
    public GetStartedView() => InitializeComponent();

    private async void GetStarted_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PushModalAsync(new LoginView());
	}
}