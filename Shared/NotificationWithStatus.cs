//using BlazingPizza.ComponentsLibrary.Map;
using System;
using System.Collections.Generic;

namespace ActionBlazor.PushNotifications.Shared
{
    public class NotificationWithStatus
    {
        public readonly static TimeSpan PreparationDuration = TimeSpan.FromSeconds(10);
        public readonly static TimeSpan DeliveryDuration = TimeSpan.FromSeconds(30);    // .FromMinutes(1) Minute Unrealistic, but more interesting to watch

        public NotificationUsers Notification { get; set; }

        public string StatusText { get; set; }

        public bool IsDelivered => StatusText == "Delivered";

        public static NotificationWithStatus FromExecute(NotificationUsers notification)
        {
            // To simulate a real backend process, we fake status updates based on the amount
            // of time since the order was placed
            string statusText;
            var dispatchTime = notification.CreatedTime.Add(PreparationDuration);

            if (DateTime.Now < dispatchTime)
            {
                statusText = "Preparing";
            }
            else if (DateTime.Now < dispatchTime + DeliveryDuration)
            {
                statusText = "Out for delivery";

                var proportionOfDeliveryCompleted = Math.Min(1, (DateTime.Now - dispatchTime).TotalMilliseconds / DeliveryDuration.TotalMilliseconds);
            }
            else
            {
                statusText = "Delivered";
            }

            return new NotificationWithStatus
            {
                Notification = notification,
                StatusText = statusText,
            };
        }
    }
}
