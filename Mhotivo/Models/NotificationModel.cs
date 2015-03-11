using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class NotificationModel /*TODO: Separate model from entity */
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre de Notificacion")]
        [Display(Name = "Name")]
        public string NotificationName { get; set; }

        [Required(ErrorMessage = "Requiere un mensaje para la Notificacion")]
        //[Display(Name = "Mensaje")]
        [AllowHtml]
        public string Message { get; set; }

        [Editable(false)]
        [Display(Name = "Approved")]
        public bool Approved { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Notificacion")]
        [Display(Name = "Tipo de Notificacion")]
        public SelectList NotificationTypeSelectList { get; set; }

        [Required(ErrorMessage = "Debe Ingresar opcion de tipo de Notificacion")]
        [Display(Name = "Tipo de Notificacion")]
        public SelectList NotificationTypeOpionSelectList { get; set; }
        
        [Required(ErrorMessage = "Debe Ingresar Tipo de Notificacion")]
        public int GradeIdifNotificationTypePersonal { get; set; } //si el NotificationTypeOpionSelectList es Grado que guarde que grado
        
        [Display(Name = "Enviar Notificacion por correo?")]
        public bool SendingEmail { get; set; }

        [Required(ErrorMessage = "Debe Ingresar opcion de tipo de Notificacion")]
        public int IdIsGradeAreaGeneralSelected { get; set; }//id de grado,area,user seleccionado

        [Required(ErrorMessage = "Debe Ingresar Tipo de Notificacion")]
        public int NotificationTypeId { get; set; } // For the the selected Product
        
        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Estudiante")]
        [Display(Name = "Estudiante")]
        public SelectList StudentOptionSelectList { get; set; }

        [Required(ErrorMessage = "Debe Ingresar opcion de tipo de Notificacion")]
        public int StudentId { get; set; }//id de grado,area,user seleccionado
    }
}