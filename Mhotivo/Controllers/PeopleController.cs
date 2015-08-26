using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using AutoMapper;
using Mhotivo.Authorizations;

namespace Mhotivo.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public PeopleController(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

         [AuthorizeAdmin]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            return View(_peopleRepository.GetAllPeople()); //Compilation Magic!
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            var people = _peopleRepository.GetPeopleEditModelById(id);
            Mapper.CreateMap<PeopleEditModel, People>().ReverseMap();
            var peopleModel = Mapper.Map<People, PeopleEditModel>(people);
            return View("Edit", peopleModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(PeopleEditModel peopleModel)
        {
            var people = _peopleRepository.GetById(peopleModel.Id);
            Mapper.CreateMap<People, PeopleEditModel>().ReverseMap();
            var peopleEdit = Mapper.Map<PeopleEditModel, People>(peopleModel);
            _peopleRepository.UpdatePeopleFromPeopleEditModel(peopleEdit, people);
            const string title = "Persona Actualizada";
            var content = "La persona " + people.FullName + " - " + people.Id + " ha sido actualizada exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }
    }
}