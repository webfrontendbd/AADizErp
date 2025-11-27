using AADizErp;
using AADizErp.Pages.HolidayPages;
using AADizErp.Pages.HRPages;
using AADizErp.Pages.ManagerPages;
using AADizErp.Pages.MisPages;
using AADizErp.Pages.NptPages;
using AADizErp.Pages.RequestPages;
using AADizErp.Pages.SalaryPages;
using AADizErp.Pages.SettingsPages;
using AADizErp.Services;
using AADizErp.Services.HrServices;
using AADizErp.Services.McServices;
using AADizErp.Services.MisServices;
using AADizErp.Services.RequestServices;
using AADizErp.ViewModels;
using AADizErp.ViewModels.HolidayVm;
using AADizErp.ViewModels.HrVM;
using AADizErp.ViewModels.ManagerPagesVM;
using AADizErp.ViewModels.McPageVM;
using AADizErp.ViewModels.MisPageVM;
using AADizErp.ViewModels.RequestVM;
using AADizErp.ViewModels.SalaryPagesVM;
using AADizErp.ViewModels.SettingsVM;
using CommunityToolkit.Maui;
using DevExpress.Maui;
using DevExpress.Maui.Core;
using Plugin.Maui.Biometric;
using ZXing.Net.Maui.Controls;
[assembly: Preserve(AllMembers = true)]
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
                .UseDevExpressCharts()
                .UseDevExpressEditors()
                .UseDevExpress()
                .UseDevExpressCollectionView()
                .UseDevExpressControls()
                .UseDevExpressDataGrid()
                .UseDevExpressScheduler()
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .UseMauiCommunityToolkit()
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
            //builder.Services.AddSingleton<IBadge>(Badge.Default);
            builder.Services.AddSingleton(NotificationCounter.Default);

            //Custom Services
            builder.Services.AddSingleton<NotificationService>();
            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddSingleton<AttendanceService>();
            builder.Services.AddSingleton<DeviceTokenService>();
            builder.Services.AddSingleton<LeaveService>();
            builder.Services.AddSingleton<HrService>();
            builder.Services.AddSingleton<OccasionService>();
            builder.Services.AddSingleton<LocalDbService>();
            builder.Services.AddSingleton<ConveyanceService>();
            builder.Services.AddSingleton<MisServices>();
            builder.Services.AddSingleton<MachineService>();
            builder.Services.AddSingleton<Mis_eAttendanceServices>();
            //builder.Services.AddSingleton<ZXing.Net.Maui.Controls.CameraView>();

            //Pages and viewmodels
            #region Pages and Viewmodels
            builder.Services.AddTransient<RequestLandingPage>();

            builder.Services.AddTransient<RemoteAttendancePageViewModel>();
            builder.Services.AddTransient<AttendanceRequestPage>();

            builder.Services.AddTransient<LeaveRequestPageViewModel>();
            builder.Services.AddTransient<LeaveRequestPage>();

            builder.Services.AddTransient<LeaveRequestFormPopup>();

            builder.Services.AddTransient<ApprovalLandingPage>();
            builder.Services.AddTransient<ApprovalLandingPageViewModel>();

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

            builder.Services.AddTransient<ConveyanceViewPage>();
            builder.Services.AddTransient<ConveyanceViewPageViewModel>();

            builder.Services.AddTransient<NptLandingPage>();
            builder.Services.AddTransient<McBarcodeScanPage>();

            builder.Services.AddTransient<AddUnconfirmEmployeePage>();
            builder.Services.AddTransient<AddEmployeeViewModel>();

            builder.Services.AddTransient<MisLandingPage>();
            builder.Services.AddTransient<OverTimePage>();
            builder.Services.AddTransient<OverTimePageViewModel>();

            builder.Services.AddTransient<eAttendancePage>();
            builder.Services.AddTransient<Mis_eAttendancePageViewModel>();
            builder.Services.AddTransient<ProductionPage>();
            builder.Services.AddTransient<MachinesPage>();
            builder.Services.AddTransient<GroupMachineStatusPageViewModel>();
            builder.Services.AddTransient<McStatusUpdatePage>();
            builder.Services.AddTransient<McStatusUpdatePageViewModel>();
            builder.Services.AddTransient<MachineInfoPage>();
            builder.Services.AddTransient<MachineInfoPageViewModel>();

            #endregion

            return builder.Build();
        }
    }
}
