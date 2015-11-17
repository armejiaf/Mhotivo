using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Mhotivo.ParentSite.Models
{
    public class EmailConfirmationModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Compare("Email", ErrorMessage = "Los correos no coinciden")]
        public string ConfirmEmail { get; set; }
    }
}