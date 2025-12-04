using AADizErp.Models;
using AADizErp.Models.Dtos;
using AADizErp.Pages.RequestPages;
using AADizErp.Services;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace AADizErp.ViewModels.RequestVM
{
    public partial class RemoteAttendancePageViewModel : BaseViewModel
    {
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalCount = 0;

        [ObservableProperty]
        bool isBusy;

        public bool IsNotBusy => !IsBusy;

        partial void OnIsBusyChanged(bool value)
        {
            OnPropertyChanged(nameof(IsNotBusy));
        }
        [ObservableProperty]
        private ObservableRangeCollection<RemoteAttendanceDto> attendances = new();
        [ObservableProperty]
        private RemoteAttendance remoteAttendance = new();
        [ObservableProperty]
        private IndividualAttendanceSummaryDto individualAttendanceSummary = new();

        [ObservableProperty]
        private ObservableCollection<string> statusList = new()
        {
            "In Time", "Out Time"
        };
        [ObservableProperty]
        private string selectedStatus;

        private readonly AttendanceService _attnService;
        private readonly NotificationService _notify;

        public RemoteAttendancePageViewModel(AttendanceService attnService, NotificationService notify)
        {
            _attnService = attnService;
            _notify = notify;
            GetRemoteAttendanceList(pageNumber, pageSize);
        }

        private async void GetRemoteAttendanceList(int index, int size)
        {
            if (totalCount != 0) totalCount = 0;

            IsLoading = true;
            Attendances.Clear();

            var userInfo = await App.GetUserInfo();

            IndividualAttendanceSummary =
                await _attnService.GetIndividualAttendanceSummary(userInfo);

            var returnAttendances =
                await _attnService.GetAttendanceDetailsByEmpid(
                    userInfo.TokenUserMetaInfo.EmployeeNumber, index, size);

            if (returnAttendances?.Count > 0)
            {
                totalCount = returnAttendances.Count;
                Attendances.ReplaceRange(returnAttendances.Data);
            }

            IsLoading = false;
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
        public async Task SubmitRemoteAttendanceRequest()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Network check
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    await ShowAlert("Oops!", "You are disconnected from the internet.");
                    return;
                }


                if (string.IsNullOrWhiteSpace(RemoteAttendance.Reason))
                {
                    await ShowAlert("Reason Missing!", "Please enter a valid reason.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedStatus))
                {
                    await ShowAlert("Type Missing!", "Please select intime or outtime.");
                    return;
                }

                //Check duplicate before save data to the server
                string dateToday = DateTime.Now.ToString("dd-MMM-yyyy");
                var duplicateCheck= await _attnService.CheckedIndvidualAttTimeByDateType(dateToday, RemoteAttendance.RequestedBy, SelectedStatus);

                if (duplicateCheck != null)
                {
                    var tokenInfo = await App.GetUserInfo();

                    // Get fresh location
                    Location location = await GetFreshLocationAsync();

                    // Retry once if needed
                    if (location == null)
                    {
                        await Task.Delay(1500);
                        location = await GetFreshLocationAsync();
                    }

                    if (location == null)
                    {
                        await ShowAlert("Location Error", "Unable to detect your current location. Move to an open area and try again.");
                        return;
                    }

                    // Build address
                    string address = await BuildAddressAsync(location);

                    // Prepare attendance object
                    RemoteAttendance = new()
                    {
                        JobId = tokenInfo?.TokenUserMetaInfo?.EmployeeNumber,
                        FullName = tokenInfo?.TokenUserMetaInfo?.Name,
                        RequestedBy = tokenInfo?.TokenUserMetaInfo?.UserName,
                        ApprovedBy = tokenInfo?.TokenUserMetaInfo?.ManagerUserName,
                        AttType = SelectedStatus,
                        Reason = RemoteAttendance.Reason,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        RequestedTime = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt"),
                        AttendanceArea = address
                    };

                    // Submit request
                    var returnAttn = await _attnService.SubmitAttendanceRequest(RemoteAttendance);
                    // Send push notification
                    bool sent = await _notify.SendAttendancePushNotificationToManager(returnAttn);

                    if (!sent)
                    {
                        await ShowAlert("Submitted", "Attendance submitted, but notification to manager failed.");
                    }
                    else
                    {
                        App.BadgeManager.Increment();
                        Attendances.Add(returnAttn);
                        await Shell.Current.GoToAsync($"{nameof(AttendanceRequestPage)}");
                    }
                }
                else
                {
                    await ShowAlert("Duplicate Found!", "You cannot enter attendance twice in one day.");
                    return;
                }
                                
            }
            catch (Exception ex)
            {
                await ShowAlert("Error", $"An unexpected error occurred.\n\n{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<Location> GetFreshLocationAsync()
        {
            try
            {
                // Force fresh GPS location (best accuracy)
                var request = new GeolocationRequest(
                    GeolocationAccuracy.Best,
                    TimeSpan.FromSeconds(10));

                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                    return location;

                // Fallback
                return await Geolocation.GetLastKnownLocationAsync();
            }
            catch
            {
                // If everything fails
                return await Geolocation.GetLastKnownLocationAsync();
            }
        }

        private async Task<string> BuildAddressAsync(Location location)
        {
            var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
            var p = placemarks?.FirstOrDefault();

            var parts = new[]
            {
                p?.Thoroughfare,
                p?.SubThoroughfare,
                p?.Locality,
                p?.SubLocality,
                p?.AdminArea,
                p?.SubAdminArea,
                p?.CountryName,
                p?.PostalCode
            };

            return string.Join(", ", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        private async Task ShowAlert(string title, string message) =>
            await Shell.Current.DisplayAlert(title, message, "OK");





    }
}
