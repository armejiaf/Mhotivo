using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public enum NotificationType
    {
        General = 1, //No destination ID needed
        EducationLevel = 2, //EducationLevel ID needed
        Grade = 3, //Grade ID needed
        Section= 4, //AcademicGrade ID needed
        Course = 5, //AcademicCourse ID needed
        Personal = 6 //Student ID needed
    }

    public class Notification
    {
        public Notification()
        {
            NotificationComments = new HashSet<NotificationComment>();
            CreationDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public long DestinationId { get; set; }
        public virtual User NotificationCreator { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Approved { get; set; }
        public bool Sent { get; set; }
        public bool SendEmail { get; set; }
        public virtual AcademicYear AcademicYear { get; set; } //used to show only pertinent Notifications
        public virtual ICollection<NotificationComment> NotificationComments { get; set; }
    }
}
