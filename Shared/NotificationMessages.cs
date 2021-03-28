using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionBlazor.PushNotifications.Shared
{
    public class NotificationMessageText
    {
        public string message { get; set; }
        public string body { get; set; }
        public NotifationAction[] actions { get; set; }
        public bool requireInteraction { get; set; } = true;
        public int[] vibrate { get; set; } = new int[] { 100, 50, 100 };
    }

    public class NotificationImage : NotificationMessageText
    {
        public string badge { get; set; }
        public string image { get; set; }
        public string icon { get; set; }
    }

    public class NotifationAction
    {
        public string action { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
    }
}
