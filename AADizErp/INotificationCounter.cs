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
    }

    public static class NotificationCounter
    {
        static INotificationCounter defaultImplementation;

        public static void SetNotificationCount(int count)
        {
            Default.SetNotificationCount(count);
        }

        public static INotificationCounter Default =>
            defaultImplementation ??= new NotificationCounterImplementation();

        internal static void SetDefault(INotificationCounter implementation) =>
            defaultImplementation = implementation;
    }
}
