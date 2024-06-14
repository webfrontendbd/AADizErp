
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
        private int pageSize = 10;
        private int totalCount = 0;

        [ObservableProperty]
        private ObservableRangeCollection<LeaveRequestDto> leaveRequests = new();
        [ObservableProperty]
        private LeaveRequestDto leaveRequest = new();
        [ObservableProperty]
        bool isPopupOpen = false;
        public ManagerLeaveListViewModel(LeaveService leaveService, NotificationService notify)
        {
            _leaveService=leaveService;
            _notify=notify;
            GetListOfLeaveRequestForManager(pageNumber, pageSize);
        }

        private void GetListOfLeaveRequestForManager(int pageIndex, int showRecord)
        {
            IsLoading = true;
            if (totalCount != 0) totalCount = 0;
            LeaveRequests.Clear();
            Task.Run(async () =>
            {
                var user = await App.GetUserInfo();
                var returnLeaveRequest = await _leaveService.GetListLeaveRequestForManager(pageIndex, showRecord, user.TokenUserMetaInfo.UserName);
                if (returnLeaveRequest.Count > 0)
                {
                    App.Current.Dispatcher.Dispatch(() =>
                    {
                        totalCount = returnLeaveRequest.Count;
                        LeaveRequests.ReplaceRange(returnLeaveRequest.Data);
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
        async Task LoadMoreLeaveRequestsForManager()
        {
            if (totalCount == LeaveRequests.Count())
            {
                return;
            }
            IsLoading = true;
            pageNumber++;
            var user = await App.GetUserInfo();
            var returnLeaveRequest = await _leaveService.GetListLeaveRequestForManager(pageNumber, pageSize, user.TokenUserMetaInfo.UserName);
            if (returnLeaveRequest.Count > 0)
            {
                LeaveRequests.AddRange(returnLeaveRequest.Data);
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }
        }
        [RelayCommand]
        void LeaveApprovalPopupAction(LeaveRequestDto leaveDto)
        {
            if (leaveDto == null) return;
            LeaveRequest = leaveDto;
            IsPopupOpen = true;            
        }

        [RelayCommand]
        async Task LeaveApprovalStatusSubmit(string status)
        {
            LeaveRequest.Status = status;

            var returnObject = await _leaveService.LeaveApprovalStatusChangedByManager(LeaveRequest);

            if(returnObject != null)
            {
                try
                {

                await _notify.SendLeavePushNotificationBackToUser(LeaveRequest);
                }
                catch
                {
                    await Shell.Current.DisplayAlert("Notification!", "We've sent a notification", "OK");
                }


                var changedLeaveRequest = LeaveRequests.FirstOrDefault(l => l.Id == LeaveRequest.Id);
                
                if (changedLeaveRequest is not null)
                {
                    var index = LeaveRequests.IndexOf(changedLeaveRequest);
                    LeaveRequests[index] = changedLeaveRequest;
                }
                  
                IsPopupOpen = false;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong", "OK");
            }
            
        }

    }
}
