using skps_services.ViewModels;

namespace skps_services.Views;

public partial class ForgotPasswordView : ContentPage
{
	public ForgotPasswordView()
	{
		InitializeComponent();
        BindingContext = new ForgotPasswordViewModel();
    }
    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}