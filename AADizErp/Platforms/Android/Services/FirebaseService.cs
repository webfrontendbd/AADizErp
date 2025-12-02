using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using DevExpress.Maui.Core.Internal;
using Firebase.Messaging;

namespace AADizErp.Platforms.Android.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);

            // Save device token
            Preferences.Set("DeviceToken", token);
        }

        //public override void OnMessageReceived(RemoteMessage message)
        //{
        //    base.OnMessageReceived(message);

        //    try
        //    {
        //        var notification = message.GetNotification();
        //        string title = notification?.Title ?? "New Notification";
        //        string body = notification?.Body ?? message.Data.GetValueOrDefault("body", "You have a new message");

        //        SendNotification(title, body, message.Data);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"FCM Message Error: {ex.Message}");
        //    }
        //}

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            try
            {

                var notification = message.GetNotification();
                SendNotification(notification.Body, notification.Title, message.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FCM Message Error: {ex.Message}");

            }
        }
        private void SendNotification(string title, string body, IDictionary<string, string> data)
        {
            // Notify MainActivity to handle navigation
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

            // Push extra data from FCM payload
            if (data != null)
            {
                foreach (var pair in data)
                {
                    intent.PutExtra(pair.Key, pair.Value);
                }
            }

            PendingIntent pendingIntent;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                pendingIntent = PendingIntent.GetActivity(
                    this,
                    0,
                    intent,
                    PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable
                );
            }
            else
            {
                pendingIntent = PendingIntent.GetActivity(
                    this,
                    0,
                    intent,
                    PendingIntentFlags.UpdateCurrent
                );
            }

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.Channel_ID)
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(body))
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetPriority(NotificationCompat.PriorityHigh);

            var notificationManager = NotificationManagerCompat.From(this);

            // Instead of overwriting same notification, generate unique ID
            int notifId = Java.Lang.JavaSystem.CurrentTimeMillis().GetHashCode();

            notificationManager.Notify(notifId, notificationBuilder.Build());
        }
    }
}
