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
        [ObservableProperty]
        private bool isAttendanceVisible;
        [ObservableProperty]
        private bool isLeaveVisible;
        public ApprovalLandingPageViewModel(AttendanceService attnService)
        {
            _attnService = attnService;
            GetAttendanceLeaveStatusUpdate();
        }
        private void GetAttendanceLeaveStatusUpdate()
        {
            Task.Run(async () =>
            {
                var userInfo = await App.GetUserInfo();
                IsAttendanceVisible = true;
                IsLeaveVisible = true;
            });
        }
    }
}
