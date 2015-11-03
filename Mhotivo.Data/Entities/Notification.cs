using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public enum NotificationType
    {
        General = 1,
        EducationLevel = 2,
        Grade = 3,
        Section= 4,
        Personal = 5
    }

    public class Notification
    {
        public Notification()
        {
            NotificationComments = new HashSet<NotificationComment>();
            Users = new HashSet<User>();
            CreationDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public long IdGradeAreaUserGeneralSelected { get; set; }
        public long GradeIdifNotificationTypePersonal { get; set; }
        public virtual User NotificationCreator { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Approved { get; set; }
        public virtual Student TargetStudent { get; set; }
        public virtual ICollection<NotificationComment> NotificationComments { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public string Section { get; set; }
    }
}
