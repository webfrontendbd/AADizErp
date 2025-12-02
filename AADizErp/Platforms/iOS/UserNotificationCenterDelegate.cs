using AADizErp.Models;
using CommunityToolkit.Mvvm.Messaging;
using Foundation;
using UIKit;
using UserNotifications;

namespace AADizErp.Platforms.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        // When user taps a notification
        public override void DidReceiveNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            Action completionHandler)
        {
            var userInfo = response.Notification.Request.Content.UserInfo;

            try
            {
                // Safely extract NavigationID
                string navigationID = null;
                if (userInfo != null && userInfo.ContainsKey(new NSString("NavigationID")))
                {
                    navigationID = userInfo["NavigationID"]?.ToString();
                }

                if (!string.IsNullOrWhiteSpace(navigationID))
                {
                    Preferences.Set("NavigationID", navigationID);
                }

                // Send message to MAUI side
                WeakReferenceMessenger.Default.Send(new PushNotificationReceived("test"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Notification Tap Error: {ex.Message}");
            }

            completionHandler?.Invoke();
        }

        // When a notification arrives while the app is open (foreground)
        public override void WillPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            try
            {
                var badgeCount = notification.Request.Content.Badge?.Int32Value ?? 0;

                // iOS 17+ requires new API
                if (UIDevice.CurrentDevice.CheckSystemVersion(17, 0))
                {
                    UNUserNotificationCenter.Current.SetBadgeCount(new IntPtr(badgeCount), null);
                }
                else
                {
                    UIApplication.SharedApplication.ApplicationIconBadgeNumber = badgeCount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Badge Update Error: {ex.Message}");
            }

            // Show Banner + Sound + Badge when app is open
            completionHandler(
                UNNotificationPresentationOptions.Banner |
                UNNotificationPresentationOptions.Sound |
                UNNotificationPresentationOptions.Badge
            );
        }
    }
}
