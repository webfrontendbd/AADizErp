using AADizErp.Models;
using AADizErp.Models.Dtos;
using AADizErp.Services;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Maui.ApplicationModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Core;
using MvvmHelpers;

namespace AADizErp.ViewModels.RequestVM
{
    public partial class RemoteAttendancePageViewModel : BaseViewModel
    {
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalCount = 0;

        [ObservableProperty]
        private ObservableRangeCollection<RemoteAttendanceDto> attendances = new();
        [ObservableProperty]
        private RemoteAttendance remoteAttendance = new();
        [ObservableProperty]
        private IndividualAttendanceSummaryDto individualAttendanceSummary = new();

        private readonly AttendanceService _attnService;
        private readonly NotificationService _notify;
        private readonly IBadge _badge;

        public RemoteAttendancePageViewModel(AttendanceService attnService, NotificationService notify, IBadge badge)
        {
            _attnService=attnService;
            _notify=notify;
            _badge = badge;
            GetRemoteAttendanceList(pageNumber, pageSize);
        }

        private void GetRemoteAttendanceList(int index, int size)
        {
            if (totalCount != 0) totalCount = 0;
            IsLoading = true;
            Attendances.Clear();
            Task.Run(async () =>
            {
                var userInfo = await App.GetUserInfo();

                IndividualAttendanceSummary = await _attnService.GetIndividualAttendanceSummary(userInfo);
                var returnAttendances = await _attnService.GetAttendanceDetailsByEmpid(userInfo.TokenUserMetaInfo.EmployeeNumber, index, size);

                if (returnAttendances.Count >0)
                {
                    App.Current.Dispatcher.Dispatch(() =>
                    {
                        totalCount = returnAttendances.Count;
                        Attendances.ReplaceRange(returnAttendances.Data);
                        IsLoading = false;
                    });
                }
                else
                {
                    IsLoading = false;
                }

            });  
        }

        [RelayCommand]
        async Task PullToRefreshAttendances()
        {
            IsLoading = true;
            pageNumber = 1;
            pageSize = 10;
            if (totalCount != 0) totalCount = 0;
            Attendances.Clear();
            var userInfo = await App.GetUserInfo();
            var returnAttendances = await _attnService.GetAttendanceDetailsByEmpid(userInfo.TokenUserMetaInfo.EmployeeNumber, pageNumber, pageSize);
            if (returnAttendances.Count > 0)
            {
                App.Current.Dispatcher.Dispatch(() =>
                {
                    totalCount = returnAttendances.Count;
                    Attendances.ReplaceRange(returnAttendances.Data);
                });
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        async Task LoadMoreRemoteAttendanceData()
        {
            var userInfo = await App.GetUserInfo();
            if (totalCount == Attendances.Count())
            {
                IsLoading = false;
                return;
            }
            IsLoading = true;
            pageNumber++;
            var returnAttendances = await _attnService.GetAttendanceDetailsByEmpid(userInfo.TokenUserMetaInfo.EmployeeNumber, pageNumber, pageSize);
            if (returnAttendances.Count > 0)
            {
                Attendances.AddRange(returnAttendances.Data);
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        //view, edit, new
        void CreateDetailFormViewModel(CreateDetailFormViewModelEventArgs e)
        {
            if (e.DetailFormType == DetailFormType.Edit)
            {
                RemoteAttendance attendance = (RemoteAttendance)e.Item;
                e.Result = new DetailEditFormViewModel(attendance, isNew: false, context: null);
            }
        }

        [RelayCommand]
        async Task ValidateAndSave(ValidateItemEventArgs e)
        {
            RemoteAttendanceDto obj = (RemoteAttendanceDto)e.Item;
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Oops!", "You are disconnected", "OK");
                return;
            }
            else
            {
                if (String.IsNullOrWhiteSpace(obj.Reason)){
                    await Shell.Current.DisplayAlert("Reason Missing!", "Please enter a valid reason", "OK");
                    return;
                }
                else
                {
                    try
                    {
                        string token = await App.GetAuthToken();
                        var location = await _attnService.GetCurrentLocation();
                        IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
                        Placemark placemark = placemarks?.FirstOrDefault();

                        var tokenInfo = await App.GetUserInfo();

                        RemoteAttendance.JobId = tokenInfo.TokenUserMetaInfo.EmployeeNumber;
                        RemoteAttendance.FullName = tokenInfo.TokenUserMetaInfo.Name;
                        RemoteAttendance.Latitude = location.Latitude;
                        RemoteAttendance.Longitude = location.Longitude;
                        RemoteAttendance.RequestedBy = tokenInfo.TokenUserMetaInfo.UserName;
                        RemoteAttendance.ApprovedBy = tokenInfo.TokenUserMetaInfo.ManagerUserName;
                        RemoteAttendance.Reason = obj.Reason;
                        RemoteAttendance.RequestedTime = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");

                        RemoteAttendance.AttendanceArea = $"{placemark.AdminArea}, {placemark.CountryCode}, {placemark.CountryName}, {placemark.FeatureName}, {placemark.Locality}, {placemark.PostalCode}, {placemark.SubAdminArea}, {placemark.SubLocality}, {placemark.SubThoroughfare}, {placemark.Thoroughfare}";

                        var returnAttn = await _attnService.SubmitAttendanceRequest(RemoteAttendance);
                        if(returnAttn != null)
                        {
                          if(!await _notify.SendAttendancePushNotificationToManager(returnAttn))
                            {
                                await Shell.Current.DisplayAlert("Notification!", "We've sent a notfication", "OK");
                                return;
                            }
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Duplicate Found!", "You can't enter twice a day", "OK");
                            return;
                        }
                        Attendances.Add(returnAttn);
                        RemoteAttendance =new();

                    }
                    catch
                    {
                        await Shell.Current.DisplayAlert("Submitted", "Your request submitted", "OK");
                    }
                }
            }
        }

        

    }
}
