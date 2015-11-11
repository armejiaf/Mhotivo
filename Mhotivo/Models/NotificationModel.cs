using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class NotificationRegisterModel
    {
        public NotificationRegisterModel()
        {
            Id1 = -1;
            Id2 = -1;
        }
        [Required(ErrorMessage = "Debe Ingresar un Titulo")]
        [Display(Name = "Titulo")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Requiere un mensaje para la Notificacion")]
        [Display(Name = "Mensaje")]
        //[AllowHtml]
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

    public class NotificationSelectListsModel
    {
        public SelectList EducationLevels { get; set; }
        public SelectList Grades { get; set; }
        public SelectList AcademicGrades { get; set; }
        public SelectList AcademicCourses { get; set; }
        public SelectList Personals { get; set; }
    }
    public class NotificationEditModel
    {
        
    }

    public class NotificationDisplayModel
    {
        public long Id { get; set; }
        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Display(Name = "Tipo de Notificacion")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Aprobada?")]
        public bool Approved { get; set; }
    }

    public class NotificationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre de Notificacion")]
        [Display(Name = "Name")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Requiere un mensaje para la Notificacion")]
        [Display(Name="Mensaje")]
        //[AllowHtml]
        public string Message { get; set; }

        [Editable(false)]
        [Display(Name = "Notificacion Aprobada?")]
        public bool Approved { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Notificacion")]
        [Display(Name = "Tipo de Notificacion")]
        public NotificationType NotificationType { get; set; }

        [Required(ErrorMessage = "Debe Ingresar opcion de tipo de Notificacion")]
        [Display(Name = "Tipo de Notificacion")]
        public SelectList NotificationTypeOpionSelectList { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Notificacion")]
        public long GradeIdifNotificationTypePersonal { get; set; } //si el NotificationTypeOpionSelectList es Grado que guarde que grado

        [Display(Name = "Enviar Notificacion por correo?")]
        public bool SendEmail { get; set; }

        [Required(ErrorMessage = "Debe Ingresar opcion de tipo de Notificacion")]
        public string IdIsGradeAreaGeneralSelected { get; set; }//id de grado,area,user seleccionado

        [Required(ErrorMessage = "Debe Ingresar Tipo de Notificacion")]
        public long NotificationTypeId { get; set; } // For the the selected Product

        [Required(ErrorMessage = "Debe Ingresar Estudiante")]
        [Display(Name = "Estudiante")]
        public SelectList StudentOptionSelectList { get; set; }

        [Required(ErrorMessage = "Debe Ingresar opcion de tipo de Notificacion")]
        public long StudentId { get; set; }//id de grado,area,user seleccionado

        [Required(ErrorMessage = "Debe Ingresar opcion de tipo de Notificacion")]
        [Display(Name = "Seccion")]
        public string Section { get; set; } //seccion del grado, A, B, C o Todos
    }
}