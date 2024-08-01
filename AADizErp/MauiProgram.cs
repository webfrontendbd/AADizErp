using AADizErp.ViewModels;
using DevExpress.Maui;
using DevExpress.Maui.Core;
using AADizErp.Services;
using CommunityToolkit.Maui;
using AADizErp.Services.RequestServices;
using AADizErp.Pages.RequestPages;
using AADizErp.Services.HrServices;
using AADizErp.ViewModels.RequestVM;
using AADizErp.Pages.ManagerPages;
using AADizErp.ViewModels.ManagerPagesVM;
using AADizErp.ViewModels.SalaryPagesVM;
using AADizErp.Pages.SalaryPages;
using AADizErp.ViewModels.HolidayVm;
using AADizErp.Pages.HolidayPages;
using AADizErp.Pages.HRPages;
using AADizErp.ViewModels.HrVM;
using Plugin.Maui.Biometric;
using AADizErp.Pages.SettingsPages;
using AADizErp.ViewModels.SettingsVM;

namespace AADizErp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            ThemeManager.ApplyThemeToSystemBars = true;
            ThemeManager.UseAndroidSystemColor = false;
            ThemeManager.Theme = new Theme(Color.FromArgb("FF6200EE"));
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseDevExpress()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("univia-pro-regular.ttf", "Univia-Pro");
                    fonts.AddFont("roboto-bold.ttf", "Roboto-Bold");
                    fonts.AddFont("roboto-regular.ttf", "Roboto");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");
                });

            //builder.Services.AddSingleton<IFingerprint>(provider => CrossFingerprint.Current);
            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);
            builder.Services.AddSingleton<IGeolocation>(provider => Geolocation.Default);
            builder.Services.AddSingleton<IMap>(provider => Map.Default);

            //Custom Services
            builder.Services.AddSingleton<NotificationService>();
            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddSingleton<AttendanceService>();
            builder.Services.AddSingleton<DeviceTokenService>();
            builder.Services.AddSingleton<LeaveService>();
            builder.Services.AddSingleton<HrService>();
            builder.Services.AddSingleton<OccasionService>();
            builder.Services.AddSingleton<LocalDbService>();

            //Pages and viewmodels
            #region Pages and Viewmodels
            builder.Services.AddTransient<RequestLandingPage>();

            builder.Services.AddTransient<RemoteAttendancePageViewModel>();
            builder.Services.AddTransient<AttendanceRequestPage>();

            builder.Services.AddTransient<LeaveRequestPageViewModel>();
            builder.Services.AddTransient<LeaveRequestPage>();

            builder.Services.AddTransient<LeaveRequestFormPopup>();

            builder.Services.AddTransient<ApprovalLandingPage>();

            builder.Services.AddTransient<ManagerAttendanceListViewModel>();
            builder.Services.AddTransient<ManagerViewAttnRequestPage>();

            builder.Services.AddTransient<MgrAttendanceDetailsPageViewModel>();
            builder.Services.AddTransient<MgrAttendanceDetailsPage>();

            builder.Services.AddTransient<ManagerLeaveListViewModel>();
            builder.Services.AddTransient<ManagerLeaveViewPage>();

            builder.Services.AddTransient<PaySlipListViewPage>();
            builder.Services.AddTransient<PaySlipListViewPageViewModel>();

            builder.Services.AddTransient<HolidayViewPage>();
            builder.Services.AddTransient<HolidayViewPageViewModel>();

            builder.Services.AddTransient<AsianEventPage>();
            builder.Services.AddTransient<AsianEventViewModel>();

            builder.Services.AddTransient<HrLandingPage>();

            builder.Services.AddTransient<HrDashboardPage>();
            builder.Services.AddTransient<HrDashboardPageViewModel>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();

            builder.Services.AddTransient<RegistrationPageViewModel>();
            builder.Services.AddTransient<RegistrationPage>();

            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoadingViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();

            builder.Services.AddTransient<IndividualTimeCardViewPage>();
            builder.Services.AddTransient<IndividualTimeCardViewModel>();

            builder.Services.AddTransient<ProfileViewPage>();
            builder.Services.AddTransient<ProfilePageViewModel>();

            builder.Services.AddTransient<EmployeeTimeCardPage>();
            builder.Services.AddTransient<EmployeeTimeCardPageViewModel>();


            builder.Services.AddTransient<EmployeeFileQueryPage>();
            builder.Services.AddTransient<EmployeeFileQueryPageViewModel>();

            builder.Services.AddTransient<DailyAbsentDetailPage>();
            builder.Services.AddTransient<DailyAbsentDetailPageViewModel>();

            builder.Services.AddTransient<UnreviewedUserListPage>();
            builder.Services.AddTransient<UnreviewedUserPageViewModel>();

            builder.Services.AddTransient<PasswordChangePage>();
            builder.Services.AddTransient<PasswordChangePageViewModel>();

            builder.Services.AddTransient<DeleteAccountPage>();
            builder.Services.AddTransient<DeleteAccountPageViewModel>();

            #endregion

            return builder.Build();
        }
    }
}
