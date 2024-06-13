using AADizErp.Models;
using AADizErp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AADizErp.ViewModels.SettingsVM
{
    public partial class PasswordChangePageViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authService;
        [ObservableProperty]
        string oldPassword;
        [ObservableProperty]
        string newPassword;
        [ObservableProperty]
        string confirmPassword;

        public PasswordChangePageViewModel(AuthenticationService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        async Task ChangedUserPassword()
        {
            if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Shell.Current.DisplayAlert("Error!", "Please fill the every password field", "OK");
                return;
            }
            else
            {
                if (NewPassword != ConfirmPassword)
                {
                    await Shell.Current.DisplayAlert("Error!", "Confirm password not match", "OK");
                    return;
                }
                else
                {
                    var expectedPasswordPattern = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$!%*?&])[A-Za-z\d@#$!%*?&]{8,}$");
                    var isValidPassword = expectedPasswordPattern.IsMatch(NewPassword);
                    if (isValidPassword)
                    {
                        var user = await App.GetUserInfo();
                        var loginDto = new LoginDto(user.Username, NewPassword);
                        var response = await _authService.ChangedUserPassword(loginDto);
                        if (response.Succeeded)
                        {
                            await Shell.Current.DisplayAlert("Success!", "Your password changed successfully!", "OK");
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Error", "Something went wrong!", "OK");
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Password Error", "Minimum 8 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character", "OK");
                    }
                }
            }
        }
    }
}
