using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class DisplayEducationLevelModel
    {
        [Display(Name = "Código del área")]
        public long Id { get; set; }

        [Display(Name = "Nombre del Nivel de Educacion")]
        public string Name { get; set; }
    }
    public class EducationLevelRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar en nombre del Nivel de Educacion")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }

    public class EducationLevelEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}