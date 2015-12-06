using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mhotivo.Implement.Attributes;

namespace Mhotivo.Models
{
    public class AccountEditModel
    {
        public long Id { get; set; }

        [Display(Name = "Foto Perfil")]
        public byte[] Photo { get; set; }

        [Display(Name = "Correo")]
        public string Email { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        
        [StringLength(100, ErrorMessage = "El número de caracteres debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        [NotEqualTo("OldPassword", ErrorMessage = "Debe ingresar una contraseña diferente a la actual.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadPhoto { get; set; }
    }
}