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
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdminDirector]
        public ActionResult Index(long pensumId, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            ViewBag.PensumId = pensumId;
            var list =
                _courseRepository.Filter(x => x.Pensum.Id == pensumId).ToList().Select(Mapper.Map<CourseDisplayModel>);
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [AuthorizeAdminDirector]
        public ActionResult Add(long pensumId)
        {
            return View("Create", new CourseRegisterModel{Pensum = pensumId});
        }


        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Add(CourseRegisterModel model)
        {
            string title;
            string content;
            var toCreate = Mapper.Map<Course>(model);
            var toCheck = _courseRepository.Filter(x => x.Name == model.Name && x.Pensum.Id == model.Pensum);
            if (toCheck.Any())
            {
                title = "Error!";
                content = "El Curso ya existe.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index", new { pensumId = model.Pensum });
            }
            toCreate = _courseRepository.Create(toCreate);
            title = "Curso Agregado";
            content = "El pensum " + toCreate.Name + " ha sido guardado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", new{pensumId = model.Pensum});
        }

        [AuthorizeAdminDirector]
        public ActionResult Edit(long id)
        {
            var item = _courseRepository.GetById(id);
            var toReturn = Mapper.Map<CourseEditModel>(item);
            return View(toReturn);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Edit(CourseEditModel model)
        {
            var item = _courseRepository.GetById(model.Id);
            var list = _courseRepository.Filter(x => x.Name == model.Name && x.Id == model.Id);
            var list2 = _courseRepository.Filter(x => x.Name == model.Name && x.Id != model.Id);
            string title;
            string content;
            if (list2.Any())
            {
                title = "Error!";
                content = "El Curso ya existe.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
            }
            else if (!list.Any())
            {
                item = Mapper.Map(model, item);
                item = _courseRepository.Update(item);
                title = "Curso Actualizado!";
                content = "El Curso " + item.Name + " fue actualizado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            }
            return RedirectToAction("Index", new { pensumId = item.Pensum.Id });
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Delete(long id)
        {
            var item = _courseRepository.GetById(id);
            var pensumId = item.Pensum.Id;
            item = _courseRepository.Delete(item);
            const string title = "Curso Eliminado!";
            var content = "El Curso " + item.Name + " fue eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", new { pensumId });
        }
    }
}