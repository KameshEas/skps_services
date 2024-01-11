namespace skps_services.Views;

public partial class SignUpView : ContentPage
{
	public SignUpView()
	{
		InitializeComponent();
	}
    private async void TapGestureRecognizer_Tapped_For_SignIn(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SignIn");
    }
}