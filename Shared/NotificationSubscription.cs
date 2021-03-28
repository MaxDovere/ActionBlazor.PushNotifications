using System.ComponentModel.DataAnnotations;

namespace ActionBlazor.PushNotifications.Shared
{
    public class NotificationSubscription
    {
        public int NotificationSubscriptionId { get; set; }

        public int NotificationId { get; set; }
        
        public int SubsctiptionId { get; set; }
        
        public string UserId { get; set; }
    }
}
