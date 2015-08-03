using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
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

        public GradeController(IGradeRepository gradeRepository)
        {
            if (gradeRepository == null) throw new ArgumentNullException("gradeRepository");
            _gradeRepository = gradeRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        /// GET: /Grade/
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
            Mapper.CreateMap<DisplayGradeModel, Grade>().ReverseMap();
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
        public ActionResult Add()
        {
            return View("Create");
        }

        /// POST: /Grade/Add
        [HttpPost]
        public ActionResult Add(GradeRegisterModel modelGrade)
        {
            string title;
            string content;
            Mapper.CreateMap<Grade, GradeRegisterModel>().ReverseMap();
            var gradeModel = Mapper.Map<GradeRegisterModel, Grade>(modelGrade);
            var myGrade = _gradeRepository.GenerateGradeFromRegisterModel(gradeModel);
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
            var grade = _gradeRepository.Create(myGrade);
            title = "Grado Agregado";
            content = grade.Name + " grado ha sido guardado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        /// POST: /Grade/Delete/5
        [HttpPost]
        public ActionResult Delete(long id)
        {
            var grade = _gradeRepository.Delete(id);
            const string title = "Grado ha sido Eliminado";
            var content = grade.Name + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }
        
        /// GET: /Grade/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var grade = _gradeRepository.GetGradeEditModelById(id);
            Mapper.CreateMap<GradeEditModel, Grade>().ReverseMap();
            var gradeModel = Mapper.Map<Grade, GradeEditModel>(grade);
            return View("Edit", gradeModel);
        }

        ///  POST: /Grade/Edit/5
        [HttpPost]
        public ActionResult Edit(GradeEditModel modelGrade)
        {
            var myGrade = _gradeRepository.GetById(modelGrade.Id);
            Mapper.CreateMap<Grade, GradeEditModel>().ReverseMap();
            var gradeModel = Mapper.Map<GradeEditModel, Grade>(modelGrade);
            _gradeRepository.UpdateGradeFromGradeEditModel(gradeModel, myGrade);
            const string title = "Grado Actualizado";
            var content = myGrade.Name + " grado ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }
    }
}