using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class DisplayEnrollStudents
    {
        public long Id { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }

        [Display(Name = "Foto Perfil")]
        public string UrlPicture { get; set; }

        [Display(Name = "Sexo")]
        public string Gender { get; set; }

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
        public long GradeId { get; set; }

        [Required(ErrorMessage = "Debe elegir un estudiante.")]
        [Display(Name = "Estudiante")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe elegir una seccion.")]
        [Display(Name = "Seccion")]
        public string Section { get; set; }
    }
}