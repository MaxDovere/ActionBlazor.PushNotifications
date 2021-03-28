using ActionBlazor.PushNotifications.Shared;
using System.Collections.Generic;

namespace ActionBlazor.PushNotifications.Client
{
    public class NotificationState
    {
        public NotificationUsers Notification { get; private set; } = new NotificationUsers();

        public void ResetNotification()
        {
            Notification = new NotificationUsers();
        }

        public void ReplaceNotification(NotificationUsers notification)
        {
            Notification = notification;
        }
    }
}
