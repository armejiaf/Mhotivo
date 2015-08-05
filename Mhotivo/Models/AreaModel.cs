using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mhotivo.Data.Entities;
namespace Mhotivo.Models
{
    public class DisplayAreaModel
    {
        //[Key]
        ///[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Código del área")]
        public int Id { get; set; }

        [Display(Name = "Nombre del área")]
        public string Name { get; set; }

        public bool Any()
        {
            throw new System.NotImplementedException();
        }
    }
    public class AreaRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar en nombre del Area")]
        [Display(Name = "Nombre")]
        public string DisplayName { get; set; }

     }
    public class AreaEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string DisplayName { get; set; }
    }
}