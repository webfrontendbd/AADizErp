
using System.IdentityModel.Tokens.Jwt;

namespace AADizErp.ViewModels
{
    public partial class LoadingViewModel : BaseViewModel
    {
        public LoadingViewModel()
        {
            CheckUserLoginDetails();
        }
        private async void CheckUserLoginDetails()
        {
            //retrieve token
            bool isAuthenticated = Preferences.Default.Get("IsAuthenticated", false);
            if (!isAuthenticated)
            {
                await GoToLoginPage();
            }
            else
            {
                var token = await SecureStorage.GetAsync("Token");
                if (String.IsNullOrEmpty(token))
                {
                    await GoToLoginPage();
                }
                else
                {
                    try
                    {
                        var jsonToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                        if (jsonToken.ValidTo < DateTime.UtcNow)
                        {
                            SecureStorage.Remove("Token");
                            await GoToLoginPage();
                        }
                        else
                        {
                            await GoToHomePage();
                        }
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Oops!", ex.Message, "OK");
                    }
                    
                }
            }
        }

        private async Task GoToLoginPage()
        {
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}", true);
        }

        private async Task GoToHomePage()
        {
            await Shell.Current.GoToAsync($"///{nameof(MainPage)}", true);
        }
    }
}
