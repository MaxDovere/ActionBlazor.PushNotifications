using ActionBlazor.PushNotifications.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ActionBlazor.PushNotifications.Client
{
    public class NotificationUsersClient
    {
        private readonly HttpClient httpClient;

        public NotificationUsersClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<NotificationWithStatus>> GetNotifications() =>
            await httpClient.GetFromJsonAsync<IEnumerable<NotificationWithStatus>>("notifications");


        public async Task<NotificationWithStatus> GetNotificationById(int notificationId) =>
            await httpClient.GetFromJsonAsync<NotificationWithStatus>($"notifications/{notificationId}");


        public async Task<int> SendNotification(NotificationUsers notification)
        {
            var response = await httpClient.PostAsJsonAsync("notifications", notification);
            response.EnsureSuccessStatusCode();
            var notificationId = await response.Content.ReadFromJsonAsync<int>();
            return notificationId;
        }

        public async Task SubscribeToNotifications(SubscriptionUsers subscription)
        {
            var response = await httpClient.PutAsJsonAsync("subscriptions/subscribe", subscription);
            response.EnsureSuccessStatusCode();
        }
    }
}
