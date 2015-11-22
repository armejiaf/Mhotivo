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
        private readonly ITutorRepository _tutorRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public StudentController(IStudentRepository studentRepository, ITutorRepository tutorRepository,
            IContactInformationRepository contactInformationRepository)
        {
            _studentRepository = studentRepository;
            _tutorRepository = tutorRepository;
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
            var allStudentDisplaysModel = allStudents.Select(Mapper.Map<Student, StudentDisplayModel>).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "name_desc":
                    allStudentDisplaysModel = allStudentDisplaysModel.OrderByDescending(s => s.FullName).ToList();
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
            var student = _studentRepository.GetById(id);
            var studentModel = Mapper.Map<Student, StudentEditModel>(student);
            ViewBag.Tutor1Id = new SelectList(_tutorRepository.Query(x => x), "Id", "FullName",
                studentModel.Tutor1);
            ViewBag.Tutor2Id = new SelectList(_tutorRepository.Query(x => x), "Id", "FullName",
                studentModel.Tutor2);
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
                    Mapper.Map(modelStudent, myStudent);
                    myStudent.Photo = fileBytes ?? myStudent.Photo;
                    _studentRepository.Update(myStudent);
                    const string title = "Estudiante Actualizado";
                    var content = "El estudiante " + myStudent.FullName + " ha sido actualizado exitosamente.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Tutor1Id = new SelectList(_tutorRepository.Query(x => x), "Id", "FullName",
                        modelStudent.Tutor1);
                    ViewBag.Tutor2Id = new SelectList(_tutorRepository.Query(x => x), "Id", "FullName",
                        modelStudent.Tutor2);
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
                            Id = id,
                            Controller = "Student"
                        };
            return View("ContactAdd", model);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            ViewBag.Tutor1Id = new SelectList(_tutorRepository.Query(x => x), "Id", "FullName",0);
            ViewBag.Tutor2Id = new SelectList(_tutorRepository.Query(x => x), "Id", "FullName",0);
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(StudentRegisterModel modelStudent)
        {
            var studentModel = Mapper.Map<StudentRegisterModel, Student>(modelStudent);
            _studentRepository.Create(studentModel);
            const string title = "Estudiante Agregado";
            var content = "El estudiante " + studentModel.FullName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Details(long id)
        {
            var student = _studentRepository.GetById(id);
            var studentModel = Mapper.Map<Student, StudentDisplayModel>(student);
            return View("Details", studentModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Details(StudentDisplayModel modelStudent)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(long id)
        {
            var student = _studentRepository.GetById(id);
            var studentModel = Mapper.Map<Student, StudentEditModel>(student);
            ViewBag.Tutor1Id = new SelectList(_tutorRepository.Query(x => x), "Id", "FullName",studentModel.Tutor1);
            ViewBag.Tutor2Id = new SelectList(_tutorRepository.Query(x => x), "Id", "FullName", studentModel.Tutor2);
            return View("DetailsEdit", studentModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult DetailsEdit(StudentEditModel modelStudent)
        {
            var myStudent = _studentRepository.GetById(modelStudent.Id);
            Mapper.Map(modelStudent, myStudent);
            _studentRepository.Update(myStudent);
            const string title = "Estudiante Actualizado";
            var content = "El estudiante " + myStudent.FullName + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Details/" + modelStudent.Id);
        }
    }
}