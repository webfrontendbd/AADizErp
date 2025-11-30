using AADizErp.Models.Dtos.LeaveDtos;
using AADizErp.Services;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.RequestVM
{
    public partial class LeaveRequestPageViewModel : BaseViewModel
    {
        private int pageNumber = 1;
        private const int pageSize = 8;

        [ObservableProperty]
        private ObservableRangeCollection<LeaveRequestDto> leaveRequests = new();

        [ObservableProperty]
        private DateTime minimumDate;

        [ObservableProperty]
        private int totalLeaveDays;

        [ObservableProperty]
        private IndividualLeaveSummary leaveSummary;

        [ObservableProperty]
        private string reason;

        [ObservableProperty]
        private string leaveType;

        [ObservableProperty]
        private DateTime leaveStartDate = DateTime.Now;

        [ObservableProperty]
        private DateTime leaveEndDate = DateTime.Now;

        private readonly LeaveService _leaveService;
        private readonly NotificationService _notify;

        public LeaveRequestPageViewModel(LeaveService leaveService, NotificationService notify)
        {
            _leaveService = leaveService;
            _notify = notify;

            MinimumDate = LeaveStartDate;
            TotalLeaveDays = 1;

            _ = InitializePageAsync();
        }

        private async Task InitializePageAsync()
        {
            await LoadLeaveSummaryAsync();
            await LoadLeaveRequestsAsync(true);
        }

        private async Task LoadLeaveSummaryAsync()
        {
            var user = await App.GetUserInfo();
            LeaveSummary = await _leaveService.GetIndividualLeaveSummary(user);
        }

        private async Task LoadLeaveRequestsAsync(bool reset = false)
        {
            if (reset)
            {
                pageNumber = 1;
                LeaveRequests.Clear();
            }

            IsLoading = true;

            var user = await App.GetUserInfo();
            var result = await _leaveService.GetListOfIndividualLeaveRequest(
                pageNumber, pageSize, user.TokenUserMetaInfo.UserName);

            if (result?.Data != null)
            {
                LeaveRequests.AddRange(result.Data);
            }

            IsLoading = false;
        }

        // =============================
        // Load More (Paging)
        // =============================

        [RelayCommand]
        private async Task LoadMoreIndividualLeaveRequestsAsync()
        {
            if (IsLoading) return;

            pageNumber++;
            await LoadLeaveRequestsAsync();
        }

        // =============================
        // Date Selection Logic
        // =============================

        [RelayCommand]
        private void StartDateSelected()
        {
            LeaveStartDate = LeaveStartDate.Date;
            MinimumDate = LeaveStartDate;

            if (LeaveEndDate < LeaveStartDate)
            {
                LeaveEndDate = LeaveStartDate;
            }

            TotalLeaveDays = (int)(LeaveEndDate - LeaveStartDate).TotalDays + 1;
        }

        [RelayCommand]
        private async Task EndDateSelectedAsync()
        {
            LeaveEndDate = LeaveEndDate.Date;
            LeaveStartDate = LeaveStartDate.Date;

            if (LeaveEndDate < LeaveStartDate)
            {
                await Shell.Current.DisplayAlert("Oops!", "Your date selection is wrong!", "OK");
                LeaveEndDate = LeaveStartDate;
                TotalLeaveDays = 1;
                return;
            }

            TotalLeaveDays = (int)(LeaveEndDate - LeaveStartDate).TotalDays + 1;
        }

        // =============================
        // Submit Leave Request
        // =============================

        [RelayCommand]
        private async Task SubmitLeaveRequestAsync()
        {
            if (string.IsNullOrWhiteSpace(Reason))
            {
                await Shell.Current.DisplayAlert("Oops!", "Please enter a valid reason!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(LeaveType))
            {
                await Shell.Current.DisplayAlert("Oops!", "Please select a leave type!", "OK");
                return;
            }

            var user = await App.GetUserInfo();

            var leaveRequest = new LeaveRequestDto
            {
                JobId = user.TokenUserMetaInfo.EmployeeNumber,
                FullName = user.TokenUserMetaInfo.Name,
                ApprovedBy = user.TokenUserMetaInfo.ManagerUserName,
                RequestedBy = user.TokenUserMetaInfo.UserName,
                Reason = Reason,
                LeaveType = LeaveType,
                LeaveStartDate = LeaveStartDate,
                LeaveEndDate = LeaveEndDate,
                Status = "Pending"
            };

            var result = await _leaveService.SubmitLeaveRequest(leaveRequest);

            if (result == null)
            {
                await Shell.Current.DisplayAlert("Oops!", "Please try again!", "OK");
                return;
            }

            var notificationSent =
                await _notify.SendLeaveRequestPushNotificationToManager(leaveRequest);

            if (notificationSent)
            {
                App.BadgeManager.Increment();
                await LoadLeaveRequestsAsync(true);
                Reason = string.Empty;

                await Shell.Current.DisplayAlert("Success!", "Your request has been submitted!", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Notification", "Notification sent.", "OK");
            }
        }
    }
}
