using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Utils.Design;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADizErp.ViewModels.ManagerPagesVM
{
    public partial class ApprovalLandingPageViewModel : BaseViewModel
    {
        private readonly AttendanceService _attnService;
        private readonly LeaveService _leaveService;
        [ObservableProperty]
        private bool isAttendanceVisible;
        [ObservableProperty]
        private bool isLeaveVisible;
        public ApprovalLandingPageViewModel(AttendanceService attnService, LeaveService leaveService)
        {
            _attnService = attnService;
            _leaveService = leaveService;
            GetAttendanceLeaveStatusUpdate();
        }
        private async void GetAttendanceLeaveStatusUpdate()
        {
            var userInfo = await App.GetUserInfo();
            int count = await _attnService.GetAttendanceRequestSeenDataAsync();
            int leaveCount = await _leaveService.GetLeaveRequestSeenDataAsync();
            if (count > 0)
            {
                IsAttendanceVisible = true;
            }
            else
            {
                IsAttendanceVisible = false;
            }

            if (leaveCount > 0)
            {
                IsLeaveVisible = true;
            }
            else
            {
                IsLeaveVisible = false;
            }
            //Task.Run(async () =>
            //{
            //    var userInfo = await App.GetUserInfo();
            //    int count = await _attnService.GetAttendanceRequestSeenDataAsync();
            //    int leaveCount = await _leaveService.GetLeaveRequestSeenDataAsync();
            //    if (count > 0)
            //    {
            //        IsAttendanceVisible = true;
            //    }
            //    else
            //    {
            //        IsAttendanceVisible = true;
            //    }

            //    if (leaveCount > 0)
            //    {
            //        IsLeaveVisible = true;
            //    }
            //    else
            //    {
            //        IsLeaveVisible = false;
            //    }
            //});
        }
    }
}
