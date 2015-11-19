using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class AcademicYearDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Año")]
        public int Year { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }
    }

    public class AcademicYearRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar Año")]
        [Display(Name = "Año")]
        public int Year { get; set; }
    }

    public class AcademicYearEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Año")]
        [Display(Name = "Año")]
        public int Year { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }
    }
}