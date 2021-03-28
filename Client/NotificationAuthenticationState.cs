using ActionBlazor.PushNotifications.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ActionBlazor.PushNotifications.Client
{
    public class NotificationAuthenticationState : RemoteAuthenticationState
    {
        public NotificationUsers Notification { get; set; }
    }
}
