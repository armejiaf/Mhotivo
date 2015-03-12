using System;
using System.Collections;
using System.Collections.Generic;
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
        public NotificationType NotificationType { get; set; }

        public int IdGradeAreaUserGeneralSelected { get; set; }

        public int GradeIdifNotificationTypePersonal { get; set; }
        public int StudentId { get; set; }
        public bool SendingEmail { get; set; }

        public virtual User NotificationCreator { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }

        public bool Approved { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<NotificationComments> NotificationComments { get; set; }

    }
}
