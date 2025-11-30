using UIKit;
using UserNotifications;

namespace AADizErp
{
    public class NotificationCounterImplementation : INotificationCounter
    {
        public void SetNotificationCount(int count)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = count;
        }

        public void ClearNotificationCount()
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }
    }
    //public class NotificationCounterImplementation : INotificationCounter
    //{
    //    public void SetNotificationCount(int count)
    //    {
    //        UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Badge, (isAuthorized, _) =>
    //        {
    //        });
    //        UIApplication.SharedApplication.ApplicationIconBadgeNumber = count;
    //    }
    //}
}
