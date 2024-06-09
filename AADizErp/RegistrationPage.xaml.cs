using AADizErp.ViewModels;
using DevExpress.Maui.Editors;

namespace AADizErp;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegistrationPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    private async void SigninButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
    }
}