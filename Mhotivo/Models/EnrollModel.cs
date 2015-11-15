using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class EnrollDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Nombre")]
        public string FullName { get; set; }

        [Display(Name = "Foto Perfil")]
        public byte[] Photo { get; set; }

        [Display(Name = "Genero")]
        public string MyGender { get; set; }

        [Display(Name = "Numero de Cuenta")]
        public string AccountNumber { get; set; }

        [Display(Name = "Grado")]
        public string Grade { get; set; }

        [Display(Name = "Seccion")]

        public string Section { get; set; }
    }

    public class EnrollRegisterModel
    {
        [Required(ErrorMessage = "Debe elegir un grado.")]
        [Display(Name = "Grado")]
        public long Grade { get; set; }

        [Required(ErrorMessage = "Debe elegir una seccion.")]
        [Display(Name = "Seccion")]
        public long AcademicGrade { get; set; }

        [Required(ErrorMessage = "Debe elegir un estudiante.")]
        [Display(Name = "Estudiante")]
        public long Student { get; set; }
    }

    public class EnrollEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe elegir un grado.")]
        [Display(Name = "Grado")]
        public long Grade { get; set; }

        [Required(ErrorMessage = "Debe elegir una seccion.")]
        [Display(Name = "Seccion")]
        public long AcademicGrade { get; set; }

        [Required(ErrorMessage = "Debe elegir un estudiante.")]
        [Display(Name = "Estudiante")]
        public long Student { get; set; }
    }

    public class EnrollDeleteModel
    {
        [Required(ErrorMessage = "Debe elegir un grado.")]
        [Display(Name = "Grado")]
        public long Grade { get; set; }

        [Required(ErrorMessage = "Debe elegir una seccion.")]
        [Display(Name = "Seccion")]
        public long AcademicGrade { get; set; }
    }
}