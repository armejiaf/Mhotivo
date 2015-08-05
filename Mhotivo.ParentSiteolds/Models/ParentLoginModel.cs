﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mhotivo.ParentSite.Models
{
    public class ParentLoginModel
    {
        [Display(Name = "Email de usuario")]
        [Required(ErrorMessage = "Debe Ingresar Email de Usuario")]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Debe Ingresar Contraseña")]
        public string Password { get; set; }
    }
}