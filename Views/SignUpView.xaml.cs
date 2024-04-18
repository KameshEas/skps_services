using Firebase.Database;
using skps_services.ViewModels;

namespace skps_services.Views;

public partial class SignUpView : ContentPage
{
    
    public SignUpView()
	{
		InitializeComponent();
        BindingContext = new SignUpViewModel(Navigation);

    }
    protected override bool OnBackButtonPressed()
    {
        return false;
    }

    private async void Sign_In_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoginView());
    }

}