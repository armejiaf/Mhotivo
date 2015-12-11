using System;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.App_Data;
using Mhotivo.Authorizations;

namespace Mhotivo.Controllers
{
    public class HomeworkController : Controller
    {

        private readonly IAcademicCourseRepository _academicCourseRepository;
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        public long TeacherId = -1;

        public HomeworkController(IHomeworkRepository homeworkRepository,
            IAcademicCourseRepository academicCourseRepository, ICourseRepository courseRepository, ITeacherRepository teacherRepository)
        {
            _homeworkRepository = homeworkRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
            _courseRepository = courseRepository;
            _academicCourseRepository = academicCourseRepository;
            _teacherRepository = teacherRepository;
        }

        [AuthorizeTeacher]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            TeacherId = GetTeacherId();
            var allAcademicYearsDetails = GetAllAcademicYearsDetail(TeacherId);
            var academicYearsDetails = allAcademicYearsDetails as AcademicCourse[] ?? allAcademicYearsDetails.ToArray();
            var academicY = academicYearsDetails.Select((t, a) => academicYearsDetails.ElementAt(a).Id).ToList();
            IEnumerable<Homework> allHomeworks = _homeworkRepository.GetAllHomeworks().Where(x => academicY.Contains(x.AcademicCourse.Id));
            IEnumerable<HomeworkDisplayModel> allHomeworkDisplaysModel =
                allHomeworks.Select(Mapper.Map<Homework, HomeworkDisplayModel>).ToList();
            return View(allHomeworkDisplaysModel);
        }

        private long GetTeacherId()
        {
            var idUser = (long)System.Web.HttpContext.Current.Session["loggedUserId"];
            var firstOrDefault = _teacherRepository.Filter(x => x.User.Id == idUser).FirstOrDefault();
            if (firstOrDefault == null) return -1;
            var toReturn = firstOrDefault.Id;
            return toReturn;
        }

        // GET: /Homework/Create
        [AuthorizeTeacher]
        public ActionResult Create()
        {
            var teacherId = GetTeacherId();
            var detalleAnhosAcademicosActivos = _academicCourseRepository.GetAllAcademicYearDetails().ToList().FindAll(x => x.AcademicGrade.AcademicYear.IsActive);
            var detallesFilteredByTeacher = detalleAnhosAcademicosActivos.FindAll(x => x.Teacher != null && x.Teacher.Id == teacherId);
            var query = detallesFilteredByTeacher.Select(detail => detail.Course).ToList();
            ViewBag.course = new SelectList(query, "Id", "Name");
            ViewBag.Years = DateTimeController.GetYears();
            ViewBag.Months = DateTimeController.GetMonths();
            ViewBag.Days = DateTimeController.GetDaysForMonthAndYearStatic(1, DateTime.Now.Year);
            var modelRegister = new HomeworkRegisterModel();
            return View(modelRegister);
        }

        private IEnumerable<AcademicCourse> GetAllAcademicYearsDetail(long id)
        {
            IEnumerable<AcademicCourse> allAcademicYearsDetail =
                _academicCourseRepository.GetAllAcademicYearDetails().Where(x => x.Teacher != null && x.Teacher.Id.Equals(id));
            return allAcademicYearsDetail;
        }

        [HttpPost]
        [AuthorizeTeacher]
        public ActionResult Create(HomeworkRegisterModel registerModelHomework)
        {
            var toCreate = Mapper.Map<Homework>(registerModelHomework);
            _homeworkRepository.Create(toCreate);
            const string title = "Tarea agregada";
            string content = "La tarea " + toCreate.Title + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        // GET: /Homework/Edit/5
        [AuthorizeTeacher]
        public ActionResult Edit(long id)
        {
            Homework thisHomework = _homeworkRepository.GetById(id);
            var homework = Mapper.Map<HomeworkEditModel>(thisHomework);
            var teacherId = GetTeacherId();
            var detalleAnhosAcademicosActivos = _academicCourseRepository.GetAllAcademicYearDetails().ToList().FindAll(x => x.AcademicGrade.AcademicYear.IsActive);
            var detallesFilteredByTeacher = detalleAnhosAcademicosActivos.FindAll(x => x.Teacher != null && x.Teacher.Id == teacherId);
            var query = detallesFilteredByTeacher.Select(detail => detail.Course).ToList();
            ViewBag.course = new SelectList(query, "Id", "Name");
            ViewBag.Years = DateTimeController.GetYears();
            ViewBag.Months = DateTimeController.GetMonths();
            ViewBag.Days = DateTimeController.GetDaysForMonthAndYearStatic(1, DateTime.Now.Year);
            return View("Edit", homework);
        }

        // POST: /Homework/Edit/5
        [HttpPost]
        [AuthorizeTeacher]
        public ActionResult Edit(HomeworkEditModel modelHomework)
        {
            Homework myStudent = _homeworkRepository.GetById(modelHomework.Id);
            Mapper.Map(modelHomework, myStudent);
            _homeworkRepository.Update(myStudent);
            const string title = "Tarea Actualizada";
            var content = "La tarea " + modelHomework.Title + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        // GET: /Homework/Delete/5
        [AuthorizeTeacher]
        public ActionResult Delete(long id)
        {
            Homework homework = _homeworkRepository.Delete(id);
            const string title = "Tarea Eliminado";
            string content = "La tarea: " + homework.Title + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        // POST: /Homework/Delete/5
        [HttpPost]
        [AuthorizeTeacher]
        public ActionResult Delete(long id, FormCollection collection)
        {
            var homework = _homeworkRepository.Delete(id);
            const string title = "Tarea Eliminada";
            string content = "La tarea: " + homework.Title + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }
    }
}