using System.ComponentModel.DataAnnotations;

namespace ActionBlazor.PushNotifications.Shared
{
    public class SubscriptionUsers
    {
        public int SubscriptionId { get; set; }

        public string UserId { get; set; }

        public string Url { get; set; }

        public string P256dh { get; set; }

        public string Auth { get; set; }
    }
}
