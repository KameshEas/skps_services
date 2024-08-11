using skps_services.Constants;
using System.Globalization;
using System.Windows.Input;

namespace skps_services.Views;

public partial class ServicesView : ContentPage
{
    string uid = UserStore.LocalId;

    public ServicesView()
	{
		InitializeComponent();

        var selectedLanguage = AppConstant.SelectedLanguage;
        CultureInfo culture;

        if (selectedLanguage == "Tamil")
        {
            culture = new CultureInfo("ta");
        }
        else
        {
            culture = new CultureInfo("en");
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    private async void Profile_tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ProfileView(uid));

    }

    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }

    private async void BookNow_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new BookNowView());

    }
}