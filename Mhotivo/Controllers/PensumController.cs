using System;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Authorizations;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Models;
using PagedList;

namespace Mhotivo.Controllers
{
    public class PensumController : Controller
    {

        private readonly IPensumRepository _pensumRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ICourseRepository _courseRepository;

        public PensumController(IPensumRepository pensumRepository, IGradeRepository gradeRepository, ICourseRepository courseRepository)
        {
            _pensumRepository = pensumRepository;
            _gradeRepository = gradeRepository;
            _courseRepository = courseRepository;
        }

         [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var message = (MessageModel)TempData["MessageInfo"];
            if (message != null)
            {
                ViewBag.MessageType = message.Type;
                ViewBag.MessageTitle = message.Title;
                ViewBag.MessageContent = message.Content;
            }
            var temp = _pensumRepository.GetAllPesums();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CourseSortParm = String.IsNullOrEmpty(sortOrder) ? "course_desc" : "";
            ViewBag.GradeSortParm = sortOrder == "Grade" ? "grade_desc" : "Grade";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                //temp = _pensumRepository.Filter(x => x.Course.Name.Contains(searchString)).ToList();
            }
            var list = temp.Select(item => item.Courses != null ? new DisplayPensumModel
            {
                Id = item.Id,
                Courses = item.Courses.Select(c => c.Name).ToList(),
                Grade = item.Grade.Name
            } : null).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "course_desc":
                    list = list.OrderByDescending(s => s.Courses).ToList();
                    break;
                case "Grade":
                    list = list.OrderBy(s => s.Grade).ToList();
                    break;
                case "grade_desc":
                    list = list.OrderByDescending(s => s.Grade).ToList();
                    break;
                default:  // Name ascending 
                    list = list.OrderBy(s => s.Courses).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            Pensum thisPensum = _pensumRepository.GetById(id);
            var pensum = new PensumEditModel
            {
                Id = thisPensum.Id,
                IdGrade = thisPensum.Grade.Id
            };
            foreach (var course in thisPensum.Courses)
            {
                pensum.Courses.Add(course.Name);
            }
            ViewBag.IdCourse = new SelectList(_courseRepository.Filter(x => x.Pensum == thisPensum), "Id", "Name", thisPensum.Courses);
            ViewBag.IdGrade = new SelectList(_gradeRepository.Query(x => x), "Id", "Name", thisPensum.Grade.Id);
            return View("Edit", pensum);
        }


        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(PensumEditModel modelPensum)
        {
            bool updateCourse = false;
            bool updateGrade = false;
            Pensum myPensum = _pensumRepository.GetById(modelPensum.Id);
            if (myPensum.Grade.Id != modelPensum.IdGrade)
            {
                myPensum.Grade = _gradeRepository.GetById(modelPensum.IdGrade);
                updateGrade = true;
            }
            //if (myPensum.Course.Id != modelPensum.IdCourse)
            //{
            //    myPensum.Course = _courseRepository.GetById(modelPensum.IdCourse);
            //    updateCourse = true;
            //}
            Pensum pensum = _pensumRepository.Update(myPensum);
            const string title = "Pensum Actualizado";
            string content = "El Pensum " + pensum.Id +
                             " ha sido actualizado exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                Type = "INFO",
                Title = title,
                Content = content
            };
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            Pensum pensum = _pensumRepository.Delete(id);
            const string title = "Pensum Eliminado";
            string content = "El Pesum para el grado " + pensum.Grade + " ha sido eliminado exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                Type = "INFO",
                Title = title,
                Content = content
            };
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            ViewBag.IdGrade = new SelectList(_gradeRepository.Query(x => x), "Id", "Name");
            ViewBag.IdCourse = new SelectList(_courseRepository.Query(x => x), "Id", "Name");
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(PensumRegisterModel modelPensum)
        {
            var myPensum = new Pensum
            {
                Grade = _gradeRepository.GetById(modelPensum.IdGrade),
                //Course = _courseRepository.GetById(modelPensum.IdCourse)
            };
            Pensum user = _pensumRepository.Create(myPensum);
            const string title = "Pensum Agregado";
            string content = "El pensum " + user.Id +  " ha sido agregado exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                Type = "SUCCESS",
                Title = title,
                Content = content
            };
            return RedirectToAction("Index");
        }
    }
}