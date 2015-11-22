using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
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

        public TutorController(ITutorRepository tutorRepository,
            IContactInformationRepository contactInformationRepository,
            IUserRepository userRepository, IPasswordGenerationService passwordGenerationService,
            IRoleRepository roleRepository)
        {
            _tutorRepository = tutorRepository;
            _contactInformationRepository = contactInformationRepository;
            _userRepository = userRepository;
            _passwordGenerationService = passwordGenerationService;
            _roleRepository = roleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

         [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var allTutors = _tutorRepository.GetAllTutors();
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
            if (!string.IsNullOrEmpty(searchString))
            {
                allTutors = _tutorRepository.Filter(x => x.FullName.Contains(searchString)).ToList();
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
            tutorModel.MyGender = tutor.MyGender.ToString("G").Substring(0, 1);
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
                try
                {
                    byte[] fileBytes = null;
                    if (modelTutor.UploadPhoto != null)
                    {
                        using (var binaryReader = new BinaryReader(modelTutor.UploadPhoto.InputStream))
                        {
                            fileBytes = binaryReader.ReadBytes(modelTutor.UploadPhoto.ContentLength);
                        }
                    }
                    var myTutor = _tutorRepository.GetById(modelTutor.Id);
                    Mapper.Map(modelTutor, myTutor);
                    myTutor.Photo = fileBytes ?? myTutor.Photo;
                    _tutorRepository.Update(myTutor);
                    const string title = "Tutor o Tutor Actualizado";
                    var content = "El Tutor o Tutor " + myTutor.FullName + " ha sido actualizado exitosamente.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(modelTutor);
                }
            }
            return View(modelTutor);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            Tutor tutor = _tutorRepository.Delete(id);
            const string title = "Tutor o Tutor Eliminado";
            var content = "El Tutor o Tutor " + tutor.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
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
            return View(modelRegister);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Create(TutorRegisterModel modelTutor)
        {
            var tutorModel = Mapper.Map<TutorRegisterModel, Tutor>(modelTutor);
            if (_tutorRepository.Filter(x => x.IdNumber == modelTutor.IdNumber).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "Ya existe el numero de Identidad ya existe", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            if (_tutorRepository.Filter(x => x.User.Email == modelTutor.Email).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
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
             _tutorRepository.Create(tutorModel);
            const string title = "Tutor o Tutor Agregado";
            var content = "El Tutor o Tutor " + tutorModel.FullName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

         [AuthorizeAdmin]
        public ActionResult Details(long id)
        {
            var tutor = _tutorRepository.GetById(id);
            var tutorModel = Mapper.Map<Tutor, TutorDisplayModel>(tutor);
            tutorModel.MyGender = tutor.MyGender.ToString("G");
            return View("Details", tutorModel);
        }

         [AuthorizeAdmin]
        public ActionResult DetailsEdit(long id)
        {
            var tutor = _tutorRepository.GetById(id);
            var tutorModel = Mapper.Map<Tutor, TutorEditModel>(tutor);
            tutorModel.MyGender = tutor.MyGender.ToString("G");
            return View("DetailsEdit", tutorModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(TutorEditModel modelTutor)
        {
            var myTutor = _tutorRepository.GetById(modelTutor.Id);
            Mapper.Map(modelTutor, myTutor);
            _tutorRepository.Update(myTutor);
            const string title = "Tutor o Tutor Actualizado";
            var content = "El Tutor o Tutor " + myTutor.FullName + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + modelTutor.Id);
        }
    }
}