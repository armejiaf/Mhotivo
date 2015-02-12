using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mhotivo.Models
{
    public class NotificationTypeModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NotificationTypeId { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Descripcion")]
        [Display(Name = "Descripcion")]
        public string TypeDescription { get; set; }
    }
}