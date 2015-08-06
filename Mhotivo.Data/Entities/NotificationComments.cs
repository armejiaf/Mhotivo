using System;
using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Data.Entities
{
    public class NotificationComments
    {
        [Key]
        public long Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual People Parent { get; set; }
    }
}
