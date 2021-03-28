using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using ActionBlazor.PushNotifications.Server.Data;
using ActionBlazor.PushNotifications.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebPush;

namespace ActionBlazor.PushNotifications.Server.Controllers
{
    [Route("subscriptions")]
    [ApiController]
    [Authorize]
    public class SubscriptionsController : Controller
    {
        private readonly ILogger<SubscriptionsController> _logger;

        private readonly ApplicationDbContext _context;

        public SubscriptionsController(ApplicationDbContext context, ILogger<SubscriptionsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPut("subscribe")]
        public async Task<SubscriptionUsers> Subscribe(SubscriptionUsers subscription)
        {
            // We're storing at most one subscription per user, so delete old ones.
            // Alternatively, you could let the user register multiple subscriptions from different browsers/devices.
            var userId = GetUserId();
            var oldSubscriptions = _context.SubscriptionUsers.Where(e => e.UserId == userId);
            _context.SubscriptionUsers.RemoveRange(oldSubscriptions);

            // Store new subscription
            subscription.UserId = userId;
            _context.SubscriptionUsers.Attach(subscription);

            await _context.SaveChangesAsync();
            return subscription;
        }

    }
}
