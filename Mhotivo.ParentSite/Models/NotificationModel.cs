using System;

namespace Mhotivo.ParentSite.Models
{
    public class NotificationModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string NotificationCreator { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public int CommentsAmount { get; set; }
    }
}