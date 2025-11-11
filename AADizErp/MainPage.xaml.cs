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
using DevExpress.Maui.Controls;
using AADizErp.Services;
using DevExpress.Utils.Design;
using CommunityToolkit.Maui.ApplicationModel;
using AADizErp.Pages.NptPages;
using AADizErp.Pages.MisPages;

namespace AADizErp
{
    public partial class MainPage : ContentPage
    {
        LocalDbService _localDbService { get; set; }
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            _localDbService = new LocalDbService();
            WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
            {
                string msg = m.Value;
                NavigateToPage();
            });

            BindingContext = viewModel;
            ReadFireBaseAdminSdk();
            NavigateToPage();
            
        }

        private async void OnClickedCircle(object sender, EventArgs e)
        {
            UserInfo userInfo = await App.GetUserInfo();
            new ImageCropper.Maui.ImageCropper()
            {
                CropShape = ImageCropper.Maui.ImageCropper.CropShapeType.Oval,
                Success = async (imageFile) =>
                {
                    await Dispatcher.DispatchAsync(async () =>
                    {
                        byte[] imageBytes = await File.ReadAllBytesAsync(imageFile);
                        string base64String = Convert.ToBase64String(imageBytes);
                        UserProfileImage image = new UserProfileImage
                        {
                            UserName = userInfo.TokenUserMetaInfo.UserName,
                            ProfilePic = base64String
                        };

                        await _localDbService.Create(image);

                        Image_Upload.Source = ImageSource.FromFile(imageFile);
                    });
                },
                Failure = () =>
                {
                    Console.WriteLine("Error capturing an image to crop.");
                }
            }.Show(this);
        }

        private async void ReadFireBaseAdminSdk()
        {
            var userInfo = await App.GetUserInfo();
            UserProfileImage user = await _localDbService.GetByUsername(userInfo.TokenUserMetaInfo.UserName);

            if (user != null && !string.IsNullOrEmpty(user.ProfilePic))
            {
                DisplayImageFromBase64(user.ProfilePic);
            }

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

        private void DisplayImageFromBase64(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            Stream imageStream = new MemoryStream(imageBytes);
            Image_Upload.Source = ImageSource.FromStream(() => imageStream);
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

        private async void NptMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(NptLandingPage)}");
        }

        private async void MisMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(MisLandingPage)}");
        }
    }
}