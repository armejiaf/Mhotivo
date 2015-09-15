using System;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Authorizations;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class ImportDataController : Controller
    {
        private readonly IImportDataRepository _importDataRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public ImportDataController(IImportDataRepository importDataRepository
                                    ,IGradeRepository gradeRepository
                                    ,IAcademicYearRepository academicYearRepository)
        {
            _importDataRepository = importDataRepository;
            _gradeRepository = gradeRepository;
            _academicYearRepository = academicYearRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }
         [AuthorizeAdmin]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var importModel = new ImportDataModel();
            ViewBag.GradeId = new SelectList(_gradeRepository.Query(x => x), "Id", "Name", 0);
            importModel.Year = DateTime.Now.Year;
            return View(importModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Index(ImportDataModel importModel)
        {
            var validImageTypes = new []
            {
                "application/vnd.ms-excel"
            };
            bool errorExcel;
            if (importModel.UploadFile != null && importModel.UploadFile.ContentLength > 0)
                errorExcel = !validImageTypes.Contains(importModel.UploadFile.ContentType);
            else
                errorExcel = true;
            if(errorExcel)
                ModelState.AddModelError("UploadFile", "Por favor seleccione un archivo de Excel");
            var academicYear = _academicYearRepository.GetByFields(importModel.Year, importModel.GradeImport, importModel.Section);
            if (academicYear == null)
                ModelState.AddModelError("Year", "No existe ese año academico");
            ViewBag.GradeId = new SelectList(_gradeRepository.Query(x => x), "Id", "Name", 0);
            if (!ModelState.IsValid)
            {
                return View(importModel);
            }
            var myDataSet = _importDataRepository.GetDataSetFromExcelFile(importModel.UploadFile);
            _importDataRepository.Import(myDataSet, academicYear);
            const string title = "Importación de Datos Correcta";
            var content = string.Format("Se importaron datos para el año: {0}, grado: {1} y sección: {2}"
                                        , importModel.Year // 0
                                        , importModel.GradeImport // 1
                                        , importModel.Section // 2
                                       );
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }
    }
}
