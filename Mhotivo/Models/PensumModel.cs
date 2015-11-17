using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

    public class PensumCourseModel
    {
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        public List<CourseRegisterModel> Courses { get; set; } 
    }
}