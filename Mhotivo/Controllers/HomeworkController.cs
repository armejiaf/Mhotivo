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

        private readonly IAcademicYearCourseRepository _academicYearCourseRepository;
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        public long TeacherId = -1;

        public HomeworkController(IHomeworkRepository homeworkRepository,
            IAcademicYearCourseRepository academicYearCourseRepository, ICourseRepository courseRepository, ITeacherRepository teacherRepository)
        {
            _homeworkRepository = homeworkRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
            _courseRepository = courseRepository;
            _academicYearCourseRepository = academicYearCourseRepository;
            _teacherRepository = teacherRepository;
        }

        [AuthorizeTeacher]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            TeacherId = GetTeacherId();
            var allAcademicYearsDetails = GetAllAcademicYearsDetail(TeacherId);
            var academicY = new List<long>();
            var academicYearsDetails = allAcademicYearsDetails as AcademicYearCourse[] ?? allAcademicYearsDetails.ToArray();
            for (int a = 0; a < academicYearsDetails.Count(); a++)
            {
                academicY.Add(academicYearsDetails.ElementAt(a).Id);
            }
            IEnumerable<Homework> allHomeworks = _homeworkRepository.GetAllHomeworks().Where(x => academicY.Contains(x.AcademicYearCourse.Id));
            IEnumerable<DisplayHomeworkModel> allHomeworkDisplaysModel =
                allHomeworks.Select(Mapper.Map<Homework, DisplayHomeworkModel>).ToList();
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
            var detalleAnhosAcademicosActivos = _academicYearCourseRepository.GetAllAcademicYearDetails().ToList().FindAll(x => x.AcademicYearGrade.IsActive);
            var detallesFilteredByTeacher = detalleAnhosAcademicosActivos.FindAll(x => x.Teacher.Id == teacherId);
            var query = detallesFilteredByTeacher.Select(detail => detail.Course).ToList();
            ViewBag.course = new SelectList(query, "Id", "Name");
            var modelRegister = new CreateHomeworkModel();
            return View(modelRegister);
        }

        private IEnumerable<AcademicYearCourse> GetAllAcademicYearsDetail(long id)
        {
            IEnumerable<AcademicYearCourse> allAcademicYearsDetail =
                _academicYearCourseRepository.GetAllAcademicYearDetails().Where(x => x.Teacher.Id.Equals(id));
            return allAcademicYearsDetail;
        }

        [HttpPost]
        [AuthorizeTeacher]
        public ActionResult Create(CreateHomeworkModel modelHomework)
        {
            
            var myHomework = new Homework
            {
                Title = modelHomework.Title,
                Description = modelHomework.Description,
                DeliverDate = ParseToHonduranDateTime.Parse(modelHomework.DeliverDate),
                Points = modelHomework.Points,
                AcademicYearCourse = _academicYearCourseRepository.FindByCourse(_courseRepository.GetById(modelHomework.Course).Id,GetTeacherId())
            };

            _homeworkRepository.Create(myHomework);
            const string title = "Tarea agregada";
            string content = "La tarea " + myHomework.Title + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        // GET: /Homework/Edit/5
        [AuthorizeTeacher]
        public ActionResult Edit(long id)
        {
            Homework thisHomework = _homeworkRepository.GetById(id);
            var homework = Mapper.Map<CreateHomeworkModel>(thisHomework);
            ViewBag.CourseId = new SelectList(_courseRepository.Query(x => x), "Id", "Name");
            return View("Edit", homework);
        }

        // POST: /Homework/Edit/5
        [HttpPost]
        [AuthorizeTeacher]
        public ActionResult Edit(EditHomeworkModel modelHomework)
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