using System;
using System.ComponentModel.DataAnnotations;
//TODO: Seemingly deprecated. Delete?
namespace Mhotivo.Models
{
    public class EventCreateModel
    {
        [Required(ErrorMessage = "Debe Ingresar Fecha de Inicio")]
        [Display(Name = "Fecha de Inicio")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "Fecha de Finalización")]
        public DateTime EndDateTime { get; set; }

        [Required(ErrorMessage = "Debe Ingresar Descripción")]
        public String Description { get; set; }
    }
}