using System.ComponentModel.DataAnnotations;
using Mhotivo.Implement.Attributes;

namespace Mhotivo.ParentSite.Models
{
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
}