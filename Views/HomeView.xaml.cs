using skps_services.Constants;
using System.Globalization;

namespace skps_services.Views;

public partial class HomeView : ContentPage
{
    string uid = UserStore.LocalId;

    public HomeView()
	{
		InitializeComponent();
	}
    protected override bool OnBackButtonPressed()
    {
        return false;
    }

    private async void BookService_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ServicesView());

    }
    private async void Profile_tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ProfileView(uid));

    }

    private void OnLanguageChanged(object sender, EventArgs e)
    {
        var selectedLanguage = LanguagePicker.SelectedItem.ToString();
        var culture = new CultureInfo(selectedLanguage);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        Application.Current.MainPage = new HomeView();
    }
}