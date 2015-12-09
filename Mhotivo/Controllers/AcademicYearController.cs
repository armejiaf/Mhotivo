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
    public class AcademicYearController : Controller
    {
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IUserRepository _userRepository;
        private readonly ISessionManagementService _sessionManagementService;

        public AcademicYearController(IAcademicYearRepository academicYearRepository, IUserRepository userRepository, ISessionManagementService sessionManagementService)
        {
            _academicYearRepository = academicYearRepository;
            _userRepository = userRepository;
            _sessionManagementService = sessionManagementService;
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
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Add()
        {
            return View("AutoGeneration", new AcademicYearRegisterModel());
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
            var content = "El año académico " + toCreate.Year + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }
    }
}