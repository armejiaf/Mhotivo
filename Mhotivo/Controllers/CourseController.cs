using System;
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
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IEducationLevelRepository _areaRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public CourseController(ICourseRepository courseRepository, 
                                IEducationLevelRepository areaRepository)
        {
            if (courseRepository == null) throw new ArgumentNullException("courseRepository");
            if (areaRepository == null) throw new ArgumentNullException("areaRepository");
            _courseRepository = courseRepository;
            _areaRepository = areaRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        /// GET: /Course/
         [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var listCourses = _courseRepository.GetAllCourse();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "course_desc" : "";
            ViewBag.AreaSortParm = sortOrder == "Area" ? "area_desc" : "Area";
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
                listCourses = _courseRepository.Filter(x => x.Name.Contains(searchString)).ToList();
            }
            Mapper.CreateMap<DisplayCourseModel, Course>().ReverseMap();
            var list = listCourses.Select(item => item.Area != null ? new DisplayCourseModel
            {
                Id = item.Id,
                Name = item.Name,
                Area = item.Area
            } : null).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "course_desc":
                    list = list.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Area":
                    list = list.OrderBy(s => s.Area.Name).ToList();
                    break;
                case "area_desc":
                    list = list.OrderByDescending(s => s.Area.Name).ToList();
                    break;
                default:  // Name ascending 
                    list = list.OrderBy(s => s.Name).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber,pageSize));
        }

        /// GET: /Course/Create
        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            ViewBag.AreaId = new SelectList(_areaRepository.Query(a => a), "Id", "Name", 0);
            return View("Create");
        }

        /// POST: /Course/Create
        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(CourseRegisterModel modelCourse)
        {
            string title;
            string content;
            Mapper.CreateMap<Course, CourseRegisterModel>().ReverseMap();
            var courseModel = Mapper.Map<CourseRegisterModel, Course>(modelCourse);
            courseModel.Area = _areaRepository.GetById(modelCourse.Area);
            var myCourse = _courseRepository.GenerateCourseFromRegisterModel(courseModel);
            var existCourse =
                _courseRepository.GetAllCourse()
                    .FirstOrDefault(c => c.Name.Equals(modelCourse.Name));
            if (existCourse != null)
            {
                title = "Materia";
                content = "La materia " + existCourse.Name + " ya existe.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                return RedirectToAction("Index");
            }
            var newCourse = _courseRepository.Create(myCourse);
            title = "Materia Agregada";
            content = "La materia " + newCourse.Name + " ha sido agregada exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        /// POST: /Course/Delete
        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            var course = _courseRepository.Delete(id);
            const string title = "Materia Eliminada";
            var content = course.Name + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }
        
        /// GET: /Course/Edit
        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(int id)
        {
            var course = _courseRepository.GetCourseEditModelById(id);
            Mapper.CreateMap<CourseEditModel, Course>().ReverseMap();
            var editCourse = new CourseEditModel
            {
                Id = course.Id,
                Name = course.Name,
                Area = course.Area
            };
            ViewBag.AreaId = new SelectList(_areaRepository.Query(a => a), "Id", "Name",
               editCourse.Area);
            return View("Edit", editCourse);
        }

        /// POST: /Course/Edit/5
        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(CourseEditModel modelCourse)
        {
            var course = _courseRepository.GetById(modelCourse.Id);
            Mapper.CreateMap<Course, CourseEditModel>().ReverseMap();
            var courseModel = Mapper.Map<CourseEditModel, Course>(modelCourse);
            _courseRepository.UpdateCourseFromCourseEditModel(courseModel, course);
            const string title = "Materia Actualizada";
            var content = course.Name + " ha sido modificado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }
    }
}