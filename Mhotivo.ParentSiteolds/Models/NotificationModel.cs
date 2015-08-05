using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using User = System.Web.Providers.Entities.User;

namespace Mhotivo.ParentSite.Models
{
    public class NotificationModel
    {
        public long Id { get; set; }
        public string NotificationName { get; set; }
        public string NotificationCreator { get; set; }
        //public NotificationType NotificationTypeId { get; set; }

        public string Message { get; set; }
        public DateTime Created { get; set; }
        public int CommentsAmount { get; set; }
        
        
    }
}