using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using Mhotivo.Implement.Utils;
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
        private readonly IEducationLevelRepository _educationLevelRepository;

        public AdministrativeController(IContactInformationRepository contactInformationRepository, IUserRepository userRepository, 
            IRoleRepository roleRepository, IPasswordGenerationService passwordGenerationService, IPeopleWithUserRepository peopleWithUserRepository, IEducationLevelRepository educationLevelRepository)
        {
            _contactInformationRepository = contactInformationRepository;
            _userRepository = userRepository;
            _passwordGenerationService = passwordGenerationService;
            _peopleWithUserRepository = peopleWithUserRepository;
            _educationLevelRepository = educationLevelRepository;
            _roleRepository = roleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdmin]
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var admins = _peopleWithUserRepository.Filter(
                x => x.User.IsActive && (x.User.Role.Name.Equals("Administrador") || x.User.Role.Name.Equals("Director")));
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                try
                {
                    admins = _peopleWithUserRepository.Filter(
                 x => x.User.IsActive && (x.User.Role.Name.Equals("Administrador") || x.User.Role.Name.Equals("Director")) &&
                     (x.FullName.Contains(searchString) || x.User.Role.Name.Contains(searchString) || x.IdNumber.Contains(searchString)));
                }
                catch (Exception)
                {
                    admins = _peopleWithUserRepository.Filter(
                x => x.User.IsActive && (x.User.Role.Name.Equals("Administrador") || x.User.Role.Name.Equals("Director")));
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
            var thisContactInformation = _contactInformationRepository.GetById(id);
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
            var items = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
            {
                Text = c.GetEnumDescription(),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.Genders = new List<SelectListItem>(items);
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
                if (
                    _peopleWithUserRepository.Filter(x => x.IdNumber.Equals(modelAdmin.IdNumber) && x.Id != modelAdmin.Id && !x.User.Role.Name.Equals("Tutor"))
                        .Any())
                {
                    const string title = "Error!";
                    const string content = "Ya existe un docente o administrativo con ese numero de identidad.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                    return RedirectToAction("Index");
                }
                try
                {
                    if (modelAdmin.UploadPhoto != null)
                    {
                        using (var binaryReader = new BinaryReader(modelAdmin.UploadPhoto.InputStream))
                        {
                            modelAdmin.Photo = binaryReader.ReadBytes(modelAdmin.UploadPhoto.ContentLength);
                        }
                    }
                    var myAdmin = _peopleWithUserRepository.GetById(modelAdmin.Id);
                    Mapper.Map(modelAdmin, myAdmin);
                    _peopleWithUserRepository.Update(myAdmin);
                    const string title = "Administrativo Actualizado";
                    var content = "El administrativo " + myAdmin.FullName + " ha sido actualizado exitosamente.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
                    return RedirectToAction("Index");
                }
                catch
                {
                    var items = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
                    {
                        Text = c.GetEnumDescription(),
                        Value = c.ToString("D")
                    }).ToList();

                    ViewBag.Genders = new List<SelectListItem>(items);
                    return View(modelAdmin);
                }
            }
            var items2 = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
            {
                Text = c.GetEnumDescription(),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.Genders = new List<SelectListItem>(items2);
            return View(modelAdmin);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            if (_educationLevelRepository.Filter(x => x.Director.Id == id).Any())
            {
                const string title2 = "Error";
                const string content2 = "El director esta asignado a un nivel educativo y no puede borrarse.";
                _viewMessageLogic.SetNewMessage(title2, content2, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var admin = _peopleWithUserRepository.Delete(id);
            const string title = "Administrativo Eliminado";
            var content = "El administrativo " + admin.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
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
            var items = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
            {
                Text = c.GetEnumDescription(),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.Genders = new List<SelectListItem>(items);
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(AdministrativeRegisterModel modelAmin)
        {
            var adminModel = Mapper.Map<AdministrativeRegisterModel, PeopleWithUser>(modelAmin);
            if (
                    _peopleWithUserRepository.Filter(x => x.IdNumber.Equals(modelAmin.IdNumber) && !x.User.Role.Name.Equals("Tutor"))
                        .Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "Ya existe un administrativo con ese numero de Identidad", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            if (_peopleWithUserRepository.Filter(x => x.User.Email == modelAmin.Email).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            _peopleWithUserRepository.Create(adminModel);
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
            _peopleWithUserRepository.Update(adminModel);
            const string title = "Administrativo Agregado";
            var content = "El administrativo " + adminModel.FullName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Details(long id)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var admin = _peopleWithUserRepository.GetById(id);
            var adminModel = Mapper.Map<PeopleWithUser, AdministrativeDisplayModel>(admin);
            return View("Details", adminModel);
        }
    }
}