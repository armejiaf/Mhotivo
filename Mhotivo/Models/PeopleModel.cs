using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Mhotivo.Models
{
    public class DisplayPeopleModel
    {
        [Display(Name = "ID")]
        public long Id { get; set; }

        [Display(Name = "Identificación")]
        public string IdNumber { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public string BirthDay { get; set; }

        [Display(Name = "Nacionalidad")]
        public string Nationality { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Foto Perfil")]
        public string UrlPicture { get; set; }

        [Display(Name = "Sexo")]
        public string Sexo { get; set; }
    }

    public class PeopleRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Fecha de Nacimiento")]
        [Display(Name = "Fecha de Nacimiento")]
        public string BirthDay { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nacionalidad")]
        [Display(Name = "Nacionalidad")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Ciudad")]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Debe Ingresar País")]
        [Display(Name = "País")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Estado")]
        [Display(Name = "Estado")]
        public string State { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Dirección")]
        [StringLength(300, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 10)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Foto Perfil")]
        [Display(Name = "Foto Perfil")]
        public HttpPostedFileBase FilePicture { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Identificación")]
        [Display(Name = "Identificación")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Sexo")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }
    }

    public class PeopleEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Fecha de Nacimiento")]
        [Display(Name = "Fecha de Nacimiento")]
        public string BirthDay { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nacionalidad")]
        [Display(Name = "Nacionalidad")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Ciudad")]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Debe Ingresar País")]
        [Display(Name = "País")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Estado")]
        [Display(Name = "Estado")]
        public string State { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Dirección")]
        [StringLength(300, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 10)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Foto Perfil")]
        [Display(Name = "Foto Perfil")]
        public HttpPostedFileBase FilePicture { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Identificación")]
        [Display(Name = "Identificación")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Sexo")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre Completo")]
        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }

        public string UrlPicture { get; set; }
    }
}