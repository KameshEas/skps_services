using skps_services.ViewModels;

namespace skps_services.Views;

public partial class LoginView : ContentPage
{
    private LoginViewModel viewModel;

    public LoginView()
	{
		InitializeComponent();
        viewModel = new LoginViewModel(Navigation);
        BindingContext = viewModel;

        // viewModel.UserLoggedIn += OnUserLoggedIn;
        Navigation.PushModalAsync(new HomeView());
    }
    private async void OnUserLoggedIn()
    {
        await Navigation.PushModalAsync(new HomeView());
    }
    protected override bool OnBackButtonPressed()
    {
        return false;
    }

    private async void SignUp_Clicked(object sender, EventArgs e)
	{
        await Navigation.PushModalAsync(new SignUpView());
    }
}