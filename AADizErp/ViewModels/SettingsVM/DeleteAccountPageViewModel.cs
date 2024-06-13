using AADizErp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADizErp.ViewModels.SettingsVM
{
    public partial class DeleteAccountPageViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authService;
        [ObservableProperty]
        string username;
        public DeleteAccountPageViewModel(AuthenticationService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        async Task DeleateUserAccount()
        {
            if (!String.IsNullOrWhiteSpace(Username))
            {
                bool response = await _authService.DeleteUserAccount(Username);
                if (response)
                {
                    _authService.Logout();
                    await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error!", "Enter Username and Try Again!", "OK");
            }

        }
    }
}
