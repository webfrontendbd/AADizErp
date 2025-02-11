using AADizErp.Services;
using AADizErp.Models.Dtos;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.ApplicationModel;

namespace AADizErp.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string fullName;
        [ObservableProperty]
        private bool isApprovalVisible;
        [ObservableProperty]
        private string organization;
        private string _deviceToken;

        private readonly AuthenticationService _authService;
        private readonly AttendanceService _attnService;
        private readonly LeaveService _leaveService;
        private readonly DeviceTokenService _tokenService;
        private readonly INotificationCounter _notificationCounter;
        public MainPageViewModel(
            AuthenticationService authService,
            DeviceTokenService tokenService,
            AttendanceService attndance,
            INotificationCounter notificationCounter,
            LeaveService leaveService
            )
        {
            _authService = authService;
            _tokenService = tokenService;
            _attnService = attndance;
            _leaveService = leaveService;
            _notificationCounter = notificationCounter;
            LoadUserInformation();
        }

        public async void LoadUserInformation()
        {
            //CheckAuthentication();
            int totalCount = 0;
            if (Preferences.ContainsKey("DeviceToken"))
            {
                _deviceToken = Preferences.Get("DeviceToken", "");
            }
            int count = await _attnService.GetAttendanceRequestSeenDataAsync();
            int leaveCount = await _leaveService.GetLeaveRequestSeenDataAsync();

            if (count > 0 || leaveCount>0)
            {
                totalCount += count;
                totalCount += leaveCount;
                IsApprovalVisible = true;                
            }
            else 
            { 
                IsApprovalVisible= false;
            }

            _notificationCounter.SetNotificationCount(totalCount);
            var user = await App.GetUserInfo();
            if (user != null)
            {
                var lowerCase = user.TokenUserMetaInfo.OrganizationName.ToLower();
                // matches the first sentence of a string, as well as subsequent sentences
                var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
                // MatchEvaluator delegate defines replacement of setence starts to uppercase
                var result = r.Replace(lowerCase, s => s.Value.ToUpper());

                FullName = user.TokenUserMetaInfo.Name;
                Organization = result;

                var dToken = await _tokenService.GetUserDeviceToken(user.TokenUserMetaInfo.UserName);
                if (String.IsNullOrEmpty(dToken) || String.CompareOrdinal(dToken, _deviceToken) != 0)
                {
                    _tokenService.AddDeviceToken(new DeviceTokenDto(user.TokenUserMetaInfo.UserName, _deviceToken));
                }
            }

            //Task.Run(async () =>
            //{                

            //    var user = await App.GetUserInfo();
            //    if (user != null)
            //    {
            //        var lowerCase = user.TokenUserMetaInfo.OrganizationName.ToLower();
            //        // matches the first sentence of a string, as well as subsequent sentences
            //        var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
            //        // MatchEvaluator delegate defines replacement of setence starts to uppercase
            //        var result = r.Replace(lowerCase, s => s.Value.ToUpper());

            //        FullName = user.TokenUserMetaInfo.Name;
            //        Organization = result;

            //        var dToken = await _tokenService.GetUserDeviceToken(user.TokenUserMetaInfo.UserName);
            //        if (String.IsNullOrEmpty(dToken) || String.CompareOrdinal(dToken, _deviceToken) != 0)
            //        {
            //            _tokenService.AddDeviceToken(new DeviceTokenDto(user.TokenUserMetaInfo.UserName, _deviceToken));
            //        }
            //    }
            //});

        }

        [RelayCommand]
        public async Task Logout()
        {
            _authService.Logout();
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
        }
        private async void CheckAuthentication()
        {
            //retrieve token
            bool isAuthenticated = Preferences.Default.Get("IsAuthenticated", false);
            if (!isAuthenticated)
            {
                await Shell.Current.GoToAsync($"///{nameof(LoginPage)}", true);
            }
            else
            {
                var token = await SecureStorage.GetAsync("Token");
                if (String.IsNullOrEmpty(token))
                {
                    await Shell.Current.GoToAsync($"///{nameof(LoginPage)}", true);
                }
                else
                {
                    var jsonToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
                    if (jsonToken.ValidTo < DateTime.UtcNow)
                    {
                        SecureStorage.Remove("Token");
                        await Shell.Current.GoToAsync($"///{nameof(LoginPage)}", true);
                    }
                }
            }
        }

    }
}
