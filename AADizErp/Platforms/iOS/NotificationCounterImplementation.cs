using UIKit;

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
}
