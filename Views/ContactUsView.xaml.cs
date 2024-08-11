using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Maps;
using skps_services.Constants;
using System.Globalization;

namespace skps_services.Views;

public partial class ContactUsView : ContentPage
{

    public ContactUsView()
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

    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }
}