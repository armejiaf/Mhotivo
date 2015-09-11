using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class DisplayClassActivityModel
    {
        public long Id { get; set; }

        [Display(Name = "Año Académico")]
        public string AcademicYear { get; set; }

        [Display(Name = "Título")]
        public string Name { get; set; }

        [Display(Name = "Tipo")]
        public string Type { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Valor")]
        public string Value { get; set; }
    }

    public class ClassActivityEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Año Académico")]
        [Display(Name = "Año Académico")]
        public long AcademicYearId { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Título")]
        [Display(Name = "Título")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo")]
        [Display(Name = "Tipo")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Descripción")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Valor")]
        [Display(Name = "Valor")]
        public double Value { get; set; }
    }

    public class ClassActivityRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar Año Académico")]
        [Display(Name = "Año Académico")]
        public long AcademicYearId { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Título")]
        [Display(Name = "Título")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo")]
        [Display(Name = "Tipo")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Descripción")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Valor")]
        [Display(Name = "Valor")]
        public double Value { get; set; }
    }
}