using System.Web.Mvc;
using Mhotivo.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class DisplayHomeworkModel
    {
        public long Id { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }
        [AllowHtml]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Día de entrega")]
        public string DeliverDate { get; set; }

        [Display(Name = "Puntaje")]
        public float Points { get; set; }

        [Display(Name = "Clase")]
        public int AcademicYearId { get; set; }

        [Display(Name = "Materia")]
        public virtual AcademicYearDetail AcademicYearDetail { get; set; }
    }

    public class CreateHomeworkModel
    {
        [Required(ErrorMessage = "Debe Ingresar título de la tarea.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una descripción.")]
        [AllowHtml]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Debe Ingresar fecha de entrega.")]
        [Display(Name = "Día de entrega")]
        public string DeliverDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar puntaje.")]
        [Display(Name = "Puntaje")]
        public float Points { get; set; }

        [Display(Name = "Materia")]
        public int Course { get; set; }
    }

    public class EditHomeworkModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Debe Ingresar título de la tarea.")]
        [Display(Name = "Titulo")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Debe Ingresar una descripción.")]
        [AllowHtml]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Debe Ingresar fecha de entrega.")]
        [Display(Name = "Dia de entrega")]
        public string DeliverDate { get; set; }

        [Required(ErrorMessage = "Debe Ingresar puntaje.")]
        [Display(Name = "puntaje")]
        public float Points { get; set; }

        [Display(Name = "Materia")]
        public virtual AcademicYearDetail AcademicYearDetail { get; set; }
    }
}