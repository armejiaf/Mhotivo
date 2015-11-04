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
        private IAcademicYearRepository _academicYearRepository;
        private readonly IAcademicYearGradeRepository _academicYearGradeRepository;

        public GradeController(IGradeRepository gradeRepository, IAcademicYearRepository academicYearRepository, IAcademicYearGradeRepository academicYearGradeRepository)
        {
            if (gradeRepository == null) throw new ArgumentNullException("gradeRepository");
            _gradeRepository = gradeRepository;
            _academicYearRepository = academicYearRepository;
            _academicYearGradeRepository = academicYearGradeRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        /// GET: /Grade/
         [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var grades = _gradeRepository.GetAllGrade();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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
                grades = _gradeRepository.Filter(x => x.Name.Contains(searchString)).ToList();
            }
            var displayGradeModels = grades.Select(Mapper.Map<Grade, DisplayGradeModel>).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
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
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            return View("Create");
        }

        /// POST: /Grade/Add
        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(GradeRegisterModel modelGrade)
        {
            string title;
            string content;
            var gradeModel = Mapper.Map<GradeRegisterModel, Grade>(modelGrade);
            var existGrade =
                _gradeRepository.GetAllGrade()
                    .FirstOrDefault(
                        g => g.Name.Equals(modelGrade.Name) && g.EducationLevel.Equals(modelGrade.EducationLevel));
            if (existGrade != null)
            {
                title = "Grado";
                content = "El grado " + existGrade.Name + " ya existe.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
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
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            var check = _academicYearGradeRepository.Filter(x => x.Grade.Id == id && x.AcademicYear.IsActive).FirstOrDefault();
            if (check == null)
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
                var content = "No se puede borrar el grado pues existe un año académico con este grado.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
        }
        
        /// GET: /Grade/Edit/5
        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            var grade = _gradeRepository.GetById(id);
            var gradeModel = Mapper.Map<Grade, GradeEditModel>(grade);
            return View("Edit", gradeModel);
        }

        ///  POST: /Grade/Edit/5
        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(GradeEditModel modelGrade)
        {
            var myGrade = _gradeRepository.GetById(modelGrade.Id);
            Mapper.Map(modelGrade, myGrade);
            _gradeRepository.Update(myGrade);
            const string title = "Grado Actualizado";
            var content = myGrade.Name + " grado ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }
    }
}