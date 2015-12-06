using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Authorizations;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace Mhotivo.Controllers
{
    public class EnrollController : Controller
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IAcademicGradeRepository _academicGradeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISessionManagementService _sessionManagementService;

        public EnrollController(IStudentRepository studentRepository,
            IGradeRepository gradeRepository, IAcademicGradeRepository academicGradeRepository, IUserRepository userRepository, ISessionManagementService sessionManagementService)
        {
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
            _academicGradeRepository = academicGradeRepository;
            _userRepository = userRepository;
            _sessionManagementService = sessionManagementService;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdminDirector]
        [ActionName("GeneralEnrollsFromAcademicGrades")]
        public ActionResult Index(long gradeId, int? page)
        {
            ViewBag.GradeId = gradeId;
            _viewMessageLogic.SetViewMessageIfExist();
            var grade = _academicGradeRepository.GetById(gradeId);
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return grade == null ? View("Index") : View("Index", grade.Students.Select(n => new EnrollDisplayModel
            {
                AcademicGradeId = grade.Id,
                StudentId = n.Id,
                FullName = n.FullName,
                Photo = n.Photo,
                MyGender = n.MyGender.ToString("G"),
                AccountNumber = n.AccountNumber,
                Grade = grade.Grade.Name,
                Section = grade.Section
            }).ToPagedList(pageNumber, pageSize));
        }

        [AuthorizeAdminDirector]
        public ActionResult Index(int? page)
        {
            ViewBag.GradeId = -1;
            _viewMessageLogic.SetViewMessageIfExist();
            var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
            var isDirector = user.Role.Name.Equals("Director");
            var grades = isDirector
                ? _academicGradeRepository.Filter(
                    x =>
                        x.AcademicYear.IsActive && x.Grade.EducationLevel.Director != null &&
                        x.Grade.EducationLevel.Director.Id == user.Id).ToList()
                : _academicGradeRepository.Filter(x => x.AcademicYear.IsActive).ToList();
            if (!grades.Any())
                return View();
            var model = new List<EnrollDisplayModel>();
            foreach (var academicGrade in grades)
            {
                model.AddRange(academicGrade.Students.Select(n => new EnrollDisplayModel
                {
                    AcademicGradeId = academicGrade.Id,
                    StudentId = n.Id,
                    FullName = n.FullName,
                    Photo = n.Photo,
                    MyGender = n.MyGender.ToString("G"),
                    AccountNumber = n.AccountNumber,
                    Grade = academicGrade.Grade.Name,
                    Section = academicGrade.Section
                }));
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Search(string searchString, long gradeId, int? page)
        {
            if (searchString.IsNullOrWhiteSpace())
                return gradeId == -1 ? RedirectToAction("Index") : RedirectToAction("Index", new {gradeId});
            _viewMessageLogic.SetViewMessageIfExist();
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            if (gradeId == -1)
            {
                var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
                var isDirector = user.Role.Name.Equals("Director");
                var grades = isDirector
                    ? _academicGradeRepository.Filter(
                        x =>
                            x.AcademicYear.IsActive && x.Grade.EducationLevel.Director != null &&
                            x.Grade.EducationLevel.Director.Id == user.Id).ToList()
                    : _academicGradeRepository.Filter(x => x.AcademicYear.IsActive).ToList();
                if (!grades.Any())
                    return View("Index");
                var toReturn = new List<EnrollDisplayModel>();
                foreach (var academicGrade in grades)
                {
                    toReturn.AddRange(Mapper.Map<IEnumerable<EnrollDisplayModel>>(academicGrade));
                }
                
                return View("Index", toReturn.Where(x => x.FullName.Contains(searchString)).ToPagedList(pageNumber, pageSize));
            }
            var grade = _academicGradeRepository.GetById(gradeId);
            return grade == null ? View("Index") : View("Index", Mapper.Map<IEnumerable<EnrollDisplayModel>>(grade).Where(x => x.FullName.Contains(searchString)).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Delete(long id, long gradeId, long academicGradeId)
        {
            var grade = _academicGradeRepository.GetById(gradeId);
            grade.Students.ToList().RemoveAll(x => x.Id == id);
            var student = _studentRepository.GetById(id);
            student.MyGrade = null;
            _studentRepository.Update(student);
            _academicGradeRepository.Update(grade);
            const string title = "Matricula Borrada";
            const string content = "El estudiante ha sido eliminado exitosamente de la lista de matriculados.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return academicGradeId == -1 ? RedirectToAction("Index") : RedirectToAction("GeneralEnrollsFromAcademicGrades", new { gradeId = academicGradeId });
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        [ActionName("DeleteAllFromCurrentAcademicGrade")]
        public ActionResult DeleteAll(long gradeId)
        {
            var grade = _academicGradeRepository.GetById(gradeId);
            grade.Students.Clear();
            _academicGradeRepository.Update(grade);
            const string title = "Matricula Borrada";
            const string content = "Todos los estudiantes de esta seccion han sido eliminados exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("GeneralEnrollsFromAcademicGrades", new { gradeId });
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult DeleteAll(EnrollDeleteModel model)
        {
            var grade = _academicGradeRepository.GetById(model.AcademicGrade);
            if (grade == null)
            {
                const string title = "Error";
                const string content = "No  hay un año academico con ese grado y seccion";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            grade.Students.Clear();
            _academicGradeRepository.Update(grade);
            const string title2 = "Matricula Borrada";
            const string content2 = "Todos los estudiantes de ese grado y seccion han sido eliminados exitosamente.";
            _viewMessageLogic.SetNewMessage(title2, content2, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [AuthorizeAdminDirector]
        public ActionResult DeleteAll()
        {
            var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
            var isDirector = user.Role.Name.Equals("Director");
            var model = new EnrollDeleteModel();
            var grades = isDirector ? _gradeRepository.Filter(x => x.EducationLevel.Director != null && x.EducationLevel.Director.Id == user.Id).ToList() : _gradeRepository.GetAllGrade().ToList();
            ViewBag.Grades = new SelectList(grades, "Id", "Name");
            var firstGradeId = grades.First().Id;
            ViewBag.Sections = new List<SelectListItem>();
            ((List<SelectListItem>) ViewBag.Sections).AddRange(_academicGradeRepository.Filter(
                x => x.AcademicYear.IsActive && x.Grade.Id == firstGradeId)
                .Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Section }));
            return PartialView(model);
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Add(long gradeId)
        {
            var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
            var isDirector = user.Role.Name.Equals("Director");
            var availableStudents = _studentRepository.Filter(x => x.MyGrade == null);
            ViewBag.Id = new SelectList(availableStudents, "Id", "FullName");
            var grades = isDirector? _gradeRepository.Filter(x => x.EducationLevel.Director != null && x.EducationLevel.Director.Id == user.Id).ToList() : _gradeRepository.GetAllGrade().ToList();
            ViewBag.Grades = new SelectList(grades, "Id", "Name");
            var firstGradeId = grades.First().Id;
            ViewBag.Sections = new List<SelectListItem>();
            ((List<SelectListItem>)ViewBag.Sections).AddRange(_academicGradeRepository.Filter(
                x => x.AcademicYear.IsActive && x.Grade.Id == firstGradeId)
                .Select(n => new SelectListItem { Value = n.Id.ToString(), Text = n.Section }));
            return View("Create", new EnrollRegisterModel{Id = gradeId});
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Add(EnrollRegisterModel modelEnroll)
        {
            var student = _studentRepository.GetById(modelEnroll.Student);
            var academicGrade = _academicGradeRepository.GetById(modelEnroll.AcademicGrade);
            if (student.MyGrade != null)
            {
                const string title = "Estudiante No Agregado";
                const string content = "El estudiante no se logro matricular.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
            }
            else
            {
                academicGrade.Students.Add(student);
                student.MyGrade = academicGrade;
                _academicGradeRepository.Update(academicGrade);
                _studentRepository.Update(student);
                const string title = "Estudiante Agregado";
                const string content = "El estudiante ha sido matriculado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            }
            return (academicGrade.Id != -1) ? RedirectToAction("Index") : RedirectToAction("GeneralEnrollsFromAcademicGrades", new { gradeId = modelEnroll.Id });
        }
    }
}