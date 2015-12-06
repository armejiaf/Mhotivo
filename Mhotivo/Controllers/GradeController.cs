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
    public class GradeController : Controller
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IAcademicGradeRepository _academicGradeRepository;
        private readonly IPensumRepository _pensumRepository;
        private readonly IEducationLevelRepository _educationLevelRepository;
        private readonly ISessionManagementService _sessionManagementService;
        private readonly IUserRepository _userRepository;

        public GradeController(IGradeRepository gradeRepository, IAcademicGradeRepository academicGradeRepository, IPensumRepository pensumRepository, IEducationLevelRepository educationLevelRepository, ISessionManagementService sessionManagementService, IUserRepository userRepository)
        {
            _gradeRepository = gradeRepository;
            _academicGradeRepository = academicGradeRepository;
            _pensumRepository = pensumRepository;
            _educationLevelRepository = educationLevelRepository;
            _sessionManagementService = sessionManagementService;
            _userRepository = userRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        /// GET: /Grade/
        [AuthorizeAdminDirector]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
            ViewBag.IsDirector = user.Role.Name.Equals("Director");
            var grades = (bool)ViewBag.IsDirector
                ? _gradeRepository.Filter(
                    x => x.EducationLevel.Director != null && x.EducationLevel.Director.Id == user.Id)
                : _gradeRepository.GetAllGrade();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.EducationLevelSortParam = sortOrder == "education_asc" ? "education_desc" : "education_asc";
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
                grades = ((bool)ViewBag.IsDirector
                ? _gradeRepository.Filter(
                    x => x.EducationLevel.Director != null && x.EducationLevel.Director.Id == user.Id)
                : _gradeRepository.GetAllGrade()).Where(x => x.Name.Contains(searchString)).ToList();
            }
            var displayGradeModels = grades.Select(Mapper.Map<Grade, GradeDisplayModel>).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "education_desc":
                    displayGradeModels = displayGradeModels.OrderByDescending(s => s.EducationLevel).ToList();
                    break;
                case "education_asc":
                    displayGradeModels = displayGradeModels.OrderBy(s => s.EducationLevel).ToList();
                    break;
                case "name_desc":
                    displayGradeModels = displayGradeModels.OrderByDescending(s => s.Name).ToList();
                    break;
                default:  // Name ascending 
                    displayGradeModels = displayGradeModels.OrderBy(s => s.Name).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(displayGradeModels.ToPagedList(pageNumber, pageSize));
        }

        /// GET: /Grade/Add
        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Add()
        {
            var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
            var isDirector = ViewBag.IsDirector = user.Role.Name.Equals("Director");
            if (isDirector)
            {
                var firstOrDefault = _educationLevelRepository.Filter(x => x.Director != null && x.Director.Id == user.Id)
                    .FirstOrDefault();
                if (firstOrDefault != null)
                    return View("Create", new GradeRegisterModel {EducationLevel = firstOrDefault.Id});
            }
            var list = _educationLevelRepository.GetAllAreas();
            ViewBag.EducationLevels = new SelectList(list, "Id", "Name");
            return View("Create");
        }

        /// POST: /Grade/Add
        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Add(GradeRegisterModel modelGrade)
        {
            string title;
            string content;
            var gradeModel = Mapper.Map<GradeRegisterModel, Grade>(modelGrade);
            var query =
                _gradeRepository.Filter(
                    g => g.Name.Equals(gradeModel.Name) && g.EducationLevel.Id == gradeModel.EducationLevel.Id);
            if (query.Any())
            {
                title = "Error!";
                content = "El Grado ya existe.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var grade = _gradeRepository.Create(gradeModel);
            title = "Grado Agregado";
            content = grade.Name + " grado ha sido guardado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        /// POST: /Grade/Delete/5
        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Delete(long id)
        {
            if (!_academicGradeRepository.Filter(x => x.Grade.Id == id).Any())
            {
                var grade = _gradeRepository.Delete(id);
                const string title = "Grado ha sido Eliminado";
                var content = grade.Name + " ha sido eliminado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
                return RedirectToAction("Index");
            }
            else
            {
                const string title = "Error!";
                const string content = "No se puede borrar el grado pues existe un año académico con este grado.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
        }
        
        /// GET: /Grade/Edit/5
        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Edit(long id)
        {
            var grade = _gradeRepository.GetById(id);
            var gradeModel = Mapper.Map<Grade, GradeEditModel>(grade);
            var user = _userRepository.GetById(Convert.ToInt64(_sessionManagementService.GetUserLoggedId()));
            var isDirector = ViewBag.IsDirector = user.Role.Name.Equals("Director");
            if (isDirector) return View("Edit", gradeModel);
            var list = _educationLevelRepository.GetAllAreas();
            ViewBag.EducationLevels = new SelectList(list, "Id", "Name", gradeModel.EducationLevel);
            return View("Edit", gradeModel);
        }

        ///  POST: /Grade/Edit/5
        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Edit(GradeEditModel modelGrade)
        {
            if (!_gradeRepository.Filter(x => x.Id != modelGrade.Id && x.Name == modelGrade.Name).Any())
            {
                var myGrade = _gradeRepository.GetById(modelGrade.Id);
                myGrade = Mapper.Map(modelGrade, myGrade);
                _gradeRepository.Update(myGrade);
                const string title = "Grado Actualizado";
                var content = myGrade.Name + " grado ha sido actualizado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
                return RedirectToAction("Index");
            }
            const string titulo = "Error!";
            const string contenido = "Ya existe un grado con esa informacion.";
            _viewMessageLogic.SetNewMessage(titulo, contenido, ViewMessageType.ErrorMessage);
            return RedirectToAction("Index");
        }

        [AuthorizeAdminDirector]
        public ActionResult Details(long id)
        {
            var pensums = _pensumRepository.Filter(x => x.Grade.Id == id).Select(Mapper.Map<PensumDisplayModel>);
            return View(pensums);
        }
    }
}