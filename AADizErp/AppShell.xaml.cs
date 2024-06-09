using AADizErp.Pages.HolidayPages;
using AADizErp.Pages.HRPages;
using AADizErp.Pages.ManagerPages;
using AADizErp.Pages.RequestPages;
using AADizErp.Pages.SalaryPages;

namespace AADizErp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            //Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            //Routing.RegisterRoute(nameof(ThankYouPage), typeof(ThankYouPage));

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
            InitializeComponent();
        }
    }
}
