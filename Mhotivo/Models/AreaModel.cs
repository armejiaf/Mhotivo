using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class DisplayAreaModel
    {
        [Display(Name = "Código del área")]
        public long Id { get; set; }

        [Display(Name = "Nombre del área")]
        public string Name { get; set; }
    }
    public class AreaRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar en nombre del Area")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }

    public class AreaEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}