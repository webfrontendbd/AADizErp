using AADizErp.Pages.HolidayPages;
using AADizErp.Pages.HRPages;
using AADizErp.Pages.ManagerPages;
using AADizErp.Pages.RequestPages;
using AADizErp.Pages.SalaryPages;
using AADizErp.ViewModels;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using AADizErp.ViewModels.ManagerPagesVM;
using CommunityToolkit.Mvvm.Messaging;
using AADizErp.Models;
using AADizErp.Pages.SettingsPages;
using AADizErp.Services;

namespace AADizErp
{
    public partial class MainPage : ContentPage
    {
        ImageUploadService imageUploadService { get; set; }
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            imageUploadService = new ImageUploadService();
            WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
            {
                string msg = m.Value;
                NavigateToPage();
            });

            BindingContext = viewModel;
            ReadFireBaseAdminSdk();
            NavigateToPage();
        }

        private async void UploadImage_Clicked(object sender, EventArgs e)
        {
            var img = await imageUploadService.OpenMediaPickerAsync();

            var imagefile = await imageUploadService.Upload(img);

            Image_Upload.Source = ImageSource.FromStream(() =>
                imageUploadService.ByteArrayToStream(imageUploadService.StringToByteBase64(imagefile.byteBase64))
            );
        }

        private async void ReadFireBaseAdminSdk()
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("fcm_sdk.json");
            var reader = new StreamReader(stream);

            var jsonContent = reader.ReadToEnd();


            if (FirebaseMessaging.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(jsonContent)
                });
            }
        }

        private void NavigateToPage()
        {
            if (Preferences.ContainsKey("NavigationID"))
            {
                string id = Preferences.Get("NavigationID", "");
                if (id == "1")
                {
                    Shell.Current.GoToAsync($"{nameof(MgrAttendanceDetailsPageViewModel)}");
                }
                Preferences.Remove("NavigationID");
            }
        }
        private async void ApprovalMenu_Tapped(object sender, EventArgs e)
        {
            var userInfo = await App.GetUserInfo();
            if (userInfo.Permissions.Contains("approve"))
            {
                await Shell.Current.GoToAsync($"{nameof(ApprovalLandingPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Unauthorized!", "You are not authorized to use this feature", "OK");
            }
        }

        private async void RequestMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(RequestLandingPage)}");
        }

        private async void PaySlipMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(PaySlipListViewPage)}");
        }
        private async void HrPageMenu_Tapped(object sender, EventArgs e)
        {
            var userInfo = await App.GetUserInfo();
            if (userInfo.Factories.Length > 0)
            {
                await Shell.Current.GoToAsync($"{nameof(HrLandingPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Unauthorized!", "You are not authorized to use this feature", "OK");
            }
        }

        private async void HolidayPageMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(HolidayViewPage)}");
        }

        private async void AsianEventMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(AsianEventPage)}");
        }

        private async void ProfilePageMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(ProfileViewPage)}");
            //await Shell.Current.DisplayAlert("Unauthorized!", "You are not authorized to use this feature", "OK");
        }

        private async void TimeCardPageMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(IndividualTimeCardViewPage)}");
        }

        private async void SettingPageMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(SettingsLandingPage)}");
            //await Shell.Current.DisplayAlert("Unauthorized!", "You are not authorized to use this feature", "OK");
        }

    }
}