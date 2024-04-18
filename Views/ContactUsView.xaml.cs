using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Maps;

namespace skps_services.Views;

public partial class ContactUsView : ContentPage
{

    public ContactUsView()
	{
		InitializeComponent();
    }

    private async void Back_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();

    }
}