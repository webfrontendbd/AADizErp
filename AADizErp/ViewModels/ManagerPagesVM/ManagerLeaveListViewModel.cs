using AADizErp.Models.Dtos.LeaveDtos;
using AADizErp.Services;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.ManagerPagesVM
{
    public partial class ManagerLeaveListViewModel : BaseViewModel
    {
        private readonly LeaveService _leaveService;
        private readonly NotificationService _notify;

        private int pageNumber = 1;
        private readonly int pageSize = 10;
        private int totalCount = 0;
        private bool isLoadingMore = false;

        [ObservableProperty]
        private ObservableRangeCollection<LeaveRequestDto> leaveRequests = new();

        [ObservableProperty]
        private LeaveRequestDto leaveRequest = new();

        [ObservableProperty]
        private bool isPopupOpen = false;


        public ManagerLeaveListViewModel(LeaveService leaveService, NotificationService notify)
        {
            _leaveService = leaveService;
            _notify = notify;

            _ = LoadInitialAsync(); // Fire and forget safely
        }

        private async Task LoadInitialAsync()
        {
            pageNumber = 1;
            LeaveRequests.Clear();
            await GetLeaveRequestsAsync(isFirstLoad: true);
        }


        private async Task GetLeaveRequestsAsync(bool isFirstLoad = false)
        {
            if (IsLoading) return;
            IsLoading = true;

            var user = await App.GetUserInfo();
            var response = await _leaveService.GetListLeaveRequestForManager(pageNumber, pageSize, user.TokenUserMetaInfo.UserName);

            if (response != null && response.Data != null)
            {
                if (isFirstLoad)
                {
                    totalCount = response.Count;
                    LeaveRequests.ReplaceRange(response.Data);
                }
                else
                {
                    LeaveRequests.AddRange(response.Data);
                }
            }

            IsLoading = false;
        }


        // ================================
        // LOAD MORE PAGINATION
        // ================================
        [RelayCommand]
        async Task LoadMoreLeaveRequestsForManager()
        {
            if (isLoadingMore || IsLoading) return;
            if (LeaveRequests.Count >= totalCount) return;

            isLoadingMore = true;

            pageNumber++;
            await GetLeaveRequestsAsync(isFirstLoad: false);

            isLoadingMore = false;
        }


        // ================================
        // OPEN APPROVAL POPUP
        // ================================
        [RelayCommand]
        async Task LeaveApprovalPopupAction(LeaveRequestDto leaveDto)
        {
            if (leaveDto == null) return;

            // Mark seen if new
            if (!leaveDto.RequestSeen)
            {
                leaveDto.RequestSeen = true;
                await _leaveService.LeaveApprovalStatusChangedByManager(leaveDto);
            }

            LeaveRequest = leaveDto;
            IsPopupOpen = true;
        }


        // ================================
        // APPROVE / REJECT LEAVE
        // ================================
        [RelayCommand]
        async Task LeaveApprovalStatusSubmit(string status)
        {
            if (LeaveRequest == null) return;

            LeaveRequest.Status = status;

            var updated = await _leaveService.LeaveApprovalStatusChangedByManager(LeaveRequest);

            if (updated == null)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong", "OK");
                return;
            }

            // Push notification
            try
            {
                await _notify.SendLeavePushNotificationBackToUser(LeaveRequest);
                App.BadgeManager.Decrement();
            }
            catch
            {
                await Shell.Current.DisplayAlert("Notification!", "We've sent a notification", "OK");
            }

            // Update item in list
            var index = LeaveRequests.IndexOf(LeaveRequests.FirstOrDefault(l => l.Id == LeaveRequest.Id));

            if (index >= 0)
            {
                LeaveRequests[index] = LeaveRequest; // triggers UI update
            }

            IsPopupOpen = false;
        }
    }
}
