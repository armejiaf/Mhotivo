using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class NotificationRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar un Titulo")]
        [Display(Name = "Titulo")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Requiere un mensaje para la Notificacion")]
        [Display(Name = "Mensaje")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Notificacion")]
        [Display(Name = "Tipo de Notificacion")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Enviar Notificacion por correo?")]
        public bool SendEmail { get; set; }
        
        [Required(ErrorMessage = "Debe selecionar una opcion.")]
        public long Id1 { get; set; }

        [Required(ErrorMessage = "Debe selecionar una opcion.")]
        public long Id2 { get; set; }

        [Required(ErrorMessage = "Debe selecionar una opcion.")]
        public long DestinationId { get; set; }

        public long NotificationCreator { get; set; }
    }

    public class NotificationSelectListsModel
    {
        public SelectList EducationLevels { get; set; }
        public SelectList Grades { get; set; }
        public SelectList AcademicGrades { get; set; }
        public SelectList AcademicCourses { get; set; }
        public SelectList Personals { get; set; }
    }

    public class NotificationDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Display(Name = "Tipo de Notificacion")]
        public NotificationType NotificationType { get; set; }

        [Display(Name="Destinatario")]
        public string DestinationId { get; set; }

        [Display(Name = "Creador")]
        public string NotificationCreator { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public string CreationDate { get; set; }

        [Display(Name="Notificacion Enviada?")]
        public bool	Sent { get; set; }

        [Display(Name="Notificacion por Correo?")]
        public bool SendEmail { get; set; }

        [Display(Name = "Aprobada?")]
        public bool Approved { get; set; }
    }

    public class NotificationEditModel
    {
        public long Id { get; set; }
        public bool Approved { get; set; }
    }
    public class NotificationPostApproveEditModel : NotificationEditModel
    {
        [Required(ErrorMessage = "Requiere un mensaje para la Notificacion")]
        [Display(Name = "Mensaje")]
        public string Message { get; set; }
    }

    public class NotificationPreApproveEditModel : NotificationEditModel
    {

        [Required(ErrorMessage = "Debe Ingresar un Titulo")]
        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Requiere un mensaje para la Notificacion")]
        [Display(Name = "Mensaje")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Notificacion")]
        [Display(Name = "Tipo de Notificacion")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Enviar Notificacion por correo?")]
        public bool SendEmail { get; set; }

        [Required(ErrorMessage = "Debe selecionar una opcion.")]
        public long Id1 { get; set; }

        [Required(ErrorMessage = "Debe selecionar una opcion.")]
        public long Id2 { get; set; }

        [Required(ErrorMessage = "Debe selecionar una opcion.")]
        public long DestinationId { get; set; }
    }
}