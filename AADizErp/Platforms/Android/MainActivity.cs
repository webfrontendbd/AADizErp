using AADizErp.Models;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using CommunityToolkit.Mvvm.Messaging;

namespace AADizErp
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize |
                               ConfigChanges.Orientation |
                               ConfigChanges.UiMode |
                               ConfigChanges.ScreenLayout |
                               ConfigChanges.SmallestScreenSize |
                               ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        internal static readonly string Channel_ID = "TestChannel";
        internal static readonly int NotificationID = 101;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            new ImageCropper.Maui.Platform().Init(this);

            base.OnCreate(savedInstanceState);

            InitializeNotificationPermissions();
        }

        // ----------------------------------------------------------
        //   NOTIFICATION PERMISSION HANDLING (ANDROID 33+)
        // ----------------------------------------------------------

        private const int NotificationPermissionRequestCode = 1010;

        private void InitializeNotificationPermissions()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                RequestNotificationPermissionFor33Plus();
            }
            else
            {
                CreateNotificationChannel();
            }
        }

        private void RequestNotificationPermissionFor33Plus()
        {
            var permission = Android.Manifest.Permission.PostNotifications;

            if (ContextCompat.CheckSelfPermission(this, permission) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(
                    this,
                    new[] { permission },
                    NotificationPermissionRequestCode);
            }
            else
            {
                CreateNotificationChannel();
            }
        }

        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == NotificationPermissionRequestCode &&
                grantResults.Length > 0 &&
                grantResults[0] == Permission.Granted)
            {
                CreateNotificationChannel();
            }
        }

        // ----------------------------------------------------------
        //   PUSH NOTIFICATION DATA HANDLING
        // ----------------------------------------------------------

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent?.Extras == null)
                return;

            foreach (var key in intent.Extras.KeySet())
            {
                if (key == "NavigationID")
                {
                    string idValue = intent.Extras.GetString(key);

                    Preferences.Remove("NavigationID");
                    Preferences.Set("NavigationID", idValue);

                    // Notify VM
                    WeakReferenceMessenger.Default.Send(new PushNotificationReceived("test"));
                }
            }
        }

        // ----------------------------------------------------------
        //   ANDROID NOTIFICATION CHANNEL
        // ----------------------------------------------------------

        private void CreateNotificationChannel()
        {
            if (!OperatingSystem.IsAndroidVersionAtLeast(26))
                return;

            var channel = new NotificationChannel(
                Channel_ID,
                "Test Notification Channel",
                NotificationImportance.Default);

            var manager = (NotificationManager)GetSystemService(NotificationService);
            manager.CreateNotificationChannel(channel);
        }
    }
}