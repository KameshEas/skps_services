using skps_services.Constants;
using skps_services.ViewModels;
using System.Globalization;

namespace skps_services.Views;

public partial class BookNowView : ContentPage
{
    string uid = UserStore.LocalId;
    public BookNowView()
	{
		InitializeComponent();
        BindingContext = new BookNowViewModel(Navigation);

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
}