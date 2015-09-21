using System.Web;
using Mhotivo.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class DisplayTeacherModel
    {
        public long Id { get; set; }

        public ICollection<ContactInformation> Contacts { get; set; }

        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Display(Name = "Numero de Identidad")]
        public string IdNumber { get; set; }

        [Display(Name = "Nacionalidad")]
        public string Nationality { get; set; }

        [Display(Name = "País")]
        public string Country { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "Dirección Principal")]
        public string Address { get; set; }

        [Display(Name = "Foto Perfil")]
        public string UrlPicture { get; set; }

        [Display(Name = "Sexo")]
        public string Gender { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public string StartDate { get; set; }

        [Display(Name = "Fecha de Finalización")]
        public string EndDate { get; set; }

        [Display(Name = "Biografía")]
        public string Biography { get; set; }
    }

    public class TeacherEditModel
    {
        public long Id { get; set; }

        public ICollection<ContactInformation> Contacts { get; set; }

        [Display(Name = "Foto Perfil")]
        public byte[] Photo { get; set; }

        public string FullName { get; set; }

        [Display(Name = "Fecha de Finalización")]
        public string EndDate { get; set; }

        [Display(Name = "Biografía")]
        public string Biography { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombres")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Número de Identidad")]
        [Display(Name = "Número de Identidad")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Fecha de Nacimiento")]
        [Display(Name = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nacionalidad")]
        [Display(Name = "Nacionalidad")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Ciudad")]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Estado")]
        [Display(Name = "Estado")]
        public string State { get; set; }

        [Required(ErrorMessage = "Debe Ingresar País")]
        [Display(Name = "País")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Dirección")]
        [StringLength(300, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 10)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Sexo")]
        [Display(Name = "Sexo")]
        public string Gender { get; set; }
        
        [Display(Name = "Fecha de Inicio")]
        public string StartDate { get; set; }

        [Display(Name = "Foto Perfil")]
        public string UrlPicture { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadPhoto { get; set; }
    }

    public class TeacherRegisterModel
    {
        public string FullName { get; set; }

        [Display(Name = "Foto Perfil")]
        public string UrlPicture { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Fecha de Finalización")]
        [Display(Name = "Fecha de Finalización")]
        public string EndDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Biografía")]
        [Display(Name = "Biografía")]
        public string Biography { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombres")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Número de Identidad")]
        [Display(Name = "Número de Identidad")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Fecha de Nacimiento")]
        [Display(Name = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nacionalidad")]
        [Display(Name = "Nacionalidad")]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Ciudad")]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Estado")]
        [Display(Name = "Estado")]
        public string State { get; set; }

        [Required(ErrorMessage = "Debe Ingresar País")]
        [Display(Name = "País")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Dirección")]
        [StringLength(300, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 10)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Un Correo Electronico")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Una Contraseña")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Sexo")]
        [Display(Name = "Sexo")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Fecha de Inicio")]
        [Display(Name = "Fecha de Inicio")]
        public string StartDate { get; set; }
    }
}