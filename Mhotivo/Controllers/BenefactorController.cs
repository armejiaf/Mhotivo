using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using System.Web.Mvc;

namespace Mhotivo.Controllers
{
    public class BenefactorController : Controller
    {
        private readonly IBenefactorRepository _benefactorRepository;
        private readonly IContactInformationRepository _contactInformationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public BenefactorController(IBenefactorRepository benefactorRepository, IStudentRepository studentRepository,
            IContactInformationRepository contactInformationRepository)
        {
            _benefactorRepository = benefactorRepository;
            _studentRepository = studentRepository;
            _contactInformationRepository = contactInformationRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            return View(_benefactorRepository.GettAllBenefactors());
        }

        [HttpGet]
        public ActionResult ContactEdit(long id)
        {
            ContactInformation thisContactInformation = _contactInformationRepository.GetById(id);
            var contactInformation = new ContactInformationEditModel
            {
                Type = thisContactInformation.Type,
                Value = thisContactInformation.Value,
                Id = thisContactInformation.Id,
                People = thisContactInformation.People,
                Controller = "Benefactor"
            };
            return View("ContactEdit", contactInformation);
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            Mapper.CreateMap<BenefactorEditModel, Benefactor>().ReverseMap();
            var editBenefactor = Mapper.Map<Benefactor, BenefactorEditModel>(_benefactorRepository.GetBenefactorEditModelById(id));
            editBenefactor.StrGender = Implement.Utilities.GenderToString(editBenefactor.Gender).Substring(0, 1);
            return View("Edit", editBenefactor);
        }

        [HttpPost]
        public ActionResult Edit(BenefactorEditModel modelBenefactor)
        {
            if (modelBenefactor.Capacity < modelBenefactor.StudentsCount)
            {
                var title = "Beneficiario No Puede Tener Menos de " + modelBenefactor.StudentsCount;
                const string content = "Elimine algunos estudiantes antes de continuar.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                return RedirectToAction("Index");
            }
            else
            {
                var myBenefactor = _benefactorRepository.GetById(modelBenefactor.Id);
                modelBenefactor.Gender = Implement.Utilities.IsMasculino(modelBenefactor.StrGender);
                Mapper.CreateMap<Benefactor, BenefactorEditModel>().ReverseMap();
                var editBenefactor = Mapper.Map<BenefactorEditModel, Benefactor>(modelBenefactor);
                _benefactorRepository.UpdateBenefactorFromBenefactorEditModel(editBenefactor, myBenefactor);
                const string title = "Beneficiario Actualizado";
                string content = "El Beneficiario " + myBenefactor.FullName + " ha sido actualizado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            Benefactor benefactor = _benefactorRepository.Delete(id);
            const string title = "Benefactor Eliminado";
            string content = "El benefactor " + benefactor.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ContactAdd(long id)
        {
            var model = new ContactInformationRegisterModel
            {
                Id = (int)id,
                Controller = "Benefactor"
            };
            return View("ContactAdd", model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("Create");
        }

        [HttpPost]
        public ActionResult Add(BenefactorRegisterModel modelBenefactor)
        {
            modelBenefactor.Gender = Implement.Utilities.IsMasculino(modelBenefactor.StrGender);
            Mapper.CreateMap<Benefactor, BenefactorRegisterModel>().ReverseMap();
            var regBenefactor = Mapper.Map<BenefactorRegisterModel, Benefactor>(modelBenefactor);
            var myBenefactor = _benefactorRepository.GenerateBenefactorFromRegisterModel(regBenefactor);
            myBenefactor.Capacity = modelBenefactor.Capacity;
            _benefactorRepository.Create(myBenefactor);
            const string title = "Benefactor Agregado";
            string content = "El Benefactor " + myBenefactor.FullName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(long id)
        {
            Benefactor benefactor = _benefactorRepository.GetBenefactorDisplayModelById(id);
            Mapper.CreateMap<DisplayBenefactorModel, Benefactor>().ReverseMap();
            DisplayBenefactorModel modelBenefactor = Mapper.Map<Benefactor, DisplayBenefactorModel>(benefactor);
            return View("Details", modelBenefactor);
        }

        [HttpGet]
        public ActionResult DetailsEdit(long id)
        {
            Benefactor benefactor = _benefactorRepository.GetBenefactorEditModelById(id);
            Mapper.CreateMap<BenefactorEditModel, Benefactor>().ReverseMap();
            BenefactorEditModel modelBenefactor = Mapper.Map<Benefactor, BenefactorEditModel>(benefactor);
            return View("DetailsEdit", modelBenefactor);
        }

        [HttpPost]
        public ActionResult DetailsEdit(BenefactorEditModel modelBenefactor)
        {
            if (modelBenefactor.StudentsCount > modelBenefactor.Capacity)
            {
                string title = "Beneficiario No Puede Tener Menos de " + modelBenefactor.StudentsCount;
                const string content = "Elimine algunos estudiantes antes de continuar.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                return RedirectToAction("DetailsEdit/" + modelBenefactor.Id);
            }
            else
            {
                Benefactor myBenefactor = _benefactorRepository.GetById(modelBenefactor.Id);
                Mapper.CreateMap<Benefactor, BenefactorEditModel>().ReverseMap();
                Benefactor editlBenefactor = Mapper.Map<BenefactorEditModel, Benefactor>(modelBenefactor);
                _benefactorRepository.UpdateBenefactorFromBenefactorEditModel(editlBenefactor, myBenefactor);
                const string title = "Beneficiario Actualizado";
                string content = "El Beneficiario " + myBenefactor.FullName + " ha sido actualizado exitosamente.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                return RedirectToAction("Details/" + modelBenefactor.Id);
            }
        }

        [HttpGet]
        public ActionResult StudentEdit(long id)
        {
            Student thisStudent = _studentRepository.GetById(id);
            var student = new StudentBenefactorEditModel
            {
                OldId = (int)id,
                Id = thisStudent.Benefactor == null ? -1 : thisStudent.Benefactor.Id
            };
            ViewBag.NewID = new SelectList(_studentRepository.Query(x => x), "Id", "FullName", student.OldId);
            return View("StudentEdit", student);
        }

        [HttpGet]
        public ActionResult StudentAdd(long id)
        {
            var student = new StudentBenefactorEditModel
            {
                Id = (int)id
            };
            ViewBag.NewID = new SelectList(_studentRepository.Query(x => x), "Id", "FullName");
            return View("StudentAdd", student);
        }

        [HttpPost]
        public ActionResult StudentAdd(StudentBenefactorEditModel modelStudent)
        {
            Benefactor benefactor = _benefactorRepository.GetById(modelStudent.Id);
            if (benefactor != null)
            {
                if (benefactor.Capacity > benefactor.Students.Count)
                {
                    Student myStudent = _studentRepository.GetById(modelStudent.NewId);
                    myStudent.Benefactor = benefactor;
                    _studentRepository.Update(myStudent);
                }
            }
            return RedirectToAction("Details/" + modelStudent.Id);
        }

        [HttpPost]
        public ActionResult StudentEdit(StudentBenefactorEditModel modelStudent)
        {
            if (modelStudent.NewId <= 0)
            {
                Student myStudent = _studentRepository.GetById(modelStudent.OldId);
                myStudent.Benefactor = null;
                _studentRepository.Update(myStudent);
                _studentRepository.GetById(modelStudent.NewId);
            }
            else if (modelStudent.OldId != modelStudent.NewId)
            {
                Student myStudent = _studentRepository.GetById(modelStudent.NewId);
                if (myStudent.Benefactor == null || myStudent.Benefactor.Id != modelStudent.Id)
                {
                    myStudent.Benefactor = _benefactorRepository.GetById(modelStudent.Id);
                    _studentRepository.Update(myStudent);
                    myStudent = _studentRepository.GetById(modelStudent.OldId);
                    myStudent.Benefactor = null;
                    _studentRepository.Update(myStudent);
                }
            }
            return RedirectToAction("Details/" + modelStudent.Id);
        }

        [HttpPost]
        public ActionResult DeleteStudent(long id)
        {
            Student myStudent = _studentRepository.GetById(id);
            long ID = myStudent.Benefactor.Id;
            myStudent.Benefactor = null;
            _studentRepository.Update(myStudent);
            const string title = "Estudiante Eliminado";
            string content = "El estudiante " + myStudent.FullName + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + ID);
        }
    }
}