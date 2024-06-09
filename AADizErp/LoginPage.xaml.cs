using AADizErp.ViewModels;

namespace AADizErp;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}

  //  private async void SignupButton_Clicked(object sender, EventArgs e)
  //  {
		//try
		//{
		//	await Shell.Current.GoToAsync($"{nameof(RegistrationPage)}");

		//}
		//catch (Exception ex)
		//{
		//	await Shell.Current.DisplayAlert("Oops!", ex.Message, "OK");
		//}
  //  }
}