namespace skps_services.Views;

public partial class ForgotPasswordView : ContentPage
{
	public ForgotPasswordView()
	{
		InitializeComponent();
	}
    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }
}