using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using Mhotivo.Authorizations;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using PagedList;

namespace Mhotivo.Controllers
{
    public class AcademicCourseController : Controller
    {
        private readonly IAcademicCourseRepository _academicCourseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public AcademicCourseController(IAcademicCourseRepository academicCourseRepository,
            ICourseRepository courseRepository, ITeacherRepository teacherRepository)
        {
            _academicCourseRepository = academicCourseRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdmin]
        public ActionResult Index(long id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var allAcademicYears = _academicCourseRepository.Filter(x => x.AcademicGrade.Id == id).ToList();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "teacher_desc" : "";
            ViewBag.ScheduleSortParm = sortOrder == "Schedule" ? "schedule_desc" : "Schedule";
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
                allAcademicYears = _academicCourseRepository.Filter(x => x.Teacher.FullName.Contains(searchString)).ToList();
            }
            var academicYearsDetails = allAcademicYears.Select(Mapper.Map<AcademicCourseDisplayModel>);
            ViewBag.IdAcademicYear = id;
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "teacher_desc":
                    academicYearsDetails = academicYearsDetails.OrderByDescending(s => s.Teacher).ToList();
                    break;
                case "Schedule":
                    academicYearsDetails = academicYearsDetails.OrderBy(s => s.Schedule).ToList();
                    break;
                case "schedule_desc":
                    academicYearsDetails = academicYearsDetails.OrderByDescending(s => s.Schedule).ToList();
                    break;
                default:  // Name ascending 
                    academicYearsDetails = academicYearsDetails.OrderBy(s => s.Teacher).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(academicYearsDetails.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            var academicYearDetails = _academicCourseRepository.GetById(id);
            var academicYearModel = Mapper.Map<AcademicCourseEditModel>(academicYearDetails);
            ViewBag.TeacherId = new SelectList(_teacherRepository.Query(x => x), "Id", "FullName", academicYearModel.Teacher);
            return View("Edit", academicYearModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(AcademicCourseEditModel academicCourseModel)
        {
            var toEdit = _academicCourseRepository.GetById(academicCourseModel.Id);
            toEdit = Mapper.Map(academicCourseModel, toEdit);
            _academicCourseRepository.Update(toEdit);
            const string title = "Curso Académico Actualizado ";
            var content = "El Curso " + toEdit.Course.Name + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", new {id = toEdit.AcademicGrade.Id});
        }
    }
}