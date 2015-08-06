using System.ComponentModel.DataAnnotations;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class DisplayUserModel
    {
        public long Id { get; set; }

        [Display(Name = "Correo Elctrónico")]
        public string Email { get; set; }

        [Display(Name = "Nombre")]
        public string DisplayName { get; set; }

        [Display(Name = "Activo")]
        public string Status { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public Role Role { get; set; }

        public string RoleName { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required(ErrorMessage = "Debe Ingresar Contraseña actual")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nueva contraseña")]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
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

    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string DisplaName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Email")]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Display(Name = "Estado")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Contraseña")]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Usuario")]
        [Display(Name = "Tipo de Usuario")]
        public int RoleId { get; set; }
    }

    public class UserEditModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Activo")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo de Usuario")]
        [Display(Name = "Tipo de Usuario")]
        public int RoleId { get; set; }
    }
}