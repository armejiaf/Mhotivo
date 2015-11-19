using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Authorizations;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class EnrollController : Controller
    {
        private readonly IEnrollRepository _enrollRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IAcademicGradeRepository _academicGradeRepository;

        public EnrollController(IStudentRepository studentRepository, IEnrollRepository enrollRepository, 
            IGradeRepository gradeRepository, IAcademicGradeRepository academicGradeRepository)
        {
            _studentRepository = studentRepository;
            _enrollRepository = enrollRepository;
            _gradeRepository = gradeRepository;
            _academicGradeRepository = academicGradeRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

       [AuthorizeAdmin]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            return View(_enrollRepository.Filter(x => x.AcademicGrade.AcademicYear.IsActive).ToList()
                .Select(x => new EnrollDisplayModel
                {
                    Id = x.Id,
                    FullName = x.Student.FullName,
                    Photo = x.Student.Photo,
                    MyGender = x.Student.MyGender.ToString("G"),
                    AccountNumber = x.Student.AccountNumber,
                    Grade = x.AcademicGrade.Grade.Name,
                    Section = x.AcademicGrade.Section
                }));
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Search(string id)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            IEnumerable<EnrollDisplayModel> model = _enrollRepository.Filter(x => x.Student.FullName.Contains(id))
                .ToList()
                .Select(x => new EnrollDisplayModel
                {
                    Id = x.Id,
                    FullName = x.Student.FullName,
                    Photo = x.Student.Photo,
                    MyGender = x.Student.MyGender.ToString("G"),
                    AccountNumber = x.Student.AccountNumber,
                    Grade = x.AcademicGrade.Grade.Name,
                    Section = x.AcademicGrade.Section
                });
            return View("Index", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            _enrollRepository.Delete(id);
            const string title = "Matricula Borrada";
            const string content = "El estudiante ha sido eliminado exitosamente de la lista de matriculados.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult DeleteAll(EnrollDeleteModel model)
        {
            var year =
                _academicGradeRepository.Filter(x => x.Grade.Id == model.Grade && x.Section.Equals(model.AcademicGrade))
                    .FirstOrDefault();
            if (year == null)
            {
                const string title = "Error";
                const string content = "No  hay un año academico con ese grado y seccion";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var enrolls = _enrollRepository.Filter(x => x.AcademicGrade.AcademicYear.Id == year.Id).ToList();
            
            foreach (var enroll in enrolls)
            {
                _enrollRepository.Delete(enroll.Id);
            }
            const string title2 = "Matricula Borrada";
            const string content2 = "Todos los estudiantes de ese grado y seccion han sido eliminados exitosamente.";
            _viewMessageLogic.SetNewMessage(title2, content2, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [AuthorizeAdmin]
        public ActionResult DeleteAll()
        {
        var model = new EnrollDeleteModel();
        ViewBag.GradeId = new SelectList(_gradeRepository.Query(a => a), "Id", "Name",
        model.Grade);
            ViewBag.Section = new SelectList(new List<string> { "A", "B", "C" }, "A");
            return PartialView(model);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            var allStudents = _studentRepository.GetAllStudents().ToList();
            var availableStudents = (from student in allStudents 
                                     where !_enrollRepository.Filter(x => x.Student.Id == student.Id && x.AcademicGrade.AcademicYear.IsActive).Any() 
                                     select student).ToList();
            ViewBag.Id = new SelectList(availableStudents, "Id", "FullName");
            ViewBag.GradeId = new SelectList(_gradeRepository.Query(x => x), "Id", "Name");
            ViewBag.Section = new SelectList(new List<string> {"A", "B", "C"}, "A");
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(EnrollRegisterModel modelEnroll)
        {
            Student student = _studentRepository.GetById(modelEnroll.Student);
            List<AcademicGrade> collection =
                _academicGradeRepository.Filter(x => x.Grade.Id == modelEnroll.Grade && x.Section.Equals(modelEnroll.AcademicGrade)).ToList();
            if (collection.Count > 0 && student != null)
            {
                foreach (AcademicGrade academicYear in collection)
                {
                    var myEnroll = new Enroll
                    {
                        AcademicGrade = academicYear,
                        Student = student
                    };
                    _enrollRepository.Create(myEnroll);
                }
                const string title = "Estudiante Agregado";
                const string content = "El estudiante ha sido matriculado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            }
            else
            {
                const string title = "Estudiante No Agregado";
                const string content = "El estudiante no se logro matricular.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
            }
            return RedirectToAction("Index");
        }
    }
}