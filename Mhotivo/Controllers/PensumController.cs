using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
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
                temp = _pensumRepository.Filter(x => x.Course.Name.Contains(searchString)).ToList();
            }

            Mapper.CreateMap<DisplayPensumModel, Pensum>().ReverseMap();
            var listapensumDisplaysModel = temp.Select(Mapper.Map<Pensum, DisplayPensumModel>).ToList();
            var list = temp.Select(item => item.Course != null ? new DisplayPensumModel
            {
                Id = item.Id,
                Course = item.Course.Name,
                Grade = item.Grade.Name
            } : null).ToList();

            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "course_desc":
                    list = list.OrderByDescending(s => s.Course).ToList();
                    break;
                case "Grade":
                    list = list.OrderBy(s => s.Grade).ToList();
                    break;
                case "grade_desc":
                    list = list.OrderByDescending(s => s.Grade).ToList();
                    break;
                default:  // Name ascending 
                    list = list.OrderBy(s => s.Course).ToList();
                    break;
            }

            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Edit(long id)
        {
            Pensum thisPensum = _pensumRepository.GetById(id);
            var pensum = new PensumEditModel
            {
                IdCourse = thisPensum.Course.Id,
                Id = thisPensum.Id,
                IdGrade = thisPensum.Grade.Id
            };

            ViewBag.IdCourse = new SelectList(_courseRepository.Query(x => x), "Id", "Name", thisPensum.Course.Id);
            ViewBag.IdGrade = new SelectList(_gradeRepository.Query(x => x), "Id", "Name", thisPensum.Grade.Id);

            return View("Edit", pensum);
        }


        [HttpPost]
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

            if (myPensum.Course.Id != modelPensum.IdCourse)
            {
                myPensum.Course = _courseRepository.GetById(modelPensum.IdCourse);
                updateCourse = true;
            }

            Pensum pensum = _pensumRepository.Update(myPensum, updateCourse,updateGrade);
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
        public ActionResult Delete(long id)
        {
            Pensum pensum = _pensumRepository.Delete(id);

            const string title = "Pensum Eliminado";
            string content = "El Pesum " + pensum.Id + " ha sido eliminado exitosamente.";
            TempData["MessageInfo"] = new MessageModel
            {
                Type = "INFO",
                Title = title,
                Content = content
            };

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.IdGrade = new SelectList(_gradeRepository.Query(x => x), "Id", "Name");
            ViewBag.IdCourse = new SelectList(_courseRepository.Query(x => x), "Id", "Name");
            return View("Create");
        }


        [HttpPost]
        public ActionResult Add(PensumRegisterModel modelPensum)
        {
            var myPensum = new Pensum
            {
                Grade = _gradeRepository.GetById(modelPensum.IdGrade),
                Course = _courseRepository.GetById(modelPensum.IdCourse)
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