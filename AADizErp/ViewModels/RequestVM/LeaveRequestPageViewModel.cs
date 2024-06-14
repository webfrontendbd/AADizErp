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
        private int pageSize = 8;
        private int totalCount = 0;

        [ObservableProperty]
        ObservableRangeCollection<LeaveRequestDto> leaveRequests = new();
        [ObservableProperty]
        DateTime minimumDate;
        [ObservableProperty]
        int totalLeaveDays;
        [ObservableProperty]
        LeaveRequestDto leaveRequestDto = new();
        [ObservableProperty]
        IndividualLeaveSummary leaveSummary = new();

        [ObservableProperty]
        string leaveType;
        [ObservableProperty]
        string reason;
        [ObservableProperty]
        DateTime leaveStartDate = DateTime.Now;
        [ObservableProperty]
        DateTime leaveEndDate = DateTime.Now;


        private readonly LeaveService _leaveService;
        private readonly NotificationService _notify;

        public LeaveRequestPageViewModel(LeaveService leaveService, NotificationService notify)
        {
            SetInitialLeaveDay();
            _leaveService = leaveService;
            _notify=notify;
            GetIndividualLeaveSummary();
            GetIndividualLeaveRequests(pageNumber, pageSize);
        }
        private void GetIndividualLeaveSummary()
        {
            Task.Run(async () =>
            {
                var user = await App.GetUserInfo();
                LeaveSummary = await _leaveService.GetIndividualLeaveSummary(user);
            });
        }
        private void GetIndividualLeaveRequests(int pageIndex, int showRecord)
        {
            if (totalCount != 0) totalCount = 0;
            IsLoading = true;
            LeaveRequests.Clear();
            Task.Run(async () =>
            {
                var user = await App.GetUserInfo();
                var returnLeaveRequest = await _leaveService.GetListOfIndividualLeaveRequest(pageIndex, showRecord, user.TokenUserMetaInfo.UserName);
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

        private void SetInitialLeaveDay()
        {
            LeaveRequestDto.LeaveStartDate = DateTime.Now;
            MinimumDate = LeaveRequestDto.LeaveStartDate;
            LeaveRequestDto.LeaveEndDate = LeaveRequestDto.LeaveStartDate;
            TotalLeaveDays = (int)(LeaveRequestDto.LeaveEndDate - LeaveRequestDto.LeaveStartDate).TotalDays + 1;
        }

        [RelayCommand]
        async Task LoadMoreIndividualLeaveRequests()
        {
            if (totalCount == LeaveRequests.Count())
            {
                IsLoading = false;
                return;
            }
            IsLoading = true;
            pageNumber++;
            var user = await App.GetUserInfo();
            var returnLeaveRequest = await _leaveService.GetListOfIndividualLeaveRequest(pageNumber, pageSize, user.TokenUserMetaInfo.UserName);
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
        async Task StartDateSelected()
        {
            SetLeaveMinimumDate(LeaveStartDate);

            if (LeaveStartDate > LeaveEndDate)
            {
                await Shell.Current.DisplayAlert("Oops!", "Your date selection is wrong! Please try again", "OK");
                LeaveEndDate = DateTime.Now;
                return;
            }
            TotalLeaveDays = (int)(LeaveEndDate - LeaveStartDate).TotalDays+1;
        }

        [RelayCommand]
        async Task EndDateSelected()
        {
            if (LeaveStartDate > LeaveEndDate)
            {
                await Shell.Current.DisplayAlert("Oops!", "Your date selection is wrong! Please try again", "OK");
                LeaveEndDate = DateTime.Today;
                return;
            }
            TotalLeaveDays = (int)(LeaveEndDate - LeaveStartDate).TotalDays+1;
        }

        [RelayCommand]
        async Task SubmitLeaveRequest()
        {
            if (!String.IsNullOrWhiteSpace(Reason))
            {
                var user = await App.GetUserInfo();
                var leaveRequestDto = new LeaveRequestDto
                {
                    JobId = user.TokenUserMetaInfo.EmployeeNumber,
                    FullName = user.TokenUserMetaInfo.Name,
                    ApprovedBy = user.TokenUserMetaInfo.ManagerUserName,
                    RequestedBy = user.TokenUserMetaInfo.UserName,
                    Reason = Reason,
                    LeaveStartDate = LeaveStartDate,
                    LeaveEndDate = LeaveEndDate,
                    Status = "Pending"
                };


                var returnLeaveRequest = await _leaveService.SubmitLeaveRequest(leaveRequestDto);

                if (returnLeaveRequest == null)
                {
                    await Shell.Current.DisplayAlert("Oops!", "Please try again!", "OK");
                }
                else
                {
                    if (await _notify.SendLeaveRequestPushNotificationToManager(leaveRequestDto))
                    {
                        GetIndividualLeaveRequests(1, 8);
                        Reason=String.Empty;
                        await Shell.Current.DisplayAlert("Success!", "Your Request Submitted!", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Notification!", "We've sent a notification", "OK");
                        return;
                    }
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Oops!", "Please enter a valid reason!", "OK");
            }

        }

        private void SetLeaveMinimumDate(DateTime startDatetime)
        {
            MinimumDate = startDatetime;
        }
    }
}
