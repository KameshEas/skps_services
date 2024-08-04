using skps_services.Constants;
using System.Windows.Input;

namespace skps_services.Views;

public partial class ServicesView : ContentPage
{
    string uid = UserStore.LocalId;

    public ServicesView()
	{
		InitializeComponent();
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