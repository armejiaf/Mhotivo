using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
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
    public class TutorController : Controller
    {
        private readonly IContactInformationRepository _contactInformationRepository;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly ITutorRepository _tutorRepository;
        private readonly IUserRepository _userRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IRoleRepository _roleRepository;
        private readonly IPeopleWithUserRepository _peopleWithUserRepository;
        private readonly IStudentRepository _studentRepository;

        public TutorController(ITutorRepository tutorRepository,
            IContactInformationRepository contactInformationRepository,
            IUserRepository userRepository, IPasswordGenerationService passwordGenerationService,
            IRoleRepository roleRepository, IPeopleWithUserRepository peopleWithUserRepository, IStudentRepository studentRepository)
        {
            _tutorRepository = tutorRepository;
            _contactInformationRepository = contactInformationRepository;
            _userRepository = userRepository;
            _passwordGenerationService = passwordGenerationService;
            _roleRepository = roleRepository;
            _peopleWithUserRepository = peopleWithUserRepository;
            _studentRepository = studentRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

         [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var allTutors = _tutorRepository.Filter(x => x.User.IsActive).ToList();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.IdNumberSortParm = sortOrder == "IdNumber" ? "idNumber_desc" : "IdNumber";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                allTutors = _tutorRepository.Filter(x => x.User.IsActive && (x.FullName.Contains(searchString) || x.IdNumber.Contains(searchString))).ToList();
            }
            var allTutorDisplaysModel = allTutors.Select(Mapper.Map<Tutor, TutorDisplayModel>).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "name_desc":
                    allTutorDisplaysModel = allTutorDisplaysModel.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "IdNumber":
                    allTutorDisplaysModel = allTutorDisplaysModel.OrderBy(s => s.IdNumber).ToList();
                    break;
                case "idNumber_desc":
                    allTutorDisplaysModel = allTutorDisplaysModel.OrderByDescending(s => s.IdNumber).ToList();
                    break;
                default:  // Name ascending 
                    allTutorDisplaysModel = allTutorDisplaysModel.OrderBy(s => s.FullName).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(allTutorDisplaysModel.ToPagedList(pageNumber, pageSize));
        }

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
                Controller = "Tutor"
            };
            return View("ContactEdit", contactInformation);
        }

         [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            var tutor = _tutorRepository.GetById(id);
            var tutorModel = Mapper.Map<Tutor, TutorEditModel>(tutor);
            var items = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
            {
                Text = c.GetEnumDescription(),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.Genders = new List<SelectListItem>(items);
            return View("Edit", tutorModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(TutorEditModel modelTutor)
        {
            var validImageTypes = new []
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };
            if (modelTutor.UploadPhoto != null && modelTutor.UploadPhoto.ContentLength > 0)
            {
                if (!validImageTypes.Contains(modelTutor.UploadPhoto.ContentType))
                {
                    ModelState.AddModelError("UploadPhoto", "Por favor seleccione entre una imagen GIF, JPG o PNG");
                }
            }
            if (ModelState.IsValid)
            {
                if (
                    _tutorRepository.Filter(x => x.IdNumber.Equals(modelTutor.IdNumber) && x.Id != modelTutor.Id)
                        .Any())
                {
                    const string title = "Error!";
                    const string content = "Ya existe un tutor con ese numero de identidad.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                    return RedirectToAction("Index");
                }
                try
                {
                    if (modelTutor.UploadPhoto != null)
                    {
                        using (var binaryReader = new BinaryReader(modelTutor.UploadPhoto.InputStream))
                        {
                            modelTutor.Photo = binaryReader.ReadBytes(modelTutor.UploadPhoto.ContentLength);
                        }
                    }
                    var myTutor = _tutorRepository.GetById(modelTutor.Id);
                    Mapper.Map(modelTutor, myTutor);
                    _tutorRepository.Update(myTutor);
                    const string title = "Tutor Actualizado";
                    var content = "El Tutor " + myTutor.FullName + " ha sido actualizado exitosamente.";
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
                    return View(modelTutor);
                }
            }
            var items2 = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
            {
                Text = c.GetEnumDescription(),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.Genders = new List<SelectListItem>(items2);
            return View(modelTutor);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            if (_studentRepository.Filter(x => x.Tutor1.Id == id || x.Tutor2.Id == id).Any())
            {
                const string title2 = "Error";
                const string content2 = "El tutor tiene hijos y no puede borrarse.";
                _viewMessageLogic.SetNewMessage(title2, content2, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var tutor = _tutorRepository.Delete(id);
            const string title = "Tutor Eliminado";
            var content = "El Tutor " + tutor.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

         [AuthorizeAdmin]
        public ActionResult ContactAdd(long id)
        {
            var model = new ContactInformationRegisterModel
            {
                Id = id,
                Controller = "Tutor"
            };
            return View("ContactAdd", model);
        }

         [AuthorizeAdmin]
        public ActionResult Create()
        {
            var modelRegister = new TutorRegisterModel();
            var items = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
            {
                Text = c.GetEnumDescription(),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.Genders = new List<SelectListItem>(items);
            return View(modelRegister);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Create(TutorRegisterModel modelTutor)
        {
            var tutorModel = Mapper.Map<TutorRegisterModel, Tutor>(modelTutor);
            if (_tutorRepository.Filter(x => x.IdNumber == modelTutor.IdNumber).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "Ya existe un tutor con ese numero de Identidad", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            if (_peopleWithUserRepository.Filter(x => x.User.Email == modelTutor.Email).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            _tutorRepository.Create(tutorModel);
            var newUser = new User
            {
                UserOwner = tutorModel,
                Email = modelTutor.Email,
                Password = _passwordGenerationService.GenerateTemporaryPassword(),
                IsUsingDefaultPassword = true,
                IsActive = true,
                Role = _roleRepository.Filter(x => x.Name == "Tutor").FirstOrDefault()
            };
            newUser.DefaultPassword = newUser.Password;
            newUser = _userRepository.Create(newUser);
            tutorModel.User = newUser;
             _tutorRepository.Update(tutorModel);
            const string title = "Tutor Agregado";
            var content = "El Tutor " + tutorModel.FullName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

         [AuthorizeAdmin]
        public ActionResult Details(long id)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var tutor = _tutorRepository.GetById(id);
            var tutorModel = Mapper.Map<Tutor, TutorDisplayModel>(tutor);
            return View("Details", tutorModel);
        }
    }
}