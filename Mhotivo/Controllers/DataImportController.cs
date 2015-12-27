using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Mhotivo.Authorizations;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class DataImportController : Controller
    {
        private readonly IDataImportService _dataImportService;
        private readonly IGradeRepository _gradeRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IAcademicGradeRepository _academicGradeRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IUserRepository _userRepository;
        private readonly ISessionManagementService _sessionManagementService;

        public DataImportController(IDataImportService dataImportService
                                    ,IGradeRepository gradeRepository
                                    ,IAcademicYearRepository academicYearRepository, IAcademicGradeRepository academicGradeRepository, IUserRepository userRepository, ISessionManagementService sessionManagementService)
        {
            _dataImportService = dataImportService;
            _gradeRepository = gradeRepository;
            _academicYearRepository = academicYearRepository;
            _academicGradeRepository = academicGradeRepository;
            _userRepository = userRepository;
            _sessionManagementService = sessionManagementService;
            _viewMessageLogic = new ViewMessageLogic(this);
        }
        
        [AuthorizeAdminDirector]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var importModel = new DataImportModel();
            var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
            var isDirector = ViewBag.IsDirector = user.Role.Name.Equals("Director");
            ViewBag.GradeId = isDirector
                ? new SelectList(_gradeRepository.Filter(x => x.EducationLevel.Director != null && x.EducationLevel.Director.Id == user.Id).ToList(), "Id", "Name", 0)
                : new SelectList(_gradeRepository.GetAllGrade(), "Id", "Name", 0);
            ViewBag.Year = new SelectList(_academicYearRepository.Filter(x => x.EnrollsOpen), "Id", "Year");
            ViewBag.Section = new List<SelectListItem>();
            return View(importModel);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Index(DataImportModel dataImportModel)
        {
            if(!IsFileValid(dataImportModel))
            {
                ModelState.AddModelError("UploadFile", "Por favor seleccione un archivo de Excel");
            }

            var academicGrade = _academicGradeRepository.Filter(x => x.AcademicYear.Id == dataImportModel.Year
            && x.Grade.Id == dataImportModel.Grade && x.Section.Equals(dataImportModel.Section)).FirstOrDefault();
            if (academicGrade == null)
                ModelState.AddModelError("Year", "No existe ese grado academico");
            else if(academicGrade.Students.Any())
                ModelState.AddModelError("Year", "Ya hay alumos en este grado, borrelos e ingreselos de nuevo.");
            if (!ModelState.IsValid)
            {
                var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
                var isDirector = ViewBag.IsDirector = user.Role.Name.Equals("Director");
                ViewBag.GradeId = isDirector
                    ? new SelectList(_gradeRepository.Filter(x => x.EducationLevel.Director != null && x.EducationLevel.Director.Id == user.Id).ToList(), "Id", "Name", 0)
                    : new SelectList(_gradeRepository.GetAllGrade(), "Id", "Name", 0);
                ViewBag.Year = new SelectList(_academicYearRepository.Filter(x => x.EnrollsOpen), "Id", "Year");
                ViewBag.Section = new List<SelectListItem>();
                return View(dataImportModel);
            }
            var myDataSet = _dataImportService.GetDataSetFromExcelFile(dataImportModel.UploadFile);
            try
            {
                _dataImportService.Import(myDataSet, academicGrade);
            }
            catch(Exception ex)
            {
                _viewMessageLogic.SetNewMessage("Error!", ex.Message, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            
            const string title = "Importación de Datos Exitosa";
            var content = string.Format("Se importaron datos para el año: {0}, grado: {1} y sección: {2}"
                                        , academicGrade.AcademicYear.Year // 0
                                        , academicGrade.Grade.Name // 1
                                        , dataImportModel.Section // 2
                                       );
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [AuthorizeAdminDirector]
        private static bool IsFileValid(DataImportModel dataImportModel)
        {
            if (dataImportModel.UploadFile == null || dataImportModel.UploadFile.ContentLength <= 0)
            {
                return false;
            }
            var extension = Path.GetExtension(dataImportModel.UploadFile.FileName);
            return extension != null && Regex.IsMatch(extension, "^*.xls$|^*.xlsx$$");
        }

        [AuthorizeAdminDirector]
        public JsonResult LoadByGrade(DataImportModel dataImportModel)
        {
            if (dataImportModel.Year == 0)
            {
                var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == dataImportModel.Grade).ToList();
                var toReturn =
                    new SelectList(
                        sList, "Section", "Section");
                return Json(toReturn, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == dataImportModel.Grade && x.AcademicYear.Id == dataImportModel.Year).ToList();
                var toReturn =
                    new SelectList(
                        sList, "Section", "Section");
                return Json(toReturn, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
