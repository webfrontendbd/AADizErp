using AADizErp.Pages.HolidayPages;
using AADizErp.Pages.HRPages;
using AADizErp.Pages.ManagerPages;
using AADizErp.Pages.MisPages;
using AADizErp.Pages.NptPages;
using AADizErp.Pages.RequestPages;
using AADizErp.Pages.SalaryPages;
using AADizErp.Pages.SettingsPages;

namespace AADizErp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(ThankYouPage), typeof(ThankYouPage));

            Routing.RegisterRoute(nameof(RequestLandingPage), typeof(RequestLandingPage));
            Routing.RegisterRoute(nameof(AttendanceRequestPage), typeof(AttendanceRequestPage));
            Routing.RegisterRoute(nameof(LeaveRequestPage), typeof(LeaveRequestPage));
            Routing.RegisterRoute(nameof(LeaveRequestFormPopup), typeof(LeaveRequestFormPopup));

            Routing.RegisterRoute(nameof(ApprovalLandingPage), typeof(ApprovalLandingPage));
            Routing.RegisterRoute(nameof(ManagerViewAttnRequestPage), typeof(ManagerViewAttnRequestPage));
            Routing.RegisterRoute(nameof(MgrAttendanceDetailsPage), typeof(MgrAttendanceDetailsPage));
            Routing.RegisterRoute(nameof(ManagerLeaveViewPage), typeof(ManagerLeaveViewPage));

            Routing.RegisterRoute(nameof(PaySlipListViewPage), typeof(PaySlipListViewPage));

            Routing.RegisterRoute(nameof(HrLandingPage), typeof(HrLandingPage));
            Routing.RegisterRoute(nameof(HrDashboardPage), typeof(HrDashboardPage));

            Routing.RegisterRoute(nameof(HolidayViewPage), typeof(HolidayViewPage));
            Routing.RegisterRoute(nameof(AsianEventPage), typeof(AsianEventPage));

            Routing.RegisterRoute(nameof(IndividualTimeCardViewPage), typeof(IndividualTimeCardViewPage));
            Routing.RegisterRoute(nameof(ProfileViewPage), typeof(ProfileViewPage));
            Routing.RegisterRoute(nameof(EmployeeFileQueryPage), typeof(EmployeeFileQueryPage));
            Routing.RegisterRoute(nameof(EmployeeTimeCardPage), typeof(EmployeeTimeCardPage));
            Routing.RegisterRoute(nameof(DailyAbsentDetailPage), typeof(DailyAbsentDetailPage));
            Routing.RegisterRoute(nameof(UnreviewedUserListPage), typeof(UnreviewedUserListPage));

            Routing.RegisterRoute(nameof(AddUnconfirmEmployeePage), typeof(AddUnconfirmEmployeePage));

            Routing.RegisterRoute(nameof(SettingsLandingPage), typeof(SettingsLandingPage));
            Routing.RegisterRoute(nameof(PasswordChangePage), typeof(PasswordChangePage));
            Routing.RegisterRoute(nameof(DeleteAccountPage), typeof(DeleteAccountPage));
            Routing.RegisterRoute(nameof(ConveyanceViewPage), typeof(ConveyanceViewPage));

            Routing.RegisterRoute(nameof(NptLandingPage), typeof(NptLandingPage));
            Routing.RegisterRoute(nameof(McBarcodeScanPage), typeof(McBarcodeScanPage));
            Routing.RegisterRoute(nameof(MisLandingPage), typeof(MisLandingPage));
            Routing.RegisterRoute(nameof(OverTimePage), typeof(OverTimePage));
            Routing.RegisterRoute(nameof(eAttendancePage), typeof(eAttendancePage));
            Routing.RegisterRoute(nameof(ProductionPage), typeof(ProductionPage));
            Routing.RegisterRoute(nameof(MachinesPage), typeof(MachinesPage));
            Routing.RegisterRoute(nameof(McStatusUpdatePage), typeof(McStatusUpdatePage));
            Routing.RegisterRoute(nameof(MachineInfoPage), typeof(MachineInfoPage));
            Routing.RegisterRoute(nameof(SaveAttendancePage), typeof(SaveAttendancePage));
            InitializeComponent();
        }
    }
}
