using AADizErp.Models.Dtos;
using AADizErp.Pages.ManagerPages;
using AADizErp.Pages.RequestPages;
using AADizErp.Services;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace AADizErp.ViewModels.ManagerPagesVM
{
    [QueryProperty(nameof(RemoteAttendanceDto), "RemoteAttendanceDto")]
    public partial class MgrAttendanceDetailsPageViewModel : BaseViewModel
    {
        IMap _map;
        private readonly AttendanceService _attnService;
        private readonly NotificationService _notify;

        public MgrAttendanceDetailsPageViewModel(IMap map, AttendanceService attnService, NotificationService notify)
        {
            _map=map;
            _attnService=attnService;
            _notify=notify;
        }

        [ObservableProperty]
        RemoteAttendanceDto remoteAttendanceDto;

        [RelayCommand]
        async Task AttendanceConfirmation(RemoteAttendanceDto attnDto)
        {
            if (attnDto == null) return;
            bool confirmation =  await Shell.Current.DisplayAlert("Confirmation", "Are you sure?", "Yes", "Decline");
            if (confirmation)
            {
               attnDto.Status ="Approved";
               var returnObject = await _attnService.AttendanceRequestApproval(attnDto);
                if (returnObject != null)
                {
                    RemoteAttendanceDto = returnObject;
                    //try
                    //{
                    //    //await _notify.SendAttendancePushNotificationBackToUser(returnObject);
                    //    //App.BadgeManager.Decrement();
                    //}
                    //catch
                    //{
                    //    await Shell.Current.DisplayAlert("Notification!", "We've sent a notification", "OK");
                    //}
                }
                
            }
            else
            {
                attnDto.Status ="Decline";
                var returnObject = await _attnService.AttendanceRequestApproval(attnDto);
                if (returnObject != null)
                {
                    RemoteAttendanceDto = returnObject;                    
                    //try
                    //{
                    //    await _notify.SendAttendancePushNotificationBackToUser(returnObject);
                    //    App.BadgeManager.Decrement();
                    //}
                    //catch
                    //{
                    //    await Shell.Current.DisplayAlert("Notification!", "We've sent a notification", "OK");
                    //}
                }
            }
            await Shell.Current.GoToAsync($"{nameof(ManagerViewAttnRequestPage)}");
        }

        [RelayCommand]
        async Task OpenAttendanceAreaMap()
        {
            try
            {
                await _map.OpenAsync(RemoteAttendanceDto.Latitude, RemoteAttendanceDto.Longitude, new MapLaunchOptions
                {
                    Name = RemoteAttendanceDto.FullName,
                    NavigationMode = NavigationMode.None
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to launch maps: {ex.Message}", "OK");
            }
        }

    }
}
