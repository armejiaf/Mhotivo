using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class AdministrativeDisplayModel
    {
        public long Id { get; set; }

        public ICollection<ContactInformation> ContactInformation { get; set; }

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

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "Dirección Principal")]
        public string Address { get; set; }

        [Display(Name = "Sexo")]
        public string MyGender { get; set; }

        [Display(Name = "Foto Perfil")]
        public byte[] Photo { get; set; }

        [Display(Name="Rol de Usuario")]
        public string Role { get; set; }
    }

    public class AdministrativeEditModel
    {
        public long Id { get; set; }

        public ICollection<ContactInformation> ContactInformation { get; set; }

        [Display(Name = "Foto Perfil")]
        public byte[] Photo { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombres")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Número de Identidad")]
        [Display(Name = "Número de Identidad")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Debe Ingresar Fecha de Nacimiento")]
        //[Display(Name = "Fecha de Nacimiento")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime BirthDate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Ciudad")]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Estado")]
        [Display(Name = "Estado")]
        public string State { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Dirección")]
        [StringLength(300, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 10)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Sexo")]
        [Display(Name = "Sexo")]
        public Gender MyGender { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadPhoto { get; set; }
    }

    public class AdministrativeRegisterModel
    {

        [Required(ErrorMessage = "Debe Ingresar Nombres")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Número de Identidad")]
        [Display(Name = "Número de Identidad")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Debe Ingresar Fecha de Nacimiento")]
        //[Display(Name = "Fecha de Nacimiento")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime BirthDate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Ciudad")]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Estado")]
        [Display(Name = "Estado")]
        public string State { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Dirección")]
        [StringLength(300, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 10)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Un Correo Electronico")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe especificar el rol del usuario.")]
        [Display(Name = "Rol de Usuario")]
        public long Role { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Sexo")]
        [Display(Name = "Sexo")]
        public Gender MyGender { get; set; }
    }
}