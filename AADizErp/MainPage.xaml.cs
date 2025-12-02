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
using AADizErp.Pages.NptPages;
using AADizErp.Pages.MisPages;

namespace AADizErp
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _localDbService;
        private bool _isInitialized = false;

        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();

            _localDbService = new LocalDbService();

            // Register safely for push notifications
            WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
            {
                try
                {
                    NavigateToPage();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Push Notification Navigation Error: {ex.Message}");
                }
            });

            BindingContext = viewModel;

            // Start async tasks safely
            InitAsync();
        }

        private async void InitAsync()
        {
            try
            {
                if (!_isInitialized)
                {
                    _isInitialized = true;

                    await LoadUserProfileImageSafe();
                    await InitializeFirebaseAdminSafe();
                    NavigateToPage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MainPage Init Error: {ex.Message}");
            }
        }

        // -----------------------------------------------------------------------
        // 1️⃣ SAFE USER PROFILE LOADING
        // -----------------------------------------------------------------------
        private async Task LoadUserProfileImageSafe()
        {
            try
            {
                var userInfo = await App.GetUserInfo();
                var user = await _localDbService.GetByUsername(userInfo.TokenUserMetaInfo.UserName);

                if (!string.IsNullOrWhiteSpace(user?.ProfilePic))
                    DisplayImageFromBase64(user.ProfilePic);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Profile Load Error: {ex.Message}");
            }
        }

        // -----------------------------------------------------------------------
        // 2️⃣ SAFE FIREBASE ADMIN SDK INITIALIZATION
        // -----------------------------------------------------------------------
        private async Task InitializeFirebaseAdminSafe()
        {
            try
            {
                // NEVER call Firebase Admin SDK inside a mobile app.
                // But if your app requires it, load it safely.
                if (FirebaseMessaging.DefaultInstance != null)
                    return;

                using var stream = await FileSystem.OpenAppPackageFileAsync("fcm_sdk.json");
                using var reader = new StreamReader(stream);

                var jsonContent = reader.ReadToEnd();

                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(jsonContent)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Firebase Admin SDK Init Error: {ex.Message}");
            }
        }

        private void DisplayImageFromBase64(string base64String)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);
                Image_Upload.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image Decode Error: {ex.Message}");
            }
        }

        // -----------------------------------------------------------------------
        // 3️⃣ SAFE NAVIGATION FROM PUSH NOTIFICATIONS
        // -----------------------------------------------------------------------
        private void NavigateToPage()
        {
            try
            {
                if (!Preferences.ContainsKey("NavigationID"))
                    return;

                string id = Preferences.Get("NavigationID", null);

                Preferences.Remove("NavigationID");

                if (string.IsNullOrWhiteSpace(id))
                    return;

                // Add more mappings here (clean, scalable)
                switch (id)
                {
                    case "1":
                        Shell.Current.GoToAsync($"{nameof(MgrAttendanceDetailsPageViewModel)}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"NavigateToPage Error: {ex.Message}");
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

        private async void OnClickedCircle(object sender, EventArgs e)
        {
            var userInfo = await App.GetUserInfo();
            if (userInfo?.TokenUserMetaInfo?.UserName == null)
                return;

            var cropper = new ImageCropper.Maui.ImageCropper
            {
                CropShape = ImageCropper.Maui.ImageCropper.CropShapeType.Oval,

                Success = async (imageFile) =>
                {
                    if (string.IsNullOrWhiteSpace(imageFile))
                    {
                        Console.WriteLine("Invalid image path.");
                        return;
                    }

                    try
                    {
                        var imageBytes = await File.ReadAllBytesAsync(imageFile);
                        var base64String = Convert.ToBase64String(imageBytes);

                        var profileImage = new UserProfileImage
                        {
                            UserName = userInfo.TokenUserMetaInfo.UserName,
                            ProfilePic = base64String
                        };

                        await _localDbService.Create(profileImage);

                        // Update UI on main thread
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Image_Upload.Source = ImageSource.FromFile(imageFile);
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing image: {ex.Message}");
                    }
                },

                Failure = () =>
                {
                    Console.WriteLine("Error capturing or cropping the image.");
                }
            };

            cropper.Show(this);
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
            var allowedRoles = new[] { "Super Admin", "HR Manager", "HR Executive", "HR GM", "HR Officer", "IT GM", "IT Executive" };
            var userInfo = await App.GetUserInfo();

            if (userInfo.Roles.Any(r => allowedRoles
            .Contains(r, StringComparer.OrdinalIgnoreCase))
                && userInfo.Factories.Length > 0)
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
        }

        private async void TimeCardPageMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(IndividualTimeCardViewPage)}");
        }

        private async void SettingPageMenu_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(SettingsLandingPage)}");
        }

        private async void MaintenanceMenu_Tapped(object sender, EventArgs e)
        {
            var allowedRoles = new[] { "Super Admin", "Maintenance", "Maintenance Manager", "Maintenance GM", "IT GM", "IE Manager" };
            var userInfo = await App.GetUserInfo();
            if (userInfo.Roles.Any(r => allowedRoles.Contains(r, StringComparer.OrdinalIgnoreCase)))
            {
                await Shell.Current.GoToAsync($"{nameof(NptLandingPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Unauthorized!", "You are not authorized to use this feature", "OK");
            }

        }

        private async void MisMenu_Tapped(object sender, EventArgs e)
        {
            var allowedRoles = new[] { "Super Admin", "IT GM" };
            var userInfo = await App.GetUserInfo();
            if (userInfo.Roles.Any(r => allowedRoles.Contains(r, StringComparer.OrdinalIgnoreCase)))
            {
                await Shell.Current.GoToAsync($"{nameof(MisLandingPage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Unauthorized!", "You are not authorized to use this feature", "OK");
            }

        }
    }
}