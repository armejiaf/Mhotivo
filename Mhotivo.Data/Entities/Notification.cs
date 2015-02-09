using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string NotificationName { get; set; }
        public User NotificationCreator { get; set; }
        public NotificationType NotificationTypeId { get; set; }

       // public  Student Sudent { get; set; }
        //public virtual string WithCopyTo { get; set; }
        //public virtual string WithHiddenCopyTo { get; set; }
        //public string Subject { get; set; }

        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
