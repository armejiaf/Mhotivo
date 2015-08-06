using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class DisplayAcademicYearModel
    {
        public ICollection<AcademicYear> AcademicYears { get; set; } //TODO: Seemingly unused. Delete?
        public long Id { get; set; }

        [Display(Name = "Grado")]
        public string Grade { get; set; }

        [Display(Name = "Año")]
        public int Year { get; set; }

        [Display(Name = "Sección")]
        public string Section { get; set; }

        [Display(Name = "Nivel")]
        public string EducationLevel { get; set; }

        [Display(Name = "Aprobado")]
        public bool Approved { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }
    }

    public class AcademicYearRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar un Grado")]
        [Display(Name = "Grado")]
        public Grade Grade { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Año")]
        [Display(Name = "Año")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Sección")]
        [Display(Name = "Sección")]
        public string Section { get; set; }

        [Required(ErrorMessage = "Debe Asignar un Nivel Educativo")]
        [Display(Name = "Sección")]
        public string EducationLevel { get; set; }

        [Display(Name = "Aprobado")]
        public string Approved { get; set; }

        [Display(Name = "Activo")]
        public string IsActive { get; set; }
    }

    public class AcademicYearEditModel
    {
        public ICollection<AcademicYear> AcademicYears { get; set; }
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Grado")]
        [Display(Name = "Grado")]
        public Grade Grade { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Año")]
        [Display(Name = "Año")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Sección")]
        [Display(Name = "Sección")]
        public string Section { get; set; }

        [Required(ErrorMessage = "Debe Asignar un Nivel Educativo")]
        [Display(Name = "Nivel")]
        public string EducationLevel { get; set; }

        [Display(Name = "Aprobado")]
        public string Approved { get; set; }

        [Display(Name = "Activo")]
        public string IsActive { get; set; }
    }

    //TODO: Unused. Delete?
    public class AcademicYearViewManagement
    {
        public IEnumerable<DisplayAcademicYearModel> Elements { get; set; }

        public bool CanGenerate { get; set; }

        public int CurrentYear { get; set; }
    }
}