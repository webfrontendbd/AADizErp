using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADizErp
{
    public interface INotificationCounter
    {
        void SetNotificationCount(int count);
        void ClearNotificationCount();
    }

    public class NotificationCounter
    {
        private readonly INotificationCounter _counter;

        public NotificationCounter(INotificationCounter counter)
        {
            _counter = counter;
        }

        public void UpdateBadge(int unreadCount)
        {
            _counter.SetNotificationCount(unreadCount);
        }

        public void ClearBadge()
        {
            _counter.ClearNotificationCount();
        }
        //static INotificationCounter defaultImplementation;

        //public static void SetNotificationCount(int count)
        //{
        //    Default.SetNotificationCount(count);
        //}

        //public static INotificationCounter Default =>
        //    defaultImplementation ??= new NotificationCounterImplementation();

        //internal static void SetDefault(INotificationCounter implementation) =>
        //    defaultImplementation = implementation;
    }
}
