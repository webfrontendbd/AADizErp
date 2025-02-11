using UIKit;
using UserNotifications;

namespace AADizErp
{
    public class NotificationCounterImplementation : INotificationCounter
    {
        public void SetNotificationCount(int count)
        {
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Badge, (isAuthorized, _) =>
            {
            });
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = count;
        }
    }
}
