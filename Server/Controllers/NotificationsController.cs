using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using ActionBlazor.PushNotifications.Server.Data;
using ActionBlazor.PushNotifications.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebPush;

namespace ActionBlazor.PushNotifications.Server.Controllers
{
    [Route("notifications")]
    [ApiController]
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(ApplicationDbContext context, ILogger<NotificationsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationWithStatus>>> GetNotifications()
        {
            var notifications = await _context.NotificationUsers
                .Where(o => o.UserId == GetUserId())
                //.Include(o => o.FK_Table).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
                .OrderByDescending(o => o.CreatedTime)
                .ToListAsync();

            return notifications.Select(o => NotificationWithStatus.FromExecute(o)).ToList();
        }

        [HttpGet("{notificationId}")]
        public async Task<ActionResult<NotificationWithStatus>> GetNotificationWithStatus(int notificationId)
        {
            var notification = await _context.NotificationUsers
                .Where(o => o.NotificationId == notificationId)
                .Where(o => o.UserId == GetUserId())
                //.Include(o => o.DeliveryLocation)
                .SingleOrDefaultAsync();

            if (notification == null)
            {
                return NotFound();
            }

            return NotificationWithStatus.FromExecute(notification);
        }

        [HttpPost]
        public async Task<ActionResult<int>> SendNotification(NotificationUsers notification)
        {
            notification.CreatedTime = DateTime.Now;
            notification.Source = "Action CRM";
            notification.Destination = "Client CRM";
            notification.UserId = GetUserId();

            _context.NotificationUsers.Attach(notification);
            await _context.SaveChangesAsync();

            // In the background, send push notifications if possible
            var subscription = await _context.SubscriptionUsers.Where(e => e.UserId == GetUserId()).SingleOrDefaultAsync();
            if (subscription != null)
            {
                _ = TrackAndSendNotificationsAsync(notification, subscription);
            }

            return notification.NotificationId;
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private static async Task TrackAndSendNotificationsAsync(NotificationUsers notification, SubscriptionUsers subscription)
        {
            // In a realistic case, some other backend process would track
            // order delivery progress and send us notifications when it
            // changes. Since we don't have any such process here, fake it.
            await Task.Delay(NotificationWithStatus.PreparationDuration);
            await SendNotificationAsync(notification, subscription, "Your notification has been dispatched!");

            await Task.Delay(NotificationWithStatus.DeliveryDuration);
            await SendNotificationAsync(notification, subscription, "Your notification is now delivered. Enjoy!");
        }

        private static async Task SendNotificationAsync(NotificationUsers notification, SubscriptionUsers subscription, string message)
        {
            // For a real application, generate your own
            var publicKey = "BNJd4yM_aOPtUZqI7IG8XWJNIHcg3iHHdmrWgv-6jLxMXySWeX7fRsl-xZcwj59GjYDWAwf4oc8UevQuViWgyPg";
            var privateKey = "W4Y49mec2mx6KoJ2I1yHgl0GoButX25SX1Sr2-GDl1I";

            var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
            var vapidDetails = new VapidDetails("mailto:<max.dovere@gmail.com>", publicKey, privateKey);
            var webPushClient = new WebPushClient();
            try
            {
                var payload = JsonSerializer.Serialize(new
                {
                    message,
                    url = $"mynotifications/{notification.NotificationId}",
                });
                await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error sending push notification: " + ex.Message);
            }
        }
        //public async Task NotificationOnlyText()
        //{
        //    var responseBody = await req.ReadAsStringAsync();
        //    NotificationMessageText messageText = JsonConvert.DeserializeObject<NotificationMessageText>(responseBody);
        //    var notifications = await _storageService.GetAllNotifications();
        //    var vapidDetails = new VapidDetails(webPushNotification.Subject, webPushNotification.PublicKey, webPushNotification.PrivateKey);
        //    foreach (var n in notifications)
        //    {
        //        var webPushClient = new WebPushClient();
        //        var pushSubscription = new PushSubscription(n.Url, n.P256dh, n.Auth);
        //        var payload = System.Text.Json.JsonSerializer.Serialize(messageText);
        //        await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
        //    }
        //    return new OkResult();
        //}

        //public async Task NotificationWithImages()
        //{
        //    var responseBody = await req.ReadAsStringAsync();
        //    NotificationImage messageImage = JsonConvert.DeserializeObject<NotificationImage>(responseBody);
        //    var notifications = await _storageService.GetAllNotifications();
        //    var vapidDetails = new VapidDetails(webPushNotification.Subject, webPushNotification.PublicKey, webPushNotification.PrivateKey);
        //    foreach (var n in notifications)
        //    {
        //        var webPushClient = new WebPushClient();
        //        var pushSubscription = new PushSubscription(n.Url, n.P256dh, n.Auth);
        //        var payload = System.Text.Json.JsonSerializer.Serialize(messageImage);
        //        try
        //        {
        //            await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
        //        }
        //        catch (WebPushException) // Sottoscrizioni non più valide
        //        {
        //            await _storageService.DeleteSubscription(n);
        //        }                
        //    }
        //    return new OkResult();
        //}
    }
}
