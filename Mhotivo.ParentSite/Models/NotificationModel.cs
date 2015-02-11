using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;

namespace Mhotivo.ParentSite.Models
{
    public class NotificationModel
    {
        public long Id { get; set; }
        public string NotificationName { get; set; }
        public User NotificationCreator { get; set; }
        //public NotificationType NotificationTypeId { get; set; }

        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}