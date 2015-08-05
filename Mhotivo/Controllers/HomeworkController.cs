using System;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mhotivo.Controllers
{
    public class HomeworkController : Controller
    {
        //Loads of unused repositories. Remove them if not needed.
        private readonly IAcademicYearDetailsRepository _academicYearDetailRepository;
        private readonly ISessionManagementRepository _sessionManagementRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISecurityRepository _securityRepository;
        private readonly IUserRepository _userRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        public long MeisterId = -1;

        public HomeworkController(IHomeworkRepository homeworkRepository,
            IAcademicYearDetailsRepository academicYearDetailRepository, IAcademicYearRepository academicYearRepository,
            IGradeRepository gradeRepository, ICourseRepository courseRepository, ISessionManagementRepository sessionManagementRepository,
            ISecurityRepository securityRepository, IUserRepository userRepository)
        {
            _homeworkRepository = homeworkRepository;
            _academicYearRepository = academicYearRepository;
            _gradeRepository = gradeRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
            _courseRepository = courseRepository;
            _academicYearDetailRepository = academicYearDetailRepository;
            _sessionManagementRepository = sessionManagementRepository;
            _securityRepository = securityRepository;
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            MeisterId = GetMeisterId();
            var allAcademicYearsDetails = GetAllAcademicYearsDetail(MeisterId);
            var academicY = new List<long>();
            var academicYearsDetails = allAcademicYearsDetails as AcademicYearDetail[] ?? allAcademicYearsDetails.ToArray();
            for (int a = 0; a < academicYearsDetails.Count(); a++)
            {
                academicY.Add(academicYearsDetails.ElementAt(a).Id);
            }
            IEnumerable<Homework> allHomeworks = _homeworkRepository.GetAllHomeworks().Where(x => academicY.Contains(x.AcademicYearDetail.Id));
            Mapper.CreateMap<DisplayHomeworkModel, Homework>().ReverseMap();
            IEnumerable<DisplayHomeworkModel> allHomeworkDisplaysModel =
                allHomeworks.Select(Mapper.Map<Homework, DisplayHomeworkModel>).ToList();
            return View(allHomeworkDisplaysModel);
        }

        private long GetMeisterId()
        {
            var people = _securityRepository.GetUserLoggedPeoples();
            long id = 0;
            foreach (var p in people)
            {
                if (p is Teacher)
                    id = p.Id;
            }
            return id;
        }

        // GET: /Homework/Create
        public ActionResult Create()
        {
            MeisterId = GetMeisterId();
            var allAcademicYearsByMeister = GetAllAcademicYearsDetail(MeisterId);
            var courseIds = new List<long>();
            var academicYearsByMeister = allAcademicYearsByMeister as AcademicYearDetail[] ?? allAcademicYearsByMeister.ToArray();
            for (int a = 0; a < academicYearsByMeister.Count(); a++)
            {
                courseIds.Add(academicYearsByMeister.ElementAt(a).Course.Id);
            }
            var query = _courseRepository.Query(x => x).Where(x => courseIds.Contains(x.Id) );
            ViewBag.course = new SelectList(query, "Id", "Name");
            var modelRegister = new CreateHomeworkModel();
            return View(modelRegister);
        }

        private IEnumerable<AcademicYearDetail> GetAllAcademicYearsDetail(long id)
        {
            IEnumerable<AcademicYearDetail> allAcademicYearsDetail =
                _academicYearDetailRepository.GetAllAcademicYearDetails().Where(x => x.Teacher.Id.Equals(id));
            return allAcademicYearsDetail;
        }

        [HttpPost]
        public ActionResult Create(CreateHomeworkModel modelHomework)
        {
            var myHomework = new Homework
            {
                Title = modelHomework.Title,
                Description = modelHomework.Description,
                DeliverDate = DateTime.Parse(modelHomework.DeliverDate),
                Points = modelHomework.Points,
                AcademicYearDetail = _academicYearDetailRepository.FindByCourse(_courseRepository.GetById(modelHomework.Course).Id,GetMeisterId())
            };
            _homeworkRepository.Create(myHomework);
            const string title = "Tarea agregada";
            string content = "La tarea " + myHomework.Title + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        // GET: /Homework/Edit/5
        public ActionResult Edit(int id)
        {
            Homework thisHomework = _homeworkRepository.GetById(id);
            var homework = Mapper.Map<CreateHomeworkModel>(thisHomework);
            ViewBag.CourseId = new SelectList(_courseRepository.Query(x => x), "Id", "Name");
            return View("Edit", homework);
        }

        // POST: /Homework/Edit/5
        [HttpPost]
        public ActionResult Edit(EditHomeworkModel modelHomework)
        {
            Homework myStudent = _homeworkRepository.GetById(modelHomework.Id);
            Mapper.CreateMap<Homework, EditHomeworkModel>().ReverseMap();
            var homeworktModel = Mapper.Map<EditHomeworkModel, Homework>(modelHomework);
            _homeworkRepository.UpdateHomeworkFromHomeworkEditModel(homeworktModel, myStudent);
            const string title = "Tarea Actualizada";
            var content = "La tarea " + modelHomework.Title + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        // GET: /Homework/Delete/5
        public ActionResult Delete(int id)
        {
            Homework homework = _homeworkRepository.Delete(id);
            const string title = "Tarea Eliminado";
            string content = "La tarea: " + homework.Title + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        // POST: /Homework/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var homework = _homeworkRepository.Delete(id);
            const string title = "Tarea Eliminada";
            string content = "La tarea: " + homework.Title + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }
    }
}