using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Authorizations;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class ContactInformationController : Controller
    {
        private readonly IContactInformationRepository _contactInformationRepository;
        private readonly IPeopleRepository _peopleRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public ContactInformationController(IContactInformationRepository contactInformationRepository,
            IPeopleRepository peopleRepository)
        {
            _contactInformationRepository = contactInformationRepository;
            _peopleRepository = peopleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }


        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Edit(ContactInformationEditModel modelContactInformation)
        {
            ContactInformation myContactInformation = _contactInformationRepository.GetById(modelContactInformation.Id);
            myContactInformation = Mapper.Map(modelContactInformation, myContactInformation);
            ContactInformation contactInformation = _contactInformationRepository.Update(myContactInformation);
            const string title = "Contacto Actualizado";
            _viewMessageLogic.SetNewMessage(title, "", ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + contactInformation.People.Id, modelContactInformation.Controller);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Delete(long id, string control)
        {
            ContactInformation myContactInformation = _contactInformationRepository.GetById(id);
            long ID = myContactInformation.People.Id;
            _contactInformationRepository.Delete(id);
            const string title = "Informacion Eliminada";
            _viewMessageLogic.SetNewMessage(title, "", ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + ID, control);
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Add(long id)
        {
            var model = new ContactInformationRegisterModel
            {
                People = id
            };
            return View("ContactAdd", model);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Add(ContactInformationRegisterModel modelContactInformation)
        {
            var myContactInformation = Mapper.Map<ContactInformation>(modelContactInformation);
            ContactInformation contactInformation = _contactInformationRepository.Create(myContactInformation);
            const string title = "Informacion Agregada";
            _viewMessageLogic.SetNewMessage(title, "", ViewMessageType.SuccessMessage);
            return RedirectToAction("Details/" + contactInformation.People.Id, modelContactInformation.Controller);
        }
    }
}