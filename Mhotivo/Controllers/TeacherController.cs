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
        private readonly IRoleRepository _roleRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public TeacherController(ITeacherRepository meisterRepository,
            IContactInformationRepository contactInformationRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _teacherRepository = meisterRepository;
            _contactInformationRepository = contactInformationRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

         [AuthorizeAdmin]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            return View(_teacherRepository.GetAllTeachers());
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
                Controller = "Meister"
            };
            return View("ContactEdit", contactInformation);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            var meister = _teacherRepository.GetTeacherEditModelById(id);
            Mapper.CreateMap<TeacherEditModel, Teacher>().ReverseMap();
            var meisterModel = Mapper.Map<Teacher, TeacherEditModel>(meister);
            meisterModel.StrGender = Implement.Utilities.GenderToString(meister.Gender).Substring(0, 1);
            return View("Edit", meisterModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(TeacherEditModel modelMeister)
        {
            var validImageTypes = new []
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };
            if (modelMeister.UpladPhoto != null && modelMeister.UpladPhoto.ContentLength > 0)
            {
                if (!validImageTypes.Contains(modelMeister.UpladPhoto.ContentType))
                {
                    ModelState.AddModelError("UpladPhoto", "Por favor seleccione entre una imagen GIF, JPG o PNG");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    byte[] fileBytes = null;
                    if (modelMeister.UpladPhoto != null)
                    {
                        using (var binaryReader = new BinaryReader(modelMeister.UpladPhoto.InputStream))
                        {
                            fileBytes = binaryReader.ReadBytes(modelMeister.UpladPhoto.ContentLength);
                        }
                    }
                    var myMeister = _teacherRepository.GetById(modelMeister.Id);
                    Mapper.CreateMap<Teacher, TeacherEditModel>().ReverseMap();
                    var meisterModel = Mapper.Map<TeacherEditModel, Teacher>(modelMeister);
                    meisterModel.Gender = Implement.Utilities.IsMasculino(modelMeister.StrGender);
                    meisterModel.Photo = null;
                    meisterModel.Photo = fileBytes ?? myMeister.Photo;
                    _teacherRepository.UpdateTeacherFromMeisterEditModel(meisterModel, myMeister);
                    const string title = "Maestro Actualizado";
                    var content = "El maestro " + myMeister.FullName + " ha sido actualizado exitosamente.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(modelMeister);
                }
            }
            return View(modelMeister);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            Teacher meister = _teacherRepository.Delete(id);
            const string title = "Maestro Eliminado";
            var content = "El maestro " + meister.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult ContactAdd(long id)
        {
            var model = new ContactInformationRegisterModel
                        {
                            Id = (int) id,
                            Controller = "Meister"
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
            Mapper.CreateMap<Teacher, TeacherRegisterModel>().ReverseMap();
            var teacherModel = Mapper.Map<TeacherRegisterModel, Teacher>(modelTeacher);
            var myTeacher = _teacherRepository.GenerateTeacherFromRegisterModel(teacherModel);
            if (_teacherRepository.ExistIdNumber(modelTeacher.IdNumber))
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "Ya existe el numero de Identidad ya existe", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            if (_teacherRepository.ExistEmail(modelTeacher.Email))
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var newUser = new User
            {
                DisplayName = modelTeacher.FirstName,
                Email = modelTeacher.Email,
                Password = modelTeacher.Password,
                IsActive = true
            };
            newUser = _userRepository.Create(newUser, _roleRepository.Filter(x => x.Name == "Maestro").FirstOrDefault());
            myTeacher.MyUser = newUser;
            _teacherRepository.Create(myTeacher);
            const string title = "Maestro Agregado";
            var content = "El maestro " + myTeacher.FullName + "ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Details(long id)
        {
            var meister = _teacherRepository.GetTeacherDisplayModelById(id);
            Mapper.CreateMap<DisplayTeacherModel, Teacher>().ReverseMap();
            var meisterModel = Mapper.Map<Teacher, DisplayTeacherModel>(meister);
            return View("Details", meisterModel);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(long id)
        {
            var meister = _teacherRepository.GetTeacherEditModelById(id);
            Mapper.CreateMap<TeacherEditModel, Teacher>().ReverseMap();
            var meisterModel = Mapper.Map<Teacher, TeacherEditModel>(meister);
            return View("DetailsEdit", meisterModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(TeacherEditModel modelMeister)
        {
            var myMeister = _teacherRepository.GetById(modelMeister.Id);
            Mapper.CreateMap<Teacher, TeacherEditModel>().ReverseMap();
            var meisterModel = Mapper.Map<TeacherEditModel, Teacher>(modelMeister);
            _teacherRepository.UpdateTeacherFromMeisterEditModel(meisterModel, myMeister);
            const string title = "Maestro Actualizado";
            var content = "El maestro " + myMeister.FullName + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + modelMeister.Id);
        }
    }
}