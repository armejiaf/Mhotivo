using System;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using AutoMapper;
using Mhotivo.Authorizations;
using PagedList;

namespace Mhotivo.Controllers
{
    public class EducationLevelController : Controller
    {
        //
        // GET: /Area/
        private readonly IEducationLevelRepository _areaReposity;
        private readonly ViewMessageLogic _viewMessageLogic;
        private ICourseRepository _courseRepository;

        public EducationLevelController(IEducationLevelRepository areaReposity, ICourseRepository courseRepository)
        {
            _areaReposity = areaReposity;
            _courseRepository = courseRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }
        [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var listaArea = _areaReposity.GetAllAreas();
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
                listaArea = _areaReposity.Filter(x => x.Name.Contains(searchString)).ToList();
            }
            var listaAreaDisplaysModel = listaArea.Select(Mapper.Map<EducationLevel, DisplayEducationLevelModel>).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "name_desc":
                    listaAreaDisplaysModel = listaAreaDisplaysModel.OrderByDescending(s => s.Name).ToList();
                    break;
                default:  // Name ascending 
                    listaAreaDisplaysModel = listaAreaDisplaysModel.OrderBy(s => s.Name).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(listaAreaDisplaysModel.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(_areaReposity.Query(x => x), "Id", "Name");
            return View("Create");
        }

        
        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Create(EducationLevelRegisterModel modelArea)
        {
            var area = new EducationLevel
            {
                Name = modelArea.Name,
            };
            _areaReposity.Create(area);
            const string title = "Nivel De Educacion Agregado";
            var content = "El area " + area.Name + " ha sido agregada exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            var check = _courseRepository.Filter(x => x.Area.Id == id).FirstOrDefault();
            if (check == null)
            {
                var area = _areaReposity.Delete(id);
                const string title = "Nivel De Educacion Eliminado";
                var content = "El Nivel De Educacion " + area.Name + " ha sido eliminado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
                return RedirectToAction("Index");
            }
            else
            {
                const string title = "Error!";
                var content = "No se puede borrar el nivel de educacion pues existe un curso dentro de este.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            EducationLevel thisArea = _areaReposity.GetById(id);
            var area = Mapper.Map<EducationLevel,EducationLevelEditModel>(thisArea);
            area.Name = thisArea.Name;
            return View("Edit", area);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(EducationLevelEditModel modelArea)
        {
            var myArea = Mapper.Map<EducationLevel>(modelArea);
            myArea.Name = modelArea.Name;
            EducationLevel area = _areaReposity.Update(myArea);
            const string title = "Nivel de Educacion Actualizado";
            var content = "El Nivel De Educacion" + area.Name + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }
    }
}
