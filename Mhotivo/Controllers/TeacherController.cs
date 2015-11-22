using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using AutoMapper;
using Mhotivo.Authorizations;

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

        public TeacherController(ITeacherRepository teacherRepository, IContactInformationRepository contactInformationRepository, 
            IUserRepository userRepository, IRoleRepository roleRepository, IPasswordGenerationService passwordGenerationService)
        {
            _teacherRepository = teacherRepository;
            _contactInformationRepository = contactInformationRepository;
            _userRepository = userRepository;
            _passwordGenerationService = passwordGenerationService;
            _roleRepository = roleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

         [AuthorizeAdmin]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            return View(Mapper.Map<IEnumerable<Teacher>, IEnumerable<TeacherDisplayModel> >(_teacherRepository.GetAllTeachers()));
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
                Controller = "Teacher"
            };
            return View("ContactEdit", contactInformation);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            var teacher = _teacherRepository.GetById(id);
            var teacherModel = Mapper.Map<Teacher, TeacherEditModel>(teacher);
            return View("Edit", teacherModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
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
                try
                {
                    byte[] fileBytes = null;
                    if (modelTeacher.UploadPhoto != null)
                    {
                        using (var binaryReader = new BinaryReader(modelTeacher.UploadPhoto.InputStream))
                        {
                            fileBytes = binaryReader.ReadBytes(modelTeacher.UploadPhoto.ContentLength);
                        }
                    }
                    var myTeacher = _teacherRepository.GetById(modelTeacher.Id);
                    Mapper.Map(modelTeacher, myTeacher);
                    myTeacher.Photo = fileBytes ?? myTeacher.Photo;
                    _teacherRepository.Update(myTeacher);
                    const string title = "Maestro Actualizado";
                    var content = "El maestro " + myTeacher.FullName + " ha sido actualizado exitosamente.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(modelTeacher);
                }
            }
            return View(modelTeacher);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            Teacher teacher = _teacherRepository.Delete(id);
            const string title = "Maestro Eliminado";
            var content = "El maestro " + teacher.FullName + " ha sido eliminado exitosamente.";
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
                            Controller = "Teacher"
                        };
            return View("ContactAdd", model);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(TeacherRegisterModel modelTeacher)
        {
            var teacherModel = Mapper.Map<TeacherRegisterModel, Teacher>(modelTeacher);
            if (_teacherRepository.Filter(x => x.IdNumber == modelTeacher.IdNumber).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "Ya existe un maestro con ese numero de Identidad", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            if (_teacherRepository.Filter(x => x.User.Email == modelTeacher.Email).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }

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
            _teacherRepository.Create(teacherModel);
            const string title = "Maestro Agregado";
            var content = "El maestro " + teacherModel.FullName + "ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Details(long id)
        {
            var teacher = _teacherRepository.GetById(id);
            var teacherModel = Mapper.Map<Teacher, TeacherDisplayModel>(teacher);
            return View("Details", teacherModel);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(long id)
        {
            var teacher = _teacherRepository.GetById(id);
            var teacherModel = Mapper.Map<Teacher, TeacherEditModel>(teacher);
            return View("DetailsEdit", teacherModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(TeacherEditModel modelTeacher)
        {
            var myTeacher = _teacherRepository.GetById(modelTeacher.Id);
            Mapper.Map(modelTeacher, myTeacher);
            _teacherRepository.Update(myTeacher);
            const string title = "Maestro Actualizado";
            var content = "El maestro " + myTeacher.FullName + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + modelTeacher.Id);
        }
    }
}