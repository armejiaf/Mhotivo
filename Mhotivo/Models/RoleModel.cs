using System;
using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class RoleEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Nombre")]
        [Display(Name = "Nombre")]
        public String Name { get; set; }

        [Display(Name = "Descripción")]
        public String Description { get; set; }
    }

    public class DisplayRolModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public String Name { get; set; }
        [Display(Name = "Descripción")]
        public String Description { get; set; }
    }

}