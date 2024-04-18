using System.Windows.Input;

namespace skps_services.Views;

public partial class ServicesView : ContentPage
{
    public ServicesView()
	{
		InitializeComponent();
	}

    private async void Profile_tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ProfileView());

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