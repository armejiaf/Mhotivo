using System.ComponentModel.DataAnnotations;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class ContactInformationEditModel
    {
        public long Id { get; set; }

        public string Controller { get; set; }

        public People People { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo")]
        [Display(Name = "Campo (Telefono Movil, Telefono de Casa, etc.)")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Valor")]
        [Display(Name = "Valor")]
        public string Value { get; set; }
    }

    public class ContactInformationRegisterModel
    {
        public long People { get; set; }

        public string Controller { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Tipo")]
        [Display(Name = "Campo (Telefono Movil, Telefono de Casa, etc.)")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Valor")]
        [Display(Name = "Valor")]
        public string Value { get; set; }
    }
}