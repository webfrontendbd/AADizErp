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
                Firebase.Core.App.Configure();
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    var authOption = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;

                    UNUserNotificationCenter.Current.RequestAuthorization(authOption, (granted, error) =>
                    {
                        Console.WriteLine(granted);
                    });

                    UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

                    Messaging.SharedInstance.AutoInitEnabled = true;
                    Messaging.SharedInstance.AutoInitEnabled = true;
                    Messaging.SharedInstance.Delegate = this;
                }
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return base.FinishedLaunching(application, launchOptions);
        }

        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // Here Handle Notification Navigation WhenNotification Is REceived
        }


        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            if (Preferences.ContainsKey("DeviceToken"))
            {
                Preferences.Remove("DeviceToken");
            }
            Preferences.Set("DeviceToken", fcmToken);

            //App.Current.MainPage.DisplayAlert("Ok", fcmToken, "OK");
        }

    }
}
