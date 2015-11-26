using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Authorizations;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using PagedList;

namespace Mhotivo.Controllers
{
    public class AcademicGradeController : Controller
    {
        private readonly IAcademicGradeRepository _academicGradeRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IPensumRepository _pensumRepository;
        private readonly IAcademicCourseRepository _academicCourseRepository;
        private readonly ICourseRepository _courseRepository;

        public AcademicGradeController(IAcademicGradeRepository academicGradeRepository, IAcademicYearRepository academicYearRepository, ITeacherRepository teacherRepository, IGradeRepository gradeRepository, IPensumRepository pensumRepository, IAcademicCourseRepository academicCourseRepository, ICourseRepository courseRepository)
        {
            _academicGradeRepository = academicGradeRepository;
            _academicYearRepository = academicYearRepository;
            _teacherRepository = teacherRepository;
            _gradeRepository = gradeRepository;
            _pensumRepository = pensumRepository;
            _academicCourseRepository = academicCourseRepository;
            _courseRepository = courseRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdminDirector]
        public ActionResult Index(long yearId, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var grades = _academicGradeRepository.Filter(x => x.AcademicYear.Id == yearId).ToList();
            ViewBag.IdAcademicYear = yearId;
            ViewBag.Year = _academicYearRepository.GetById(yearId).Year;
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    grades = _academicGradeRepository.Filter(x => x.AcademicYear.Id == yearId && (x.Section.Equals(searchString) || x.Grade.Name.Contains(searchString))).ToList();
                }
                catch (Exception)
                {
                    grades = _academicGradeRepository.Filter(x => x.AcademicYear.Id == yearId).ToList();
                }
            }
            ViewBag.CurrentFilter = searchString;
            var model = grades.Select(Mapper.Map<AcademicGradeDisplayModel>);
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [AuthorizeAdminDirector]
        public ActionResult Add(long yearId)
        {
            ViewBag.Grades = new SelectList(_gradeRepository.GetAllGrade(), "Id", "Name");
            ViewBag.Pensums = new List<SelectListItem>();
            return View("Create",new AcademicGradeRegisterModel {AcademicYear = yearId});
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Add(AcademicGradeRegisterModel model)
        {
            string title;
            string content;
            var toCreate = Mapper.Map<AcademicGrade>(model);
            var toCheck = _academicGradeRepository.Filter(x => x.Grade.Id == model.Grade && x.Section.Equals(model.Section));
            if (toCheck.Any())
            {
                title = "Error!";
                content = "El Grado Academico ya existe.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index", new { yearId = model.AcademicYear });
            }
            toCreate = _academicGradeRepository.Create(toCreate);
            foreach (var course in toCreate.ActivePensum.Courses)
            {
                var academicCourse = new AcademicCourse
                {
                    AcademicGrade = toCreate,
                    Course = course
                };
                _academicCourseRepository.Create(academicCourse);
            }
            title = "Grado Academico Agregado";
            content = "El Grado Academico " + toCreate.Grade.Name + " " +toCreate.Section + " ha sido guardado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", new { yearId = model.AcademicYear });
        }

        [AuthorizeAdminDirector]
        public ActionResult EditTeacher(long id)
        {
            var item = _academicGradeRepository.GetById(id);
            ViewBag.SectionTeacher = new SelectList(_teacherRepository.GetAllTeachers(), "Id", "FullName", item.SectionTeacher);
            var toReturn = Mapper.Map<AcademicGradeTeacherAssignModel>(item);
            return View(toReturn);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult EditTeacher(AcademicGradeTeacherAssignModel model)
        {
            var grade = _academicGradeRepository.GetById(model.Id);
            grade = Mapper.Map(model, grade);
            grade = _academicGradeRepository.Update(grade);
            const string title = "Maestro de Seccion Asignado";
            var content = "Se ha asignado el maestro de seccion de " + grade.Grade.Name + " " + grade.Section + ".";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", new{yearId = grade.AcademicYear.Id});
        }

        [AuthorizeAdminDirector]
        public ActionResult Edit(long id)
        {
            var item = _academicGradeRepository.GetById(id);
            var toReturn = Mapper.Map<AcademicGradeEditModel>(item);
            ViewBag.Grades = new SelectList(_gradeRepository.GetAllGrade(), "Id", "Name", item.Grade);
            ViewBag.Pensums = new SelectList(_pensumRepository.Filter(x => x.Grade.Id == item.Grade.Id), "Id", "Name", item.ActivePensum).ToList();
            return View(toReturn);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Edit(AcademicGradeEditModel model)
        {
            var item = _academicGradeRepository.GetById(model.Id);
            var list = _academicGradeRepository.Filter(x => x.Grade.Id == model.Grade && x.Section == model.Section &&  x.Id == model.Id);
            var list2 = _academicGradeRepository.Filter(x => x.Grade.Id == model.Grade && x.Section == model.Section && x.Id != model.Id);
            string title;
            string content;
            if (list2.Any())
            {
                title = "Error!";
                content = "Ese grado academico ya existe.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
            }
            else if (!list.Any())
            {
                var pensumId = item.ActivePensum.Id;
                var newPensumId = model.ActivePensum;
                if (pensumId != newPensumId)
                {
                    var courses = _academicCourseRepository.Filter(x => x.AcademicGrade.Id == model.Id).ToList();
                    foreach (var academicCourse in courses)
                    {
                        _academicCourseRepository.Delete(academicCourse);
                    }
                }
                item = Mapper.Map(model, item);
                item = _academicGradeRepository.Update(item);
                if (pensumId != newPensumId)
                {
                    foreach (var course in item.ActivePensum.Courses)
                    {
                        var academicCourse = new AcademicCourse
                        {
                            AcademicGrade = item,
                            Course = course
                        };
                        _academicCourseRepository.Create(academicCourse);
                    }
                }
                title = "Grado Academico Actualizado!";
                content = "El Grado Academico " + item.Grade.Name + " "+ item.Section + " fue actualizado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            }
            return RedirectToAction("Index", new { yearId = item.AcademicYear.Id });
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Delete(long id)
        {
            var item = _academicGradeRepository.GetById(id);
            var yearId = item.AcademicYear.Id;
            var gradeName = item.Grade.Name;
            var details = item.CoursesDetails.ToList();
            foreach (var academicCourse in details)
            {
                _academicCourseRepository.Delete(academicCourse);
            }
            item = _academicGradeRepository.Delete(item);
            const string title = "Grado Academico Eliminado!";
            var content = "El Grado Academico " + gradeName + " " + item.Section + " fue eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", new { yearId });
        }

        [AuthorizeAdminDirector]
        public JsonResult GetPensumsForGrade(AcademicGradeRegisterModel model)
        {
            var sList = _pensumRepository.Filter(
                    x => x.Grade.Id == model.Grade).ToList();
            var toReturn =
                new SelectList(
                    sList, "Id", "Name");
            return Json(toReturn, JsonRequestBehavior.AllowGet);
        }
    }
}
