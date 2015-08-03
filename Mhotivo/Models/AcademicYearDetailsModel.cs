 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class DisplayAcademicYearDetailsModel
    {
        public ICollection<AcademicYearDetail> AcademicYears { get; set; }
        public int Id { get; set; }

        [Display(Name = "Fecha inicio de clase")]
        public DateTime TeacherStartDate { get; set; }

        [Display(Name = "Fecha de fin de clase")]
        public DateTime TeacherEndDate { get; set; }

        [Display(Name = "Horario")]
        public DateTime Schedule { get; set; }

        [Display(Name = "Aula")]
        public string Room { get; set; }

        [Display(Name = "Curso")]
        public string Course { get; set; }

        [Display(Name = "Maestro/a")]
        public string Teacher { get; set; }

    }

    public class AcademicYearDetailsRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar una Fecha de Ingreso del Maestro")]
        [Display(Name = "Fecha ingreso del maestro")]
        public string TeacherStartDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Fecha de Salida del Maestro")]
        [Display(Name = "Fecha de salida del maestro")]
        public string TeacherEndDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Horario")]
        [Display(Name = "Horario")]
        public string Schedule { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Aula")]
        [Display(Name = "Aula")]
        public string Room { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Curso")]
        [Display(Name = "Curso")]
        public Course Course { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Maestro")]
        [Display(Name = "Maestro")]
        public Teacher Teacher { get; set; }

        [Display(Name = "Id Año Academico")]
        public int AcademicYearId { get; set; }

    }

    public class AcademicYearDetailsEditModel
    {
        public ICollection<AcademicYearDetail> AcademicYears { get; set; }
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Fecha de Ingreso del Maestro")]
        [Display(Name = "Fecha ingreso del maestro")]
        public string TeacherStartDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Fecha de Salida del Maestro")]
        [Display(Name = "Fecha de salida del maestro")]
        public string TeacherEndDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Horario")]
        [Display(Name = "Horario")]
        public string Schedule { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Aula")]
        [Display(Name = "Aula")]
        public string Room { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Curso")]
        [Display(Name = "Curso")]
        public Course Course { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una Maestro")]
        [Display(Name = "Maestro/a")]
        public Teacher Teacher { get; set; }
    }
}