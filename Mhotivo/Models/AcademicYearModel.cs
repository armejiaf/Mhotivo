using System.ComponentModel.DataAnnotations;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class AcademicYearDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Año")]
        public int Year { get; set; }

        [Display(Name = "Activo?")]
        public bool IsActive { get; set; }

        [Display(Name = "Matricula Abierta?")]
        public bool EnrollsOpen { get; set; }
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

        [Display(Name = "Matricula Abierta")]
        public bool EnrollsOpen { get; set; }
    }

    public class NewAcademicYearGradeSpecModel
    {
        public long Grade { get; set; }
        public Grade Reference { get; set; }
        public int Sections { get; set; }
        public long SelectedPensum { get; set; }
    }
}