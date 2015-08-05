using System.ComponentModel.DataAnnotations;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class DisplayCourseModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Area")]
        public Area Area { get; set; }
    }

    public class CourseEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Area")]
        [Display(Name = "Area")]
        public Area Area { get; set; }
    }

    public class CourseRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Area")]
        [Display(Name = "Area")]
        public int AreaId { get; set; }
    }

    public class CourseAreaEditModel
    {
        public long Id { get; set; }

        [Display(Name = "Area")]
        public string Name { get; set; }
    }
}
