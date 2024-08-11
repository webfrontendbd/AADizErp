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

namespace AADizErp
{
    public partial class MainPage : ContentPage
    {
        ImageUploadService imageUploadService { get; set; }
        LocalDbService _localDbService { get; set; }
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            imageUploadService = new ImageUploadService();
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

        //private async void UploadImage_Clicked(object sender, EventArgs e)
        //{
        //    var img = await imageUploadService.OpenMediaPickerAsync();

        //    var imagefile = await imageUploadService.Upload(img);

        //    Image_Upload.Source = ImageSource.FromStream(() =>
        //        imageUploadService.ByteArrayToStream(imageUploadService.StringToByteBase64(imagefile.byteBase64))
        //    );
        //}

        private async void OnClickedCircle(object sender, EventArgs e)
        {
            UserInfo userInfo = await App.GetUserInfo();
            Image_Upload.Source = null;
            new ImageCropper.Maui.ImageCropper()
            {
                CropShape = ImageCropper.Maui.ImageCropper.CropShapeType.Oval,
                Success = async (imageFile) =>
                {
                    await Dispatcher.DispatchAsync(async () =>
                    {
                        // Read the image file into a byte array
                        byte[] imageBytes = await File.ReadAllBytesAsync(imageFile);
                        // Convert the byte array to a Base64 string
                        string base64String = Convert.ToBase64String(imageBytes);
                        // Create the UserProfileImage object with the Base64 string
                        UserProfileImage image = new UserProfileImage
                        {
                            UserName = userInfo.Username,
                            ProfilePic = base64String
                        };

                        // Save the object to the database
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

        //private void ImageTapped(object sender, EventArgs e)
        //{
        //    bottomSheet.State = BottomSheetState.HalfExpanded;
        //}

        //private async void DeletePhotoClicked(object sender, EventArgs args)
        //{
        //    await Dispatcher.DispatchAsync(() => {
        //        bottomSheet.State = BottomSheetState.Hidden;
        //        editControl.IsVisible = false;
        //        preview.Source = null;
        //    });
        //}

        //private async void SelectPhotoClicked(object sender, EventArgs args)
        //{
        //    var photo = await MediaPicker.PickPhotoAsync();
        //    await ProcessResult(photo);
        //    editControl.IsVisible = true;
        //}

        //private async void TakePhotoClicked(object sender, EventArgs args)
        //{
        //    if (!MediaPicker.Default.IsCaptureSupported)
        //        return;

        //    var photo = await MediaPicker.Default.CapturePhotoAsync();
        //    await ProcessResult(photo);
        //    editControl.IsVisible = true;
        //}

        //private async Task ProcessResult(FileResult result)
        //{
        //    await Dispatcher.DispatchAsync(() => {
        //        bottomSheet.State = BottomSheetState.Hidden;
        //    });


        //    if (result == null)
        //        return;

        //    ImageSource imageSource = null;
        //    if (System.IO.Path.IsPathRooted(result.FullPath))
        //        imageSource = ImageSource.FromFile(result.FullPath);
        //    else
        //    {
        //        var stream = await result.OpenReadAsync();
        //        imageSource = ImageSource.FromStream(() => stream);
        //    }
        //    var editorPage = new ImageEditView(imageSource);
        //    await Navigation.PushAsync(editorPage);

        //    var cropResult = await editorPage.WaitForResultAsync();
        //    if (cropResult != null)
        //        preview.Source = cropResult;

        //    editorPage.Handler.DisconnectHandler();
        //}

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
            UserProfileImage user = await _localDbService.GetById(2);
            await Shell.Current.GoToAsync($"{nameof(SettingsLandingPage)}");
            //await Shell.Current.DisplayAlert("Unauthorized!", "You are not authorized to use this feature", "OK");
        }

    }
}