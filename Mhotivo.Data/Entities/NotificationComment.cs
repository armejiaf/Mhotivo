using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class NotificationComment
    {
        public NotificationComment()
        {
            CreationDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual User Commenter { get; set; }
        public Notification Notification { get; set; }
    }
}
