using System.IdentityModel.Tokens.Jwt;

namespace AADizErp.ViewModels
{
    public partial class LoadingViewModel : BaseViewModel
    {
        public LoadingViewModel()
        {
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                // 1. Check simple auth flag
                if (!Preferences.Default.Get("IsAuthenticated", false))
                {
                    await GoToLoginPage();
                    return;
                }

                // 2. Retrieve token
                var token = await SecureStorage.GetAsync("Token");
                if (string.IsNullOrWhiteSpace(token))
                {
                    await GoToLoginPage();
                    return;
                }

                // 3. Validate JWT token expiration
                if (!IsTokenValid(token))
                {
                    SecureStorage.Remove("Token");
                    await GoToLoginPage();
                    return;
                }

                // 4. Token is valid → go home
                await GoToHomePage();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Oops!", ex.Message, "OK");
                await GoToLoginPage(); // fallback
            }
        }

        private bool IsTokenValid(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadToken(token) as JwtSecurityToken;

                if (jwt == null)
                    return false;

                return jwt.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }

        private Task GoToLoginPage() =>
            Shell.Current.GoToAsync($"///{nameof(LoginPage)}", true);

        private Task GoToHomePage() =>
            Shell.Current.GoToAsync($"///{nameof(MainPage)}", true);
    }
}
