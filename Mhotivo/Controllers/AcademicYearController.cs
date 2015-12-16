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
    public class AcademicYearController : Controller
    {
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IUserRepository _userRepository;
        private readonly ISessionManagementService _sessionManagementService;
        private readonly IGradeRepository _gradeRepository;
        private readonly IPensumRepository _pensumRepository;
        private readonly IAcademicGradeRepository _academicGradeRepository;
        private readonly IAcademicCourseRepository _academicCourseRepository;

        public AcademicYearController(IAcademicYearRepository academicYearRepository, IUserRepository userRepository, ISessionManagementService sessionManagementService, IGradeRepository gradeRepository, IPensumRepository pensumRepository, IAcademicGradeRepository academicGradeRepository, IAcademicCourseRepository academicCourseRepository)
        {
            _academicYearRepository = academicYearRepository;
            _userRepository = userRepository;
            _sessionManagementService = sessionManagementService;
            _gradeRepository = gradeRepository;
            _pensumRepository = pensumRepository;
            _academicGradeRepository = academicGradeRepository;
            _academicCourseRepository = academicCourseRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdminDirector]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
            ViewBag.IsDirector = user.Role.Name.Equals("Director");
            var allAcademicYears = _academicYearRepository.GetAllAcademicYears();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "year_desc" : "";
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    var year = Convert.ToInt32(searchString);
                    allAcademicYears = _academicYearRepository.Filter(x => x.Year.Equals(year));
                }
                catch (Exception)
                {
                    allAcademicYears = _academicYearRepository.GetAllAcademicYears();
                }
            }
            var academicYears = allAcademicYears.Select(Mapper.Map<AcademicYearDisplayModel>).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "year_desc":
                    academicYears = academicYears.OrderByDescending(s => s.Year).ToList();
                    break;
                default:  // Name ascending 
                    academicYears = academicYears.OrderBy(s => s.Year).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(academicYears.ToPagedList(pageNumber,pageSize));
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Edit(long id)
        {
            var academicYear = _academicYearRepository.GetById(id);
            var academicYearModel = Mapper.Map<AcademicYearEditModel>(academicYear);
            return View("Edit", academicYearModel);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Edit(AcademicYearEditModel modelAcademicYear)
        {
            if (modelAcademicYear.IsActive &&
                _academicYearRepository.Filter(x => x.IsActive && x.Id != modelAcademicYear.Id).Any())
            {
                _viewMessageLogic.SetNewMessage("Error", "Solo puede haber un año académico activo.", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var myAcademicYear = _academicYearRepository.GetById(modelAcademicYear.Id);
            myAcademicYear = Mapper.Map(modelAcademicYear, myAcademicYear);
            myAcademicYear = _academicYearRepository.Update(myAcademicYear);
            const string title = "Año Académico Actualizado ";
            var content = "El año académico " + myAcademicYear.Year + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Delete(long id)
        {
            //TODO: Extra validations when deleting.
            var academicYear = _academicYearRepository.GetById(id);
            if (academicYear.IsActive)
            {
                const string title = "Error";
                const string content = "No se puede borrar el año académico activo.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            else
            {
                academicYear = _academicYearRepository.Delete(academicYear);
                const string title = "Año Académico Eliminado";
                var content = "El año académico " + academicYear.Year + " ha sido eliminado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Add()
        {
            return View("Create", new AcademicYearRegisterModel());
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult AutoGeneration(long yearId)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var model = _gradeRepository.GetAllGrade().Select(n => new NewAcademicYearGradeSpecModel {Grade = n.Id, Reference = n});
            ViewBag.YearId = yearId;
            return View("AutoGeneration", model);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult AutoGeneration(IEnumerable<NewAcademicYearGradeSpecModel> model, long yearId)
        {
            var year = _academicYearRepository.GetById(yearId);
            foreach (var newAcademicYeardGradeSpecModel in model)
            {
                char section = 'A';
                var grade = _gradeRepository.GetById(newAcademicYeardGradeSpecModel.Grade);
                var pensum = _pensumRepository.GetById(newAcademicYeardGradeSpecModel.SelectedPensum);
                for (int i = 0; i < newAcademicYeardGradeSpecModel.Sections; i++)
                {
                    var newGrade = new AcademicGrade
                    {
                        Grade = grade,
                        AcademicYear = year,
                        Section = section++ + "",
                        ActivePensum = pensum
                    };
                    newGrade = _academicGradeRepository.Create(newGrade);
                    foreach (var course in newGrade.ActivePensum.Courses)
                    {
                        var academicCourse = new AcademicCourse
                        {
                            Course = course,
                            AcademicGrade = newGrade
                        };
                        academicCourse = _academicCourseRepository.Create(academicCourse);
                        newGrade.CoursesDetails.Add(academicCourse);
                        newGrade = _academicGradeRepository.Update(newGrade);
                    }
                    year.Grades.Add(newGrade);
                    _academicYearRepository.Update(year);
                }
            }
            const string title = "Año Académico Agregado";
            var content = "El año académico " + year.Year + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", "AcademicGrade", new {yearId});
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Add(AcademicYearRegisterModel academicYearModel)
        {
            if (_academicYearRepository.Filter(x => x.Year == academicYearModel.Year).Any())
            {
                _viewMessageLogic.SetNewMessage("Error", "Este año académico ya existe.", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var toCreate = Mapper.Map<AcademicYear>(academicYearModel);
            toCreate = _academicYearRepository.Create(toCreate);
            const string title = "Año Académico Agregado";
            bool v = _gradeRepository.GetAllGrade().Any();
            var content = v ? "Elija la cantidad de secciones a crearse y el pensum a usarse para cada grado." 
                : "El año académico " + toCreate.Year + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return v ? RedirectToAction("AutoGeneration", new {yearId = toCreate.Id}) : RedirectToAction("Index", "AcademicGrade", new { toCreate.Id });
        }
    }
}