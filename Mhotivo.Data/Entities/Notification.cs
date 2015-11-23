using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public enum NotificationType
    {
        [Description("General")]
        General = 1, //No destination ID needed
        [Description("Nivel de Educacion")]
        EducationLevel = 2, //EducationLevel ID needed
        [Description("Grado")]
        Grade = 3, //Grade ID needed
        [Description("Seccion")]
        Section= 4, //AcademicGrade ID needed
        [Description("Materia")]
        Course = 5, //AcademicCourse ID needed
        [Description("Estudiante")]
        Personal = 6 //Student ID needed
    }

    public class Notification
    {
        public Notification()
        {
            NotificationComments = new HashSet<NotificationComment>();
            RecipientUsers = new HashSet<User>();
            CreationDate = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public long DestinationId { get; set; }
        public virtual PeopleWithUser NotificationCreator { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Approved { get; set; }
        public bool Sent { get; set; }
        public bool SendEmail { get; set; }
        public virtual AcademicYear AcademicYear { get; set; } //used to show only pertinent Notifications
        public virtual ICollection<NotificationComment> NotificationComments { get; set; }
        public virtual ICollection<User> RecipientUsers { get; set; }
    }
}
