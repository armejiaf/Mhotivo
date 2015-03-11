using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class CourseController : Controller
    {
        #region private members

        private readonly ICourseRepository _courseRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public CourseController(ICourseRepository courseRepository, 
                                IAreaRepository areaRepository)
        {
            if (courseRepository == null) throw new ArgumentNullException("courseRepository");
            if (areaRepository == null) throw new ArgumentNullException("areaRepository");

            _courseRepository = courseRepository;
            _areaRepository = areaRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        #endregion

        #region public methods

        /// <summary>
        /// GET: /Course/
        /// </summary>
        /// <returns />
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var listCourses = _courseRepository.GetAllCourse();
            
            Mapper.CreateMap<DisplayCourseModel, Course>().ReverseMap();
            
            var list = listCourses.Select(item => item.Area != null ? new DisplayCourseModel
            {
                Id = item.Id,
                Name = item.Name,
                Area = item.Area
            } : null).ToList();

            return View(list);
        }

        /// <summary>
        /// GET: /Course/Create
        /// </summary>
        /// <returns />
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.AreaId = new SelectList(_areaRepository.Query(a => a), "Id", "Name", 0);

            return View("Create");
        }

        /// <summary>
        /// POST: /Course/Create
        /// </summary>
        /// <param name="modelCourse"></param>
        /// <returns />
        [HttpPost]
        public ActionResult Add(CourseRegisterModel modelCourse)
        {
            string title;
            string content;

            Mapper.CreateMap<Course, CourseRegisterModel>().ReverseMap();
            var courseModel = Mapper.Map<CourseRegisterModel, Course>(modelCourse);
            courseModel.Area = _areaRepository.GetById(modelCourse.AreaId);

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

        /// <summary>
        /// POST: /Course/Delete
        /// </summary>
        /// <param name="id" />
        /// <returns />
        [HttpPost]
        public ActionResult Delete(long id)
        {
            var course = _courseRepository.Delete(id);

            const string title = "Materia Eliminada";
            var content = course.Name + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// GET: /Course/Edit
        /// </summary>
        /// <param name="id" />
        /// <returns />
        [HttpGet]
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

        /// <summary>
        /// POST: /Course/Edit/5
        /// </summary>
        /// <param name="modelCourse" />
        /// <returns />
        [HttpPost]
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

        #endregion
    }
}