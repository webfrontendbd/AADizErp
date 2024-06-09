using AADizErp.Models;
using AADizErp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Biometric;

namespace AADizErp.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authService;
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;
        [ObservableProperty]
        bool isBusy;

        private string _buttonText = "Login";
        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        public LoginViewModel(AuthenticationService authService)
        {
            Title="Login";
            _authService=authService;
        }

        [RelayCommand]
        async Task FingerPrintAuthentication()
        {
            var biometricResult = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
            {
                Title="Use fingerprint to login",
                NegativeText="Cancel Authentication"
            }, CancellationToken.None);

            if (biometricResult.Status == BiometricResponseStatus.Success)
            {
                var token = await SecureStorage.GetAsync("Token");
                if (String.IsNullOrEmpty(token))
                {
                    IsBusy=false;
                    await Shell.Current.DisplayAlert("Token Missing!", "Please Login by username and password first", "OK");
                }
                else
                {
                    await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
                }
            }
            else
            {
                IsBusy=false;
                await Shell.Current.DisplayAlert("Not authenticated!", "Access denied", "OK");
            }

        }

        [RelayCommand]
        async Task Login()
        {
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(Username)||string.IsNullOrWhiteSpace(Password))
            {
                await DisplayLoginMessage("Invalid Username or Password");
            }
            else
            {
                try
                {
                    var loginDto = new LoginDto(Username, Password);
                    var response = await _authService.Login(loginDto);
                    if (!string.IsNullOrEmpty(response.Token))
                    {
                        Preferences.Default.Set("IsAuthenticated", true);
                        // Clear the existing token if it exists
                        SecureStorage.Remove("Token");
                        await SecureStorage.SetAsync("Token", response.Token);
                        await Shell.Current.GoToAsync($"///{nameof(MainPage)}", true);
                    }
                    else
                    {
                        await DisplayLoginMessage(_authService.StatusMessage);
                    }
                }
                catch (Exception ex)
                {
                    await DisplayLoginMessage(ex.Message);
                }
                
            }
            IsBusy = false;
        }
              
        async Task DisplayLoginMessage(string message)
        {
            await Shell.Current.DisplayAlert("Login Attemp Result", message, "OK");
            Password = string.Empty;
        }
    }
}
