using System.Web.Mvc;
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
        [AuthorizeAdmin]
        public ActionResult Edit(ContactInformationEditModel modelContactInformation)
        {
            ContactInformation myContactInformation = _contactInformationRepository.GetById(modelContactInformation.Id);
            myContactInformation.Type = modelContactInformation.Type;
            myContactInformation.Value = modelContactInformation.Value;
            ContactInformation contactInformation = _contactInformationRepository.Update(myContactInformation);
            const string title = "Contacto Actualizado";
            _viewMessageLogic.SetNewMessage(title, "", ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + contactInformation.People.Id, modelContactInformation.Controller);
        }

        [HttpPost]
        [AuthorizeAdmin]
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
        [AuthorizeAdmin]
        public ActionResult Add(long id)
        {
            var model = new ContactInformationRegisterModel
                        {
                            Id = id
                        };
            return View("ContactAdd", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(ContactInformationRegisterModel modelContactInformation)
        {
            var myContactInformation = new ContactInformation
                                       {
                                           Type = modelContactInformation.Type,
                                           Value = modelContactInformation.Value,
                                           People = _peopleRepository.GetById(modelContactInformation.Id)
                                       };
            ContactInformation contactInformation = _contactInformationRepository.Create(myContactInformation);
            const string title = "Informacion Agregada";
            _viewMessageLogic.SetNewMessage(title, "", ViewMessageType.SuccessMessage);
            return RedirectToAction("Details/" + contactInformation.People.Id, modelContactInformation.Controller);
        }
    }
}