using System;
using Mhotivo.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Mhotivo.Models
{
    public class StudentDisplayModel
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

        [Display(Name = "Número de Identidad")]
        public string IdNumber { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "Dirección Principal")]
        public string Address { get; set; }

        [Display(Name = "Sexo")]
        public string MyGender { get; set; }

        [Display(Name = "Tipo de Sangre")]
        public string BloodType { get; set; }

        [Display(Name = "Número de Cuenta")]
        public string AccountNumber { get; set; }

        [Display(Name = "Tutor o Padre")]
        public string Tutor1 { get; set; }

        [Display(Name = "Segundo Tutor o Madre")]
        public string Tutor2 { get; set; }

        [Display(Name = "Foto Perfil")]
        public byte[] Photo { get; set; }
    }

    public class StudentEditModel
    {
        public long Id { get; set; }

        public ICollection<ContactInformation> ContactInformation { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombres")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Número de Identidad")]
        [Display(Name = "Número de Identidad")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Apellidos")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Ciudad")]
        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Estado")]
        [Display(Name = "Estado")]
        public string State { get; set; }

        [StringLength(300, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 10)]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Foto Perfil")]
        public HttpPostedFileBase FilePicture { get; set; }

        [Display(Name = "Foto Perfil")]
        public byte[] Photo { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Sexo")]
        [Display(Name = "Sexo")]
        public Gender MyGender { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Sangre")]
        [Display(Name = "Tipo de Sangre")]
        public string BloodType { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Numero de Cuenta")]
        [Display(Name = "Número de Cuenta")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tutor o Tutor")]
        [Display(Name = "Padre o Tutor")]
        public long Tutor1 { get; set; }

        [Display(Name = "Madre o Segundo Tutor")]
        public long? Tutor2 { get; set; }
    }

    public class StudentRegisterModel
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

        [Required(ErrorMessage = "Debe Ingresar Fecha de Nacimiento")]
        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

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

        [Required(ErrorMessage = "Debe Ingresar Tipo de Sangre")]
        [Display(Name = "Tipo de Sangre")]
        public string BloodType { get; set; }

        [Required(ErrorMessage = "Debe Ingresar un Número de Cuenta")]
        [Display(Name = "Número de Cuenta")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tutor o Tutor")]
        [Display(Name = "Padre o Tutor")]
        public long Tutor1 { get; set; }

        [Display(Name = "Madre o Segundo Tutor")]
        public long? Tutor2 { get; set; }
    }
}