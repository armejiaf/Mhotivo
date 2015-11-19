using System.ComponentModel.DataAnnotations;

namespace Mhotivo.ParentSite.Models
{
    public class UpdateParentMailModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Compare("Email", ErrorMessage = "Los correos no coinciden")]
        public string ConfirmEmail { get; set; }
    }
}