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
    public class TeacherController : Controller
    {
        private readonly IContactInformationRepository _contactInformationRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUserRepository _userRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly IRoleRepository _roleRepository;
        private readonly IPeopleWithUserRepository _peopleWithUserRepository;

        public TeacherController(ITeacherRepository teacherRepository, IContactInformationRepository contactInformationRepository, 
            IUserRepository userRepository, IRoleRepository roleRepository, IPasswordGenerationService passwordGenerationService, IPeopleWithUserRepository peopleWithUserRepository)
        {
            _teacherRepository = teacherRepository;
            _contactInformationRepository = contactInformationRepository;
            _userRepository = userRepository;
            _passwordGenerationService = passwordGenerationService;
            _peopleWithUserRepository = peopleWithUserRepository;
            _roleRepository = roleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdminDirector]
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
             var teachers = _teacherRepository.Filter(x => x.User.IsActive).ToList();
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                try
                {
                    teachers = _teacherRepository.Filter(x => x.User.IsActive && (x.FullName.Contains(searchString) || x.IdNumber.Contains(searchString))).ToList();
                }
                catch (Exception)
                {
                    teachers = _teacherRepository.Filter(x => x.User.IsActive).ToList();
                }
            }
            ViewBag.CurrentFilter = searchString;
            var model = Mapper.Map<IEnumerable<Teacher>, IEnumerable<TeacherDisplayModel>>(teachers);
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult ContactEdit(long id)
        {
            ContactInformation thisContactInformation = _contactInformationRepository.GetById(id);
            var contactInformation = new ContactInformationEditModel
            {
                Type = thisContactInformation.Type,
                Value = thisContactInformation.Value,
                Id = thisContactInformation.Id,
                People = thisContactInformation.People,
                Controller = "Teacher"
            };
            return View("ContactEdit", contactInformation);
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Edit(long id)
        {
            var teacher = _teacherRepository.GetById(id);
            var teacherModel = Mapper.Map<Teacher, TeacherEditModel>(teacher);
            var items = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
            {
                Text = c.GetEnumDescription(),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.Genders = new List<SelectListItem>(items);
            return View("Edit", teacherModel);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Edit(TeacherEditModel modelTeacher)
        {
            var validImageTypes = new []
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };
            if (modelTeacher.UploadPhoto != null && modelTeacher.UploadPhoto.ContentLength > 0)
            {
                if (!validImageTypes.Contains(modelTeacher.UploadPhoto.ContentType))
                {
                    ModelState.AddModelError("UploadPhoto", "Por favor seleccione entre una imagen GIF, JPG o PNG");
                }
            }
            if (ModelState.IsValid)
            {
                if (
                    _peopleWithUserRepository.Filter(x => x.IdNumber.Equals(modelTeacher.IdNumber) && x.Id != modelTeacher.Id && !x.User.Role.Name.Equals("Tutor"))
                        .Any())
                {
                    const string title = "Error!";
                    const string content = "Ya existe un docente o administrativo con ese numero de identidad.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                    return RedirectToAction("Index");
                }
                try
                {
                    if (modelTeacher.UploadPhoto != null)
                    {
                        using (var binaryReader = new BinaryReader(modelTeacher.UploadPhoto.InputStream))
                        {
                            modelTeacher.Photo = binaryReader.ReadBytes(modelTeacher.UploadPhoto.ContentLength);
                        }
                    }
                    var myTeacher = _teacherRepository.GetById(modelTeacher.Id);
                    Mapper.Map(modelTeacher, myTeacher);
                    _peopleWithUserRepository.Update(myTeacher);
                    const string title = "Maestro Actualizado";
                    var content = "El maestro " + myTeacher.FullName + " ha sido actualizado exitosamente.";
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
                    return View(modelTeacher);
                }
            }
            var items2 = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
            {
                Text = c.GetEnumDescription(),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.Genders = new List<SelectListItem>(items2);
            return View(modelTeacher);
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Delete(long id)
        {
            var teacher = _teacherRepository.GetById(id);
            if (teacher.MyCourses.Any() || teacher.MySections.Any())
            {
                const string title2 = "Error";
                const string content2 = "Maestro esta asignado a una o mas clases o secciones y no puede borrarse.";
                _viewMessageLogic.SetNewMessage(title2, content2, ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            const string title = "Maestro Eliminado";
            var content = "El maestro " + teacher.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult ContactAdd(long id)
        {
            var model = new ContactInformationRegisterModel
            {
                Id = id,
                Controller = "Teacher"
            };
            return View("ContactAdd", model);
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Add()
        {
            var items = ((Gender[])Enum.GetValues(typeof(Gender))).Select(c => new SelectListItem
            {
                Text = c.GetEnumDescription(),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.Genders = new List<SelectListItem>(items);
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdminDirector]
        public ActionResult Add(TeacherRegisterModel modelTeacher)
        {
            var teacherModel = Mapper.Map<TeacherRegisterModel, Teacher>(modelTeacher);
            if (
                    _peopleWithUserRepository.Filter(x => x.IdNumber.Equals(modelTeacher.IdNumber) && !x.User.Role.Name.Equals("Tutor"))
                        .Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "Ya existe un maestro con ese numero de Identidad", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            if (_peopleWithUserRepository.Filter(x => x.User.Email == modelTeacher.Email).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            _teacherRepository.Create(teacherModel);
            var newUser = new User
            {
                Email = modelTeacher.Email,
                Password = _passwordGenerationService.GenerateTemporaryPassword(),
                IsUsingDefaultPassword = true,
                IsActive = true,
                Role = _roleRepository.Filter(x => x.Name == "Maestro").FirstOrDefault(),
                UserOwner = teacherModel
            };
            newUser.DefaultPassword = newUser.Password;
            newUser = _userRepository.Create(newUser);
            teacherModel.User = newUser;
            _teacherRepository.Update(teacherModel);
            const string title = "Maestro Agregado";
            var content = "El maestro " + teacherModel.FullName + "ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdminDirector]
        public ActionResult Details(long id)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var teacher = _teacherRepository.GetById(id);
            var teacherModel = Mapper.Map<Teacher, TeacherDisplayModel>(teacher);
            return View("Details", teacherModel);
        }
    }
}