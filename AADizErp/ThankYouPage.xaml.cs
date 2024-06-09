namespace AADizErp;

public partial class ThankYouPage : ContentPage
{
    public ThankYouPage()
    {
        InitializeComponent();
    }

    private async void GoToButton_Clicked(object sender, EventArgs e)
    {
        bool isAuthenticated = Preferences.Default.Get("IsAuthenticated", false);
        if (!isAuthenticated)
        {
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"///{nameof(MainPage)}");

        }
    }
}