using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActionBlazor.PushNotifications.Server.Models
{
    public interface IVapidKeysSettings
    {
        string Publickey { get; set; }
        string Privatekey { get; set; }
    }

    public class VapidKeysSettings: IVapidKeysSettings
    {
        //public string Subject { get; } = "actioncrm@gmail.com";
        public string Publickey { get; set; }
        public string Privatekey { get; set; }
    }
}
