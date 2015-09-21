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
    public class ParentController : Controller
    {
        private readonly IContactInformationRepository _contactInformationRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public ParentController(IParentRepository parentRepository,
            IContactInformationRepository contactInformationRepository,
            IUserRepository userRepository)
        {
            _parentRepository = parentRepository;
            _contactInformationRepository = contactInformationRepository;
            _userRepository = userRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

         [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var allParents = _parentRepository.GetAllParents();
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
                allParents = _parentRepository.Filter(x => x.FullName.Contains(searchString)).ToList();
            }
            var allParentDisplaysModel = allParents.Select(Mapper.Map<Parent, DisplayParentModel>).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "name_desc":
                    allParentDisplaysModel = allParentDisplaysModel.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "IdNumber":
                    allParentDisplaysModel = allParentDisplaysModel.OrderBy(s => s.IdNumber).ToList();
                    break;
                case "idNumber_desc":
                    allParentDisplaysModel = allParentDisplaysModel.OrderByDescending(s => s.IdNumber).ToList();
                    break;
                default:  // Name ascending 
                    allParentDisplaysModel = allParentDisplaysModel.OrderBy(s => s.FullName).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(allParentDisplaysModel.ToPagedList(pageNumber, pageSize));
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
                                         Controller = "Parent"
                                     };
            return View("ContactEdit", contactInformation);
        }

         [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            var parent = _parentRepository.GetParentEditModelById(id);
            var parentModel = Mapper.Map<Parent, ParentEditModel>(parent);
            parentModel.MyGender = parent.MyGender.ToString("G").Substring(0, 1);
            return View("Edit", parentModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(ParentEditModel modelParent)
        {
            var validImageTypes = new []
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };
            if (modelParent.UploadPhoto != null && modelParent.UploadPhoto.ContentLength > 0)
            {
                if (!validImageTypes.Contains(modelParent.UploadPhoto.ContentType))
                {
                    ModelState.AddModelError("UploadPhoto", "Por favor seleccione entre una imagen GIF, JPG o PNG");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    byte[] fileBytes = null;
                    if (modelParent.UploadPhoto != null)
                    {
                        using (var binaryReader = new BinaryReader(modelParent.UploadPhoto.InputStream))
                        {
                            fileBytes = binaryReader.ReadBytes(modelParent.UploadPhoto.ContentLength);
                        }
                    }
                    var myParent = _parentRepository.GetById(modelParent.Id);
                    var parentModel = Mapper.Map<ParentEditModel, Parent>(modelParent);
                    parentModel.Photo = null;
                    parentModel.Photo = fileBytes ?? myParent.Photo;
                    _parentRepository.UpdateParentFromParentEditModel(parentModel, myParent);
                    const string title = "Padre o Tutor Actualizado";
                    var content = "El Padre o Tutor " + myParent.FullName + " ha sido actualizado exitosamente.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(modelParent);
                }
            }
            return View(modelParent);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            Parent parent = _parentRepository.Delete(id);
            const string title = "Padre o Tutor Eliminado";
            var content = "El Padre o Tutor " + parent.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

         [AuthorizeAdmin]
        public ActionResult ContactAdd(long id)
        {
            var model = new ContactInformationRegisterModel
                        {
                            Id = (int) id,
                            Controller = "Parent"
                        };
            return View("ContactAdd", model);
        }

         [AuthorizeAdmin]
        public ActionResult Create()
        {
            var modelRegister = new ParentRegisterModel();
            return View(modelRegister);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Create(ParentRegisterModel modelParent)
        {
            var parentModel = Mapper.Map<ParentRegisterModel, Parent>(modelParent);
            var myParent = _parentRepository.GenerateParentFromRegisterModel(parentModel);
            if (_parentRepository.ExistIdNumber(modelParent.IdNumber))
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "Ya existe el numero de Identidad ya existe", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            if (_parentRepository.ExistEmail(modelParent.Email))
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var newUser = new User
            {
                DisplayName = myParent.FirstName,
                Email = modelParent.Email,
                Password = modelParent.Password,
                IsActive = true
            };
            newUser = _userRepository.Create(newUser, Roles.Padre);
            myParent.MyUser = newUser;
             _parentRepository.Create(myParent);
            const string title = "Padre o Tutor Agregado";
            var content = "El Padre o Tutor " + myParent.FullName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

         [AuthorizeAdmin]
        public ActionResult Details(long id)
        {
            var parent = _parentRepository.GetParentDisplayModelById(id);
            var parentModel = Mapper.Map<Parent, DisplayParentModel>(parent);
            parentModel.MyGender = parent.MyGender.ToString("G");
            return View("Details", parentModel);
        }

         [AuthorizeAdmin]
        public ActionResult DetailsEdit(long id)
        {
            var parent = _parentRepository.GetParentEditModelById(id);
            var parentModel = Mapper.Map<Parent, ParentEditModel>(parent);
            parentModel.MyGender = parent.MyGender.ToString("G");
            return View("DetailsEdit", parentModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(ParentEditModel modelParent)
        {
            var myParent = _parentRepository.GetById(modelParent.Id);
            var parentModel = Mapper.Map<ParentEditModel, Parent>(modelParent);
            _parentRepository.UpdateParentFromParentEditModel(parentModel, myParent);
            const string title = "Padre o Tutor Actualizado";
            var content = "El Padre o Tutor " + myParent.FullName + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + modelParent.Id);
        }
    }
}