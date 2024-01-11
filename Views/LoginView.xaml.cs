namespace skps_services.Views;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
		InitializeComponent();
        //BindingContext = new LoginViewModel(Navigation);
    }

	private async void SignUp_Clicked(object sender, EventArgs e)
	{
        SignUpView signUpPage = new SignUpView(); // Create an instance of SignUpView
        await Navigation.PushAsync(signUpPage); // Navigate to SignUpView
    }
}