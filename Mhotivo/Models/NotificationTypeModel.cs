using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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