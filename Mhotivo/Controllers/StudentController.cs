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
    public class StudentController : Controller
    {
        private readonly IContactInformationRepository _contactInformationRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public StudentController(IStudentRepository studentRepository, IParentRepository parentRepository,
            IContactInformationRepository contactInformationRepository)
        {
            _studentRepository = studentRepository;
            _parentRepository = parentRepository;
            _contactInformationRepository = contactInformationRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

         [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var allStudents = _studentRepository.GetAllStudents();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                allStudents = _studentRepository.Filter(x => x.FullName.Contains(searchString)).ToList();
            }

            Mapper.CreateMap<DisplayStudentModel, Student>().ReverseMap();
            var allStudentDisplaysModel = allStudents.Select(Mapper.Map<Student, DisplayStudentModel>).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "name_desc":
                    allStudentDisplaysModel = allStudentDisplaysModel.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "Date":
                    allStudentDisplaysModel = allStudentDisplaysModel.OrderBy(s => s.StartDate).ToList();
                    break;
                case "date_desc":
                    allStudentDisplaysModel = allStudentDisplaysModel.OrderByDescending(s => s.StartDate).ToList();
                    break;
                default:  // Name ascending 
                    allStudentDisplaysModel = allStudentDisplaysModel.OrderBy(s => s.FullName).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(allStudentDisplaysModel.ToPagedList(pageNumber, pageSize));
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
                                         Controller = "Student"
                                     };
            return View("ContactEdit", contactInformation);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            var student = _studentRepository.GetStudentEditModelById(id);
            Mapper.CreateMap<StudentEditModel, Student>().ReverseMap();
            var studentModel = Mapper.Map<Student, StudentEditModel>(student);
            studentModel.FirstParent = student.Tutor1.Id;
            if (student.Tutor2 != null)
                studentModel.SecondParent = student.Tutor2.Id;
            ViewBag.Tutor1Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName",
                studentModel.Tutor1.Id);
            if (studentModel.Tutor2 == null)
                studentModel.Tutor2 = new Parent();
            ViewBag.Tutor2Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName",
                studentModel.Tutor2.Id);
            studentModel.StrGender = Implement.Utilities.GenderToString(student.Gender).Substring(0, 1);
            return View("Edit", studentModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(StudentEditModel modelStudent)
        {
            var validImageTypes = new []
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };
            if (modelStudent.FilePicture != null && modelStudent.FilePicture.ContentLength > 0)
            {
                if (!validImageTypes.Contains(modelStudent.FilePicture.ContentType))
                {
                    ModelState.AddModelError("FilePicture", "Por favor seleccione entre una imagen GIF, JPG o PNG");
                }
            }
            var myStudent = _studentRepository.GetById(modelStudent.Id);
                try
                {
                    byte[] fileBytes = null;
                    if (modelStudent.FilePicture != null)
                    {
                        using (var binaryReader = new BinaryReader(modelStudent.FilePicture.InputStream))
                        {
                            fileBytes = binaryReader.ReadBytes(modelStudent.FilePicture.ContentLength);
                        }
                    }
                    if (modelStudent.Tutor1 == null)
                        modelStudent.Tutor1 = myStudent.Tutor1;
                    if (modelStudent.Tutor2 == null)
                        modelStudent.Tutor2 = myStudent.Tutor2;
                    Mapper.CreateMap<Student, StudentEditModel>().ReverseMap();
                    var studentModel = Mapper.Map<StudentEditModel, Student>(modelStudent);
                    studentModel.Gender = Implement.Utilities.IsMasculino(modelStudent.StrGender);
                    modelStudent.Photo = null;
                    studentModel.Photo = fileBytes ?? myStudent.Photo;
                    studentModel.MyUser = _parentRepository.GetById(modelStudent.Tutor1.Id).MyUser;
                    myStudent = _studentRepository.UpdateStudentFromStudentEditModel(studentModel, myStudent);
                    const string title = "Estudiante Actualizado";
                    var content = "El estudiante " + myStudent.FullName + " ha sido actualizado exitosamente.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                    return RedirectToAction("Index");
                }
                catch
                {
                    modelStudent.StrGender = Implement.Utilities.GenderToString(myStudent.Gender).Substring(0, 1);
                    modelStudent.FirstParent = myStudent.Tutor1.Id;
                    modelStudent.Tutor1 = myStudent.Tutor1;
                    modelStudent.Tutor2 = myStudent.Tutor2;
                    if (myStudent.Tutor2 != null)
                        modelStudent.SecondParent = myStudent.Tutor2.Id;
                    ViewBag.Tutor1Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName",
                        modelStudent.Tutor1.Id);
                    if (modelStudent.Tutor2 == null)
                        modelStudent.Tutor2 = new Parent();
                    ViewBag.Tutor2Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName",
                        modelStudent.Tutor2.Id);
                    return View(modelStudent);
                }
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            Student student = _studentRepository.Delete(id);
            const string title = "Estudiante Eliminado";
            var content = "El estudiante " + student.FullName + " ha sido eliminado exitosamente.";
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
                            Controller = "Student"
                        };
            return View("ContactAdd", model);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            ViewBag.Tutor1Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName",0);
            ViewBag.Tutor2Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName",0);
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(StudentRegisterModel modelStudent)
        {
            Mapper.CreateMap<Student, StudentRegisterModel>().ReverseMap();
            var studentModel = Mapper.Map<StudentRegisterModel, Student>(modelStudent);
            studentModel.Tutor1 = _parentRepository.GetById(modelStudent.FirstParent);
            studentModel.Tutor2 = _parentRepository.GetById(modelStudent.SecondParent);
            var myStudent = _studentRepository.GenerateStudentFromRegisterModel(studentModel);
            myStudent.MyUser = studentModel.Tutor1.MyUser;
            _studentRepository.Create(myStudent);
            const string title = "Estudiante Agregado";
            var content = "El estudiante " + myStudent.FullName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Details(long id)
        {
            var student = _studentRepository.GetStudentDisplayModelById(id);
            Mapper.CreateMap<DisplayStudentModel, Student>().ReverseMap();
            var studentModel = Mapper.Map<Student, DisplayStudentModel>(student);
            return View("Details", studentModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Details(DisplayStudentModel modelStudent)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(long id)
        {
            var student = _studentRepository.GetStudentEditModelById(id);
            Mapper.CreateMap<Student, StudentEditModel>().ReverseMap();
            var studentModel = Mapper.Map<Student, StudentEditModel>(student);
            ViewBag.Tutor1Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName",studentModel.Tutor1.Id);
            ViewBag.Tutor2Id = new SelectList(_parentRepository.Query(x => x), "Id", "FullName", studentModel.Tutor2.Id);
            return View("DetailsEdit", studentModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(StudentEditModel modelStudent)
        {
            var myStudent = _studentRepository.GetById(modelStudent.Id);
            Mapper.CreateMap<Student, StudentEditModel>().ReverseMap();
            modelStudent.Tutor1 = _parentRepository.GetById(modelStudent.Tutor1.Id);
            modelStudent.Tutor2 = _parentRepository.GetById(modelStudent.Tutor2.Id);
            var studentModel = Mapper.Map<StudentEditModel, Student>(modelStudent);
            _studentRepository.UpdateStudentFromStudentEditModel(studentModel, myStudent);
            const string title = "Estudiante Actualizado";
            var content = "El estudiante " + myStudent.FullName + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + modelStudent.Id);
        }
    }
}