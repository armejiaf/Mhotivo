 using System;
using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class AcademicCourseDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Horario")]
        [DataType(DataType.Time)]
        public string Schedule { get; set; }

        [Display(Name = "Curso")]
        public string Course { get; set; }

        [Display(Name = "Maestro/a")]
        public string Teacher { get; set; }
    }

    public class AcademicCourseEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Horario")]
        [Display(Name = "Horario")]
        [DataType(DataType.Time)]
        public TimeSpan Schedule { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Maestro")]
        [Display(Name = "Maestro/a")]
        public long Teacher { get; set; }
    }
}