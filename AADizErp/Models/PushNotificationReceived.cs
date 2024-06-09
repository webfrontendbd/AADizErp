﻿using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AADizErp.Models
{
    public class PushNotificationReceived : ValueChangedMessage<string>
    {
        public PushNotificationReceived(string message) : base(message) { }
    }
}
