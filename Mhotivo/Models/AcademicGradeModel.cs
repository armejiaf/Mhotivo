using System.ComponentModel.DataAnnotations;
using Microsoft.Office.Interop.Excel;

namespace Mhotivo.Models
{
    public class AcademicGradeDisplayModel
    {
        public long Id { get; set; }

        [Display(Name="Grado")]
        public string Grade { get; set; }

        [Display(Name = "Seccion")]
        public string Section { get; set; }

        [Display(Name = "Pensum Elegido")]
        public string ActivePensum { get; set; }

        [Display(Name = "Maestro de Grado")]
        public string SectionTeacher { get; set; }
    }

    public class AcademicGradeRegisterModel
    {
        public long AcademicYear { get; set; }

        [Display(Name = "Grado")]
        public long Grade { get; set; }

        [Display(Name = "Seccion")]
        public string Section { get; set; }

        [Display(Name = "Pensum a usarse")]
        public long ActivePensum { get; set; }
    }

    public class AcademicGradeEditModel
    {
        public long Id { get; set; }

        [Display(Name = "Grado")]
        public long Grade { get; set; }

        [Display(Name = "Seccion")]
        public string Section { get; set; }

        [Display(Name = "Pensum a usarse")]
        public long ActivePensum { get; set; }

        [Display(Name = "Maestro de Grado")]
        public long SectionTeacher { get; set; }
    }
}