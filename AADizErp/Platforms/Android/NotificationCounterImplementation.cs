﻿using Android.App;
using AndroidX.Core.App;

namespace AADizErp
{
    public class NotificationCounterImplementation: INotificationCounter
    {
        public void SetNotificationCount(int count)
        {
            ME.Leolin.Shortcutbadger.ShortcutBadger.ApplyCount(Android.App.Application.Context, count);
            NotificationCompat.Builder builder = new(Android.App.Application.Context, $"{Android.App.Application.Context.PackageName}.channel");
            builder.SetNumber(count);
            builder.SetContentTitle(" ");
            builder.SetContentText("");
            builder.SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon);
            var notification = builder.Build();
            var notificationManager = NotificationManager.FromContext(Android.App.Application.Context);
            CreateNotificationChannel();
            notificationManager?.Notify((int)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), notification);
        }

        private static void CreateNotificationChannel()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(26))
            {
                using var channel = new NotificationChannel($"{Android.App.Application.Context.PackageName}.channel", "Notification channel", NotificationImportance.Default)
                {
                    Description = "Default notification channel"
                };
                var notificationManager = NotificationManager.FromContext(Android.App.Application.Context);
                notificationManager?.CreateNotificationChannel(channel);
            }
        }
    }
}
