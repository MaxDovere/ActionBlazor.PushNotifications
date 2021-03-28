using System;
using System.ComponentModel.DataAnnotations;

namespace ActionBlazor.PushNotifications.Shared
{
    public class NotificationUsers
    {
        public int NotificationId { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Source  { get; set; }

        public string Destination { get; set; }
    }
}
