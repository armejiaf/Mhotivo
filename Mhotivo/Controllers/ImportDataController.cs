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
        private readonly IParentRepository _parentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IEnrollRepository _enrollRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public ImportDataController(IImportDataRepository importDataRepository
                                    ,IGradeRepository gradeRepository
                                    ,IAcademicYearRepository academicYearRepository
                                    ,IParentRepository parentRepository
                                    ,IStudentRepository studentRepository
                                    ,IEnrollRepository enrollRepository
                                    ,IUserRepository userRepository
                                    ,IRoleRepository roleRepository)
        {
            _importDataRepository = importDataRepository;
            _gradeRepository = gradeRepository;
            _academicYearRepository = academicYearRepository;
            _parentRepository = parentRepository;
            _studentRepository = studentRepository;
            _enrollRepository = enrollRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
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
            if (importModel.UpladFile != null && importModel.UpladFile.ContentLength > 0)
                errorExcel = !validImageTypes.Contains(importModel.UpladFile.ContentType);
            else
                errorExcel = true;
            if(errorExcel)
                ModelState.AddModelError("UpladFile", "Por favor seleccione un archivo de Excel");
            var academicYear = _academicYearRepository.GetByFields(importModel.Year, (int) importModel.GradeImport, importModel.Section);
            if (academicYear == null)
                ModelState.AddModelError("Year", "No existe ese año academico");
            ViewBag.GradeId = new SelectList(_gradeRepository.Query(x => x), "Id", "Name", 0);
            if (!ModelState.IsValid)
            {
                return View(importModel);
            }
            try
            {
                var path = "";
                if (importModel.UpladFile != null)
                {
                    //var extension = System.IO.Path.GetExtension(importModel.UpladFile.FileName);
                    path = string.Format("{0}\\{1}", Server.MapPath("~/Content/UploadedFolder"), importModel.UpladFile.FileName);
                    var directory = Server.MapPath("~/Content/UploadedFolder");
                    if (!System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    importModel.UpladFile.SaveAs(path);
                }

                var myDataSet = _importDataRepository.GetDataSetFromExcelFile(path);
                _importDataRepository.Import(myDataSet, academicYear, _parentRepository, _studentRepository, _enrollRepository, _academicYearRepository, _userRepository, _roleRepository);
                const string title = "Importación de Datos Correcta";
                var content = string.Format("Se importaron datos para el año: {0}, grado: {1} y sección: {2}"
                                            ,importModel.Year // 0
                                            ,importModel.GradeImport // 1
                                            ,importModel.Section // 2
                                           );
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(importModel);
            }
        }
    }
}
