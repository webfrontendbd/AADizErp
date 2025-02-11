using AADizErp.Models;
using CommunityToolkit.Mvvm.Messaging;
using UIKit;
using UserNotifications;

namespace AADizErp.Platforms.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var userInfo = response.Notification.Request.Content.UserInfo;

            string navigationID = userInfo["NavigationID"].ToString();
            Preferences.Set("NavigationID", navigationID);

            WeakReferenceMessenger.Default.Send(new PushNotificationReceived("test"));
        }
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var badgeCount = notification.Request.Content.Badge?.Int32Value ?? 0;
            if (UIDevice.CurrentDevice.CheckSystemVersion(17, 0))
            {
                UNUserNotificationCenter.Current.SetBadgeCount(new IntPtr(badgeCount), null);
            }
            else
            {
                UIApplication.SharedApplication.ApplicationIconBadgeNumber = badgeCount;
            }
            completionHandler(UNNotificationPresentationOptions.Badge);

            //completionHandler(UNNotificationPresentationOptions.Banner);
        }
    }
}
