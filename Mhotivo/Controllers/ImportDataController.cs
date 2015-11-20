using System;
using System.Collections.Generic;
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
        private readonly IDataImportService _dataImportService;
        private readonly IGradeRepository _gradeRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IAcademicGradeRepository _academicGradeRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public ImportDataController(IDataImportService dataImportService
                                    ,IGradeRepository gradeRepository
                                    ,IAcademicYearRepository academicYearRepository, IAcademicGradeRepository academicGradeRepository)
        {
            _dataImportService = dataImportService;
            _gradeRepository = gradeRepository;
            _academicYearRepository = academicYearRepository;
            _academicGradeRepository = academicGradeRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }
        
        [AuthorizeAdmin]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var importModel = new ImportDataModel();
            ViewBag.GradeId = new SelectList(_gradeRepository.Query(x => x), "Id", "Name", 0);
            ViewBag.Year = new SelectList(_academicYearRepository.Filter(x => x.IsActive).Select(x => x.Year).Distinct().ToList());
            ViewBag.Section = new List<SelectListItem>();
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
            var academicYear = _academicYearRepository.Filter(x => x.Year == importModel.Year
            && x.Grades.Any(n => n.Grade.Id == importModel.GradeImport && n.Section == importModel.Section)).FirstOrDefault();
            if (academicYear == null)
                ModelState.AddModelError("Year", "No existe ese año academico");
            ViewBag.GradeId = new SelectList(_gradeRepository.Query(x => x), "Id", "Name", 0);
            if (!ModelState.IsValid)
            {
                return View(importModel);
            }
            var myDataSet = _dataImportService.GetDataSetFromExcelFile(importModel.UploadFile);
            try
            {
                _dataImportService.Import(myDataSet, academicYear);
            }
            catch(Exception ex)
            {
                _viewMessageLogic.SetNewMessage("Error!", ex.Message, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            
            const string title = "Importación de Datos Correcta";
            var content = string.Format("Se importaron datos para el año: {0}, grado: {1} y sección: {2}"
                                        , importModel.Year // 0
                                        , academicYear.Grades.ElementAt(0).Grade.Name // 1
                                        , importModel.Section // 2
                                       );
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }


        public JsonResult LoadByGrade(ImportDataModel importModel)
        {
            if (importModel.Year == 0)
            {
                var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == importModel.GradeImport).ToList();
                var toReturn =
                    new SelectList(
                        sList, "Id", "Section");
                return Json(toReturn, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == importModel.GradeImport && x.AcademicYear.Id == importModel.Year).ToList();
                var toReturn =
                    new SelectList(
                        sList, "Id", "Section");
                return Json(toReturn, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
