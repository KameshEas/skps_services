using skps_services.Constants;
using skps_services.ViewModels;
using System.Globalization;

namespace skps_services.Views;

public partial class ForgotPasswordView : ContentPage
{
	public ForgotPasswordView()
	{
		InitializeComponent();
        BindingContext = new ForgotPasswordViewModel(Navigation);

    }
    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }
}