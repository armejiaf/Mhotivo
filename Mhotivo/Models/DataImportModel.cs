using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Mhotivo.Models
{
    public class DataImportModel
    {
        [Required(ErrorMessage = "Debe Ingresar el año")]
        [Display(Name = "Año")]
        public long Year { get; set; }

        [Required(ErrorMessage = "Debe ingresar el grado")]
        [Display(Name = "Grado")]
        public long Grade { get; set; }

        [Required(ErrorMessage = "Debe ingresar la seccion")]
        [Display(Name = "Seccion")]
        public string Section { get; set; }

        [Required(ErrorMessage = "Debe especificar el archivo a subir")]
        [Display(Name = "Archivo Excel")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadFile { get; set; }
    }
}