using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class EducationLevelDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Nombre del Nivel de Educacion")]
        public string Name { get; set; }

        [Display(Name = "Director(a) a cargo del Nivel")]
        public string Director { get; set; }
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

    public class EducationLevelDirectorAssignModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un Director")]
        [Display(Name = "Director(a)")]
        public long Director { get; set; }
    }
}