using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Mhotivo.Models
{
    public class CourseDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }

    public class CourseEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }

    public class CourseRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre:")]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public long Pensum { get; set; }
    }
}
