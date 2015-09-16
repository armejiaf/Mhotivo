using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Mhotivo.Models
{
    public class ImportDataModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Debe Ingresar el año")]
        [Display(Name = "Año")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Debe ingresar el grado")]
        [Display(Name = "Grado")]
        public long GradeImport { get; set; }

        [Required(ErrorMessage = "Debe ingresar la seccion")]
        [Display(Name = "Seccion")]
        [MaxLength(1, ErrorMessage = "La longitud debe ser de 1")]
        public string Section { get; set; }

        [Required(ErrorMessage = "Debe especificar el archivo a subir")]
        [Display(Name = "Archivo Excel")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase UploadFile { get; set; }
    }
}