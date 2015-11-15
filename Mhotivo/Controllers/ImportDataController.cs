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
        private readonly IImportDataRepository _importDataRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IAcademicGradeRepository _academicGradeRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public ImportDataController(IImportDataRepository importDataRepository
                                    ,IGradeRepository gradeRepository
                                    ,IAcademicYearRepository academicYearRepository, IAcademicGradeRepository academicGradeRepository)
        {
            _importDataRepository = importDataRepository;
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
             ViewBag.Section = new SelectList(new List<string> { "A", "B", "C" }, "A");
             return View(importModel);
        }

        /*[AuthorizeAdmin]
        public ActionResult DynamicDropDownList(long gradeId)
        {
            var model = new DynamicListModel();
            var items = _academicYearGradeRepository.Filter(x => x.Grade.Id == gradeId).Select(x => x.Section);
            foreach (var item in items)
            {
                model.Items.Add(new SelectListItem
                {
                    Text = item,
                    Value = item
                });
            }
            return View(model);
        }*/

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
            var myDataSet = _importDataRepository.GetDataSetFromExcelFile(importModel.UploadFile);
            try
            {
                _importDataRepository.Import(myDataSet, academicYear);
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
    }
}
