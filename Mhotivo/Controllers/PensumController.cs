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
    public class PensumController : Controller
    {

        private readonly IPensumRepository _pensumRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public PensumController(IPensumRepository pensumRepository, IGradeRepository gradeRepository, ICourseRepository courseRepository)
        {
            _pensumRepository = pensumRepository;
            _gradeRepository = gradeRepository;
            _courseRepository = courseRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int gradeId)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var temp = _pensumRepository.Filter(x => x.Grade.Id == gradeId).ToList();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CourseSortParm = String.IsNullOrEmpty(sortOrder) ? "course_desc" : "";
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            if (!String.IsNullOrEmpty(searchString))
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
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            Pensum thisPensum = _pensumRepository.GetById(id);
            var model = Mapper.Map<PensumEditModel>(thisPensum);
            return View("Edit", model);
        }


        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(PensumEditModel modelPensum)
        {
            Pensum myPensum = _pensumRepository.GetById(modelPensum.Id);
            myPensum = Mapper.Map(modelPensum, myPensum);
            Pensum pensum = _pensumRepository.Update(myPensum);
            const string title = "Pensum Actualizado";
            string content = "El Pensum " + pensum.Name +
                             " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            Pensum pensum = _pensumRepository.Delete(id);
            const string title = "Pensum Eliminado";
            string content = "El Pesum " + pensum.Name + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(PensumRegisterModel modelPensum)
        {
            var myPensum = Mapper.Map<Pensum>(modelPensum);
            myPensum = _pensumRepository.Create(myPensum);
            const string title = "Pensum Agregado";
            string content = "El pensum " + myPensum.Name +  " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }
    }
}