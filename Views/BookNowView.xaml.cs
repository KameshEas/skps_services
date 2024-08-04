using skps_services.Constants;
using skps_services.ViewModels;

namespace skps_services.Views;

public partial class BookNowView : ContentPage
{
    string uid = UserStore.LocalId;
    public BookNowView()
	{
		InitializeComponent();
        BindingContext = new BookNowViewModel(Navigation);

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