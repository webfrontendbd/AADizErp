using Android.App;
using AndroidX.Core.App;

namespace AADizErp
{
    public class NotificationCounterImplementation : INotificationCounter
    {
        const int NOTIFICATION_ID = 1001;
        const string CHANNEL_ID = "aadizerp.default.channel";
        //public void SetNotificationCount(int count)
        //{
        //    ME.Leolin.Shortcutbadger.ShortcutBadger.ApplyCount(Android.App.Application.Context, count);
        //    NotificationCompat.Builder builder = new(Android.App.Application.Context, $"{Android.App.Application.Context.PackageName}.channel");
        //    builder.SetNumber(count);
        //    builder.SetContentTitle(" ");
        //    builder.SetContentText("");
        //    builder.SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon);
        //    var notification = builder.Build();
        //    var notificationManager = NotificationManager.FromContext(Android.App.Application.Context);
        //    CreateNotificationChannel();
        //    notificationManager?.Notify((int)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), notification);
        //}

        public void SetNotificationCount(int count)
        {
            CreateNotificationChannel();

            var context = Android.App.Application.Context;
            var notificationManager = NotificationManager.FromContext(context);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                .SetContentTitle("New Updates")
                .SetContentText($"You have {count} unread items")
                .SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon)
                .SetNumber(count)
                .SetOngoing(true)  // Keep it active
                .SetAutoCancel(false);

            notificationManager.Notify(NOTIFICATION_ID, builder.Build());

            // Optional: OEM badge support
            ME.Leolin.Shortcutbadger.ShortcutBadger.ApplyCount(context, count);
        }
        public void ClearNotificationCount()
        {
            var context = Android.App.Application.Context;
            var notificationManager = NotificationManager.FromContext(context);
            notificationManager.Cancel(NOTIFICATION_ID);

            ME.Leolin.Shortcutbadger.ShortcutBadger.ApplyCount(context, 0);
        }

        //private static void CreateNotificationChannel()
        //{
        //    if (OperatingSystem.IsAndroidVersionAtLeast(26))
        //    {
        //        using var channel = new NotificationChannel($"{Android.App.Application.Context.PackageName}.channel", "Notification channel", NotificationImportance.Default)
        //        {
        //            Description = "Default notification channel"
        //        };
        //        var notificationManager = NotificationManager.FromContext(Android.App.Application.Context);
        //        notificationManager?.CreateNotificationChannel(channel);
        //    }
        //}

        private static void CreateNotificationChannel()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(26))
            {
                var context = Android.App.Application.Context;
                var channel = new NotificationChannel(
                    CHANNEL_ID,
                    "General Notifications",
                    NotificationImportance.High)
                {
                    Description = "Default notifications"
                };
                var notificationManager = NotificationManager.FromContext(context);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
    }
}
