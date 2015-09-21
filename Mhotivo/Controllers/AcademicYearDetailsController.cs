using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using Mhotivo.App_Data;
using Mhotivo.Authorizations;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using PagedList;

namespace Mhotivo.Controllers
{
    public class AcademicYearDetailsController : Controller
    {
        private readonly IAcademicYearDetailsRepository _academicYearDetailsRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IAcademicYearRepository _academicYearRepository;

        public AcademicYearDetailsController(IAcademicYearDetailsRepository academicYearDetailsRepository,
            ICourseRepository courseRepository, ITeacherRepository teacherRepository,
            IAcademicYearRepository academicYearRepository)
        {
            _academicYearDetailsRepository = academicYearDetailsRepository;
            _courseRepository = courseRepository;
            _teacherRepository = teacherRepository;
            _academicYearRepository = academicYearRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdmin]
        public ActionResult Index(int id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var allAcademicYears = _academicYearDetailsRepository.GetAllAcademicYearsDetails(id);
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
                allAcademicYears = _academicYearDetailsRepository.Filter(x => x.Teacher.FullName.Contains(searchString)).ToList();
            }
            var academicYearsDetails = allAcademicYears.Select(academicYearD =>  academicYearD.Schedule != null ? (academicYearD.TeacherEndDate != null ? (academicYearD.TeacherStartDate != null ? new DisplayAcademicYearDetailsModel
            {
                Id = academicYearD.Id,
                TeacherStartDate = academicYearD.TeacherStartDate.Value,
                TeacherEndDate = academicYearD.TeacherEndDate.Value,
                Schedule = academicYearD.Schedule.Value,
                Room = academicYearD.Room,
                Course = academicYearD.Course.Name,
                Teacher = academicYearD.Teacher.FullName
            } : null) : null) : null).ToList();
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
        public ActionResult Edit(int id)
        {
            var academicYearDetails = _academicYearDetailsRepository.GetById(id);
            var academicYearModel = new AcademicYearDetailsEditModel
            {
                Id = academicYearDetails.Id,
                TeacherStartDate = academicYearDetails.TeacherStartDate.ToString(),
                TeacherEndDate = academicYearDetails.TeacherEndDate.ToString(),
                Schedule = academicYearDetails.Schedule.ToString(),
                Room = academicYearDetails.Room,
                Course = academicYearDetails.Course,
                Teacher = academicYearDetails.Teacher
            };
            ViewBag.CourseId = new SelectList(_courseRepository.Query(x => x), "Id", "Name", academicYearModel.Course.Id);
            ViewBag.TeacherId = new SelectList(_teacherRepository.Query(x => x), "Id", "FullName", academicYearModel.Teacher.Id);
            return View("Edit", academicYearModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(AcademicYearDetailsEditModel academicYearDetailsModel)
        {
            var myAcademicYearDetails = _academicYearDetailsRepository.GetById(academicYearDetailsModel.Id);
            myAcademicYearDetails.TeacherStartDate = academicYearDetailsModel.TeacherStartDate.AsDateTime();
            myAcademicYearDetails.TeacherEndDate = academicYearDetailsModel.TeacherEndDate.AsDateTime();
            myAcademicYearDetails.Schedule = academicYearDetailsModel.Schedule.AsDateTime();
            myAcademicYearDetails.Room = academicYearDetailsModel.Room;
            myAcademicYearDetails.Course = _courseRepository.GetById(academicYearDetailsModel.Course.Id);
            myAcademicYearDetails.Teacher = _teacherRepository.GetById(academicYearDetailsModel.Teacher.Id);
            _academicYearDetailsRepository.Update(myAcademicYearDetails);
            const string title = "El Detalle del Año Académico Actualizado ";
            var content = "El detalle " + myAcademicYearDetails.Course.Name + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return Redirect(string.Format("~/AcademicYearDetails/Index/{0}", myAcademicYearDetails.AcademicYear.Id));
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(int id)
        {
            var academicYearDetail = _academicYearDetailsRepository.Delete(id);
            const string title = "Detalle Académico Eliminado";
             var content = "El detalle de año académico " + academicYearDetail.Course.Name + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return Redirect(string.Format("~/AcademicYearDetails/Index/{0}", academicYearDetail.AcademicYear.Id));
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add(long id)
        {
            ViewBag.CourseId = new SelectList(_courseRepository.Query(x => x), "Id", "Name", 0);
            ViewBag.TeacherId = new SelectList(_teacherRepository.Query(x => x), "Id", "FullName", 0);
            ViewBag.IdAcademicYear = id;
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(AcademicYearDetailsRegisterModel academicYearDetailsModel)
        {

            var academicYearDetails = new AcademicYearDetail
            {
                TeacherStartDate = ParseToHonduranDateTime.Parse(academicYearDetailsModel.TeacherStartDate),
                TeacherEndDate = ParseToHonduranDateTime.Parse(academicYearDetailsModel.TeacherEndDate),
                Schedule = ParseToHonduranDateTime.Parse(academicYearDetailsModel.Schedule),
                Room = academicYearDetailsModel.Room,
                Course = _courseRepository.GetById(academicYearDetailsModel.Course.Id),
                Teacher = _teacherRepository.GetById(academicYearDetailsModel.Teacher.Id),
                AcademicYear = _academicYearRepository.GetById(academicYearDetailsModel.AcademicYearId)
            };

           
            _academicYearDetailsRepository.Create(academicYearDetails);
            const string title = "Detalles de Año Académico Agregado";
            const string content = "El detalle del año académico ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return Redirect(string.Format("~/AcademicYearDetails/Index/{0}", academicYearDetailsModel.AcademicYearId));
        }
    }
}