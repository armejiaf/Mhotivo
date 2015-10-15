using System;
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
        public long IdGradeAreaUserGeneralSelected { get; set; }
        public long GradeIdifNotificationTypePersonal { get; set; }
       // public long StudentId { get; set; }
        public bool SendingEmail { get; set; }
        public User NotificationCreator { get; set; }
        public long UserCreatorId { get; set; }
        public string UserCreatorName { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public bool Approved { get; set; }
        public Student TargetStudent { get; set; }
        public virtual ICollection<NotificationComments> NotificationComments { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public string Section { get; set; }
    }
}
