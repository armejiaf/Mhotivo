using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using AutoMapper;
using Mhotivo.Authorizations;
using PagedList;

namespace Mhotivo.Controllers
{
    public class AdministrativeController : Controller
    {
        private readonly IContactInformationRepository _contactInformationRepository;
        private readonly IPeopleWithUserRepository _peopleWithUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly IRoleRepository _roleRepository;

        public AdministrativeController(IContactInformationRepository contactInformationRepository, IUserRepository userRepository, 
            IRoleRepository roleRepository, IPasswordGenerationService passwordGenerationService, IPeopleWithUserRepository peopleWithUserRepository)
        {
            _contactInformationRepository = contactInformationRepository;
            _userRepository = userRepository;
            _passwordGenerationService = passwordGenerationService;
            _peopleWithUserRepository = peopleWithUserRepository;
            _roleRepository = roleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

         [AuthorizeAdmin]
        public ActionResult Index(string currentFilter, string searchString, int? page)
         {
             var admins = _peopleWithUserRepository.Filter(
                 x => x.User.Role.Name.Equals("Administrador") || x.User.Role.Name.Equals("Director"));
             if (searchString != null)
                 page = 1;
             else
                 searchString = currentFilter;
             if (!string.IsNullOrEmpty(searchString))
             {
                 try
                 {
                     admins = _peopleWithUserRepository.Filter(
                  x => (x.User.Role.Name.Equals("Administrador") || x.User.Role.Name.Equals("Director")) && (x.FullName.Contains(searchString)));
                 }
                 catch (Exception)
                 {
                     admins = _peopleWithUserRepository.Filter(
                  x => x.User.Role.Name.Equals("Administrador") || x.User.Role.Name.Equals("Director"));
                 }
             }
             ViewBag.CurrentFilter = searchString;
             var model = Mapper.Map<IEnumerable<PeopleWithUser>, IEnumerable<AdministrativeDisplayModel>>(admins);
             const int pageSize = 10;
             var pageNumber = (page ?? 1);
             return View(model.ToPagedList(pageNumber, pageSize));
         }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult ContactEdit(long id)
        {
            ContactInformation thisContactInformation = _contactInformationRepository.GetById(id);
            var contactInformation = new ContactInformationEditModel
            {
                Type = thisContactInformation.Type,
                Value = thisContactInformation.Value,
                Id = thisContactInformation.Id,
                People = thisContactInformation.People,
                Controller = "Administrative"
            };
            return View("ContactEdit", contactInformation);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            var admin = _peopleWithUserRepository.GetById(id);
            var adminModel = Mapper.Map<PeopleWithUser, AdministrativeEditModel>(admin);
            return View("Edit", adminModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(AdministrativeEditModel modelAdmin)
        {
            var validImageTypes = new []
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };
            if (modelAdmin.UploadPhoto != null && modelAdmin.UploadPhoto.ContentLength > 0)
            {
                if (!validImageTypes.Contains(modelAdmin.UploadPhoto.ContentType))
                {
                    ModelState.AddModelError("UploadPhoto", "Por favor seleccione entre una imagen GIF, JPG o PNG");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    byte[] fileBytes = null;
                    if (modelAdmin.UploadPhoto != null)
                    {
                        using (var binaryReader = new BinaryReader(modelAdmin.UploadPhoto.InputStream))
                        {
                            fileBytes = binaryReader.ReadBytes(modelAdmin.UploadPhoto.ContentLength);
                        }
                    }
                    var myAdmin = _peopleWithUserRepository.GetById(modelAdmin.Id);
                    Mapper.Map(modelAdmin, myAdmin);
                    myAdmin.Photo = fileBytes ?? myAdmin.Photo;
                    _peopleWithUserRepository.Update(myAdmin);
                    const string title = "Maestro Actualizado";
                    var content = "El maestro " + myAdmin.FullName + " ha sido actualizado exitosamente.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(modelAdmin);
                }
            }
            return View(modelAdmin);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            PeopleWithUser admin = _peopleWithUserRepository.Delete(id);
            const string title = "Maestro Eliminado";
            var content = "El maestro " + admin.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult ContactAdd(long id)
        {
            var model = new ContactInformationRegisterModel
                        {
                            Id = id,
                            Controller = "Administrative"
                        };
            return View("ContactAdd", model);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            ViewBag.Roles = new SelectList(_roleRepository.Filter(x => x.Name.Equals("Administrador") || x.Name.Equals("Director")), "Id", "Name");
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(AdministrativeRegisterModel modelAmin)
        {
            var adminModel = Mapper.Map<AdministrativeRegisterModel, PeopleWithUser>(modelAmin);
            if (_peopleWithUserRepository.Filter(x => x.IdNumber == modelAmin.IdNumber).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "Ya existe un maestro con ese numero de Identidad", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            if (_peopleWithUserRepository.Filter(x => x.User.Email == modelAmin.Email).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }

            var newUser = new User
            {
                Email = modelAmin.Email,
                Password = _passwordGenerationService.GenerateTemporaryPassword(),
                IsUsingDefaultPassword = true,
                IsActive = true,
                Role = _roleRepository.GetById(modelAmin.Role),
                UserOwner = adminModel
            };
            newUser.DefaultPassword = newUser.Password;
            newUser = _userRepository.Create(newUser);
            adminModel.User = newUser;
            _peopleWithUserRepository.Create(adminModel);
            const string title = "Maestro Agregado";
            var content = "El maestro " + adminModel.FullName + "ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Details(long id)
        {
            var admin = _peopleWithUserRepository.GetById(id);
            var adminModel = Mapper.Map<PeopleWithUser, AdministrativeDisplayModel>(admin);
            return View("Details", adminModel);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(long id)
        {
            var admin = _peopleWithUserRepository.GetById(id);
            var adminModel = Mapper.Map<PeopleWithUser, AdministrativeEditModel>(admin);
            return View("DetailsEdit", adminModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(AdministrativeEditModel modelAdmin)
        {
            var myAdmin = _peopleWithUserRepository.GetById(modelAdmin.Id);
            Mapper.Map(modelAdmin, myAdmin);
            _peopleWithUserRepository.Update(myAdmin);
            const string title = "Maestro Actualizado";
            var content = "El maestro " + myAdmin.FullName + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + modelAdmin.Id);
        }
    }
}