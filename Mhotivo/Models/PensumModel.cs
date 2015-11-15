using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class PensumDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }

    public class PensumRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar un Nombre")]
        [Display(Name = "Nombre")]
        public string Name  { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Grado")]
        [Display(Name = "Grado")]
        public long Grade { get; set; }

    }

    public class PensumEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}