using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
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

        [AllowHtml]
        public string Message { get; set; }

        //[Required(ErrorMessage = "Debe Seleccionar el Tipo de Notificacion")]
        //[Display(Name = "Tipo de Notificacion")]
        //public NotificationType NotificationTypeId { get; set; }

        public DateTime Created { get; set; }
    }
}