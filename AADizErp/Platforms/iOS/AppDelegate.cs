using AADizErp.Platforms.iOS;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;

namespace AADizErp
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate, IMessagingDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            try
            {
                // Initialize image cropper
                new ImageCropper.Maui.Platform().Init();

                // Initialize Firebase
                Firebase.Core.App.Configure();

                // iOS 10+ Notification setup
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    var authOptions = UNAuthorizationOptions.Alert |
                                      UNAuthorizationOptions.Badge |
                                      UNAuthorizationOptions.Sound;

                    UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                    {
                        Console.WriteLine($"Notification Permission Granted: {granted}");
                    });

                    UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

                    // Set FCM Delegate
                    Messaging.SharedInstance.Delegate = this;

                    // Enable auto initialization explicitly (once)
                    Messaging.SharedInstance.AutoInitEnabled = true;
                }

                // Register for APNs Token
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AppDelegate Init Error: {ex.Message}");
            }

            return base.FinishedLaunching(application, launchOptions);
        }

        // FIXED: Correct FCM token callback
        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fcmToken))
                {
                    Preferences.Set("DeviceToken", fcmToken);
                    Console.WriteLine($"FCM Token Received: {fcmToken}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FCM Token Error: {ex.Message}");
            }
        }

        // Notification tap handler
        //public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        //{
        //    Console.WriteLine("Notification received (foreground/background).");
        //}
    }
}
