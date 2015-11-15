using System.ComponentModel.DataAnnotations;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class DisplayGradeModel
    {
        public long Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Nivel Educativo")]
        public string EducationLevel { get; set; }
    }

    public class GradeEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nivel Educativo")]
        [Display(Name = "Nivel Educativo")]
        public EducationLevel EducationLevel { get; set; }
    }

    public class GradeRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nivel Educativo")]
        [Display(Name = "Nivel Educativo")]
        public long EducationLevel { get; set; }
    }
}