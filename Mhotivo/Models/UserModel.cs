using System.ComponentModel.DataAnnotations;
using Mhotivo.Implement.Attributes;

namespace Mhotivo.Models
{
    public class UserDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Correo Elctrónico")]
        public string Email { get; set; }

        [Display(Name = "Nombre")]
        public string UserOwner { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public string Role { get; set; }
    }

    public class NewUserDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Correo Elctrónico")]
        public string Email { get; set; }

        [Display(Name = "Nombre")]
        public string UserOwner { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public string Role { get; set; }
    }

    public class NewUserDefaultPasswordDisplayModel
    {
        public long Id { get; set; }

        [Display(Name = "Nombre")]
        public string UserOwner { get; set; }

        [Display(Name = "Contraseña Temporal")]
        public string DefaultPassword { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Debe Ingresar Contraseña actual")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nueva contraseña")]
        [StringLength(100, ErrorMessage = "El número de caracteres debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        [NotEqualTo("OldPassword", ErrorMessage = "Debe ingresar una contraseña diferente a la actual.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar la nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Display(Name = "Email de usuario")]
        [Required(ErrorMessage = "Debe Ingresar Email de Usuario")]
        [EmailAddress]
        public string UserEmail { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe Ingresar Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar cuenta?")]
        public bool RememberMe { get; set; }
    }

    public class UserEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Correo")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Display(Name = "Activo?")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Usuario")]
        [Display(Name = "Tipo de Usuario")]
        public long Role { get; set; }
    }
}