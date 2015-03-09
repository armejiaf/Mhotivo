using System;
using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class DisplayEnrollStudents
    {
        public int Id { get; set; }

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
        [Display(Name = "Grado")]
        public int GradeId { get; set; }

        [Display(Name = "Estudiante")]
        public int Id { get; set; }
    }
}