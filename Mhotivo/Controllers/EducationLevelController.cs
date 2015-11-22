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
        private readonly IGradeRepository _gradeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public EducationLevelController(IEducationLevelRepository areaReposity, IGradeRepository gradeRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _areaReposity = areaReposity;
            _gradeRepository = gradeRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
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
            var listaAreaDisplaysModel = listaArea.Select(Mapper.Map<EducationLevel, EducationLevelDisplayModel>).ToList();
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
            return View("Create");
        }

        
        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Create(EducationLevelRegisterModel modelArea)
        {
            var area = Mapper.Map<EducationLevel>(modelArea);
            if (_areaReposity.Filter(x => x.Name == modelArea.Name).Any())
            {
                const string title = "Error!";
                const string content = "Ya existe un nivel educativo con ese nombre.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            else
            {
                _areaReposity.Create(area);
                const string title = "Nivel De Educacion Agregado";
                var content = "El nivel educaivo \"" + area.Name + "\" ha sido agregado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            var check = _gradeRepository.Filter(x => x.EducationLevel.Id == id);
            if (!check.Any())
            {
                var area = _areaReposity.Delete(id);
                const string title = "Nivel De Educacion Eliminado";
                var content = "El Nivel De Educacion \"" + area.Name + "\" ha sido eliminado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
                return RedirectToAction("Index");
            }
            else
            {
                const string title = "Error!";
                const string content = "No se puede borrar el nivel de educacion pues existen uno o mas grados dentro de este.";
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
            return View("Edit", area);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(EducationLevelEditModel modelArea)
        {
            if (_gradeRepository.Filter(x => x.Name == modelArea.Name && x.Id != modelArea.Id).Any())
            {
                const string titulo = "Error!";
                const string contenido = "No se pueden aceptar los cambios ya que existe otro nivel educativo con ese nombre.";
                _viewMessageLogic.SetNewMessage(titulo, contenido, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var myArea = _areaReposity.GetById(modelArea.Id);
            myArea = Mapper.Map(modelArea, myArea);
            myArea = _areaReposity.Update(myArea);
            const string title = "Nivel de Educacion Actualizado";
            var content = "El Nivel De Educacion" + myArea.Name + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        public ActionResult EditDirector(int id)
        {
            var model = Mapper.Map<EducationLevelDirectorAssignModel>(_areaReposity.GetById(id));
            var role = _roleRepository.Filter(n => n.Name.Equals("Director")).FirstOrDefault();
            var directors = _userRepository.Filter(x => x.Role.Id == role.Id).ToList();
            ViewBag.Directors = new SelectList(directors, "Id", "UserOwner.FullName", model.Director);
            return View("EditDirector", model);
        }

        [HttpPost]
        public ActionResult EditDirector(EducationLevelDirectorAssignModel model)
        {
            if (_areaReposity.Filter(x => x.Director.Id == model.Director).Any())
            {
                const string titulo = "Error!";
                const string contenido = "No se pueden asignar varios niveles educativos a un director.";
                _viewMessageLogic.SetNewMessage(titulo, contenido, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var level = _areaReposity.GetById(model.Id);
            level = Mapper.Map(model, level);
            level = _areaReposity.Update(level);
            const string title = "Director Asignado";
            var content = "Se ha asignado el director de " + level.Name+".";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }
    }
}
