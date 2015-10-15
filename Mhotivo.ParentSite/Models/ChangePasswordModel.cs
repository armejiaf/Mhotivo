using System.ComponentModel.DataAnnotations;

namespace Mhotivo.ParentSite.Models
{
    public class ChangePasswordModel
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas")]
        public string ConfirmNewPassword { get; set; }
    }
}