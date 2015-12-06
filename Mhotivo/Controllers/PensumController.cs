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
    public class PensumController : Controller
    {

        private readonly IPensumRepository _pensumRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IAcademicGradeRepository _academicGradeRepository;

        public PensumController(IPensumRepository pensumRepository, IGradeRepository gradeRepository, ICourseRepository courseRepository, IAcademicGradeRepository academicGradeRepository)
        {
            _pensumRepository = pensumRepository;
            _gradeRepository = gradeRepository;
            _courseRepository = courseRepository;
            _academicGradeRepository = academicGradeRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdminDirector]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int gradeId)
        {
            ViewBag.gradeId = gradeId;
            _viewMessageLogic.SetViewMessageIfExist();
            var temp = _pensumRepository.Filter(x => x.Grade.Id == gradeId).ToList();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CourseSortParm = string.IsNullOrEmpty(sortOrder) ? "course_desc" : "";
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            if (!string.IsNullOrEmpty(searchString))
            {
                temp = _pensumRepository.Filter(x => x.Name.Contains(searchString)).ToList();
            }
            var list = temp.Select(Mapper.Map<PensumDisplayModel>).OrderBy(s => s.Name).ToList();
            ViewBag.CurrentFilter = searchString;
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Edit(long id)
        {
            Pensum thisPensum = _pensumRepository.GetById(id);
            var model = Mapper.Map<PensumEditModel>(thisPensum);
            return View("Edit", model);
        }


        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Edit(PensumEditModel modelPensum)
        {
            Pensum myPensum = _pensumRepository.GetById(modelPensum.Id);
            if (_pensumRepository.Filter(x => x.Grade.Id == myPensum.Grade.Id && x.Id != modelPensum.Id && x.Name.Equals(modelPensum.Name)).Any())
            {
                _viewMessageLogic.SetNewMessage("Error", "Ya existe un pensum con ese nombre.", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index", new { gradeId = myPensum.Grade.Id });
            }
            myPensum = Mapper.Map(modelPensum, myPensum);
            Pensum pensum = _pensumRepository.Update(myPensum);
            const string title = "Pensum Actualizado";
            string content = "El Pensum " + pensum.Name +
                             " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", new { gradeId= myPensum.Grade.Id });
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Delete(long id)
        {
            Pensum pensum = _pensumRepository.GetById(id);
            var gradeId = pensum.Grade.Id;
            if (_academicGradeRepository.Filter(x => x.ActivePensum.Id == id).Any())
            {
                _viewMessageLogic.SetNewMessage("Error", "El pensum esta siendo usado por un grado academico y no puede eliminarse.", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index", new { gradeId });
            }
            pensum = _pensumRepository.Delete(pensum);
            const string title = "Pensum Eliminado";
            string content = "El Pesum " + pensum.Name + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", new {gradeId});
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Add(long gradeId)
        {
            return View("Create", new PensumRegisterModel {Grade = gradeId});
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Add(PensumRegisterModel modelPensum)
        {
            if (_pensumRepository.Filter(x => x.Grade.Id == modelPensum.Grade && x.Name.Equals(modelPensum.Name)).Any())
            {
                _viewMessageLogic.SetNewMessage("Error", "Ya existe un pensum con ese nombre.", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index", new { gradeId = modelPensum.Grade });
            }
            var myPensum = Mapper.Map<Pensum>(modelPensum);
            myPensum = _pensumRepository.Create(myPensum);
            const string title = "Pensum Agregado";
            string content = "El pensum " + myPensum.Name +  " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index", new {gradeId = modelPensum.Grade});
        }

        /*[HttpGet]
        public ActionResult Details(int pensumId)
        {
            var pensum = _pensumRepository.GetById(pensumId);
            var pensumModel = Mapper.Map<PensumCourseModel>(pensum);
            ViewBag.Title = "Detalles para el pensum: " + pensumModel.Name;
            ViewBag.PensumId = pensumId;
            return View(pensumModel);
        }*/

        /*[HttpPost]
        public ActionResult Details(PensumCourseModel model)
        {
            var pensum = _pensumRepository.GetById(model.Id);
            pensum = Mapper.Map(model, pensum);
            _pensumRepository.Update(pensum);
            const string title = "Detalles de Pensum Actualizados";
            string message = "Los detalles del pensum " + model.Name + " han sido actualizados exitosamente.";
            _viewMessageLogic.SetNewMessage(title, message, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }*/
    }
}