using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class NotificationMainModel
    {
        [Required(ErrorMessage = "Debe Ingresar un Titulo")]
        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Requiere un mensaje para la Notificacion")]
        [AllowHtml]
        public string Message { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Notificacion")]
        [Display(Name = "Tipo de Notificacion")]
        public NotificationType NotificationType { get; set; }

        [Display(Name = "Enviar Notificacion por correo?")]
        public bool SendEmail { get; set; }

        public SelectListNotificationRegisterModel DestinationSelectModel { get; set; }
    }

    public class SelectListNotificationRegisterModel
    {
        public long DestinationId { get; set; }
    }
    public class EducationLevelNotificationRegisterModel : SelectListNotificationRegisterModel
    {
        public SelectList EducationLevelSelectList { get; set; }
    }

    public class GradeNotificationRegisterModel : SelectListNotificationRegisterModel
    {
        public SelectList GradeSelectList { get; set; }
    }

    public class AcademicGradeNotificationRegisterModel : SelectListNotificationRegisterModel
    {
        public AcademicGradeNotificationRegisterModel()
        {
            GradeId = -1;
        }
        public long GradeId { get; set; }
        public SelectList GradeSelectList { get; set; }
        public SelectList AcademicGradeSelectList { get; set; }
    }

    public class AcademicCourseNotificationRegisterModel : SelectListNotificationRegisterModel
    {
        public AcademicCourseNotificationRegisterModel()
        {
            GradeId = -1;
            AcademicGradeId = -1;
        }
        public long GradeId { get; set; }
        public long AcademicGradeId { get; set; }
        public SelectList GradeSelectList { get; set; }
        public SelectList AcademicGradeSelectList { get; set; }
        public SelectList AcademicCourseSelectList { get; set; }
    }

    public class PersonalNotificationRegisterModel : SelectListNotificationRegisterModel
    {
        public PersonalNotificationRegisterModel()
        {
            GradeId = -1;
            AcademicGradeId = -1;
        }
        public long GradeId { get; set; }
        public long AcademicGradeId { get; set; }
        public SelectList GradeSelectList { get; set; }
        public SelectList AcademicGradeSelectList { get; set; }
        public SelectList PersonalSelectList { get; set; }
    }

    public class NotificationEditModel
    {
        
    }

    public class NotificationDisplayModel
    {
        
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
        [AllowHtml]
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