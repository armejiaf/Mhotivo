using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.WebPages;
using AutoMapper;
using Mhotivo.Authorizations;
using Mhotivo.Logic;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;


namespace Mhotivo.Controllers
{
    public class NotificationController : Controller
    {
        const int General = 1;
        const int Area = 2;
        const int Grado = 3;
        const int Personal = 4;
        public MhotivoContext Db = new MhotivoContext(); //DEPENDENCIES. 
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly ISessionManagementRepository _sessionManagement;
        private readonly IGradeRepository _gradeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPeopleRepository _peopleRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAcademicYearRepository _academicYear;
        private readonly IAcademicYearDetailsRepository _academicYearDetailRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationTypeRepository _notificationTypeRepository;
        private readonly IEnrollRepository _enrollRepository;
        private readonly IEducationLevelRepository _areaReporsitory;
        private static string _searchText = string.Empty;

        public NotificationController(ISessionManagementRepository sessionManagement, IUserRepository userRepository, INotificationRepository notificationRepository, INotificationTypeRepository notificationTypeRepository, IPeopleRepository peopleRepository, ITeacherRepository teacherRepository,
            IAcademicYearDetailsRepository academicYearDetailRepository, IStudentRepository studentRepository, IParentRepository parentRepository, IGradeRepository gradeRepository, IAcademicYearRepository academicYearRepository, IEnrollRepository enrollRepository, IEducationLevelRepository areaReporsitory)
        {
            _sessionManagement = sessionManagement;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _notificationTypeRepository = notificationTypeRepository;
            _peopleRepository = peopleRepository;
            _teacherRepository = teacherRepository;
            _academicYearDetailRepository = academicYearDetailRepository;
            _parentRepository = parentRepository;
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
            _academicYear = academicYearRepository;
            _enrollRepository = enrollRepository;
            _areaReporsitory = areaReporsitory;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        private void LoadTypeNotification(ref NotificationModel model)
        {
            var items = _notificationTypeRepository.Query(c => c).Select(c => new SelectListItem
            {
                Text = c.Description,
                Value = c.Id.ToString()
            }).ToList();


            model.NotificationTypeSelectList = new SelectList(items, "Value", "Text", model.NotificationTypeId);
            if (model.NotificationTypeId == Personal)
            {
                var list = GetListOpcionTypeNotification(model.NotificationTypeId.ToString(CultureInfo.InvariantCulture));
                model.NotificationTypeOpionSelectList = new SelectList(list, "Value", "Text",
                    model.GradeIdifNotificationTypePersonal);
                IEnumerable<SelectListItem> listStudent = LoadListStudent(model.NotificationTypeId == Personal ? Convert.ToInt32(model.IdIsGradeAreaGeneralSelected) : 0);
                model.StudentOptionSelectList = new SelectList(listStudent, "Value", "Text", model.IdIsGradeAreaGeneralSelected);
            }
            else
            {
                var list = GetListOpcionTypeNotification(model.NotificationTypeId.ToString(CultureInfo.InvariantCulture));
                model.NotificationTypeOpionSelectList = new SelectList(list, "Value", "Text",
                    model.IdIsGradeAreaGeneralSelected);
                var lis2 = new List<SelectListItem> {new SelectListItem {Value = "0", Text = "N/A"}};
                model.StudentOptionSelectList = new SelectList(lis2, "Value", "Text", model.IdIsGradeAreaGeneralSelected);
            }
        }


        private IEnumerable<SelectListItem> LoadListStudent(int idGrade)
        {
            var list = new List<SelectListItem>();
            if (idGrade != 0)
            {
                var grade = _gradeRepository.GetById(idGrade);
                var academyYear = _academicYear.Query(x => x).Select(x => x).Include("Grade").FirstOrDefault(x => x.Grade.Id.Equals(grade.Id));
                var query =
                    from a in Db.Enrolls
                    join b in Db.Students on a.Student.Id equals b.Id
                    where a.AcademicYear.Id == academyYear.Id
                    select new
                    {
                        Field1 = a.Student.FullName,
                        Field2 = a.Student.Id
                    };
                try
                {
                    if (query.Any())
                    {
                        list = query.Select(c => new SelectListItem
                        {
                            Text = c.Field1,
                            Value = c.Field2.ToString()
                        }).ToList();
                    }
                }
                catch (TargetException)
                {
                    ;//silently catch an exception and do nothing with it? Let's find out why... *throws*
                    throw;
                }
            }
            if (list.Count <= 0)
            {
                list.Add(new SelectListItem { Value = "0", Text = "N/A" });
            }
            return list;
        }

        // GET: /NotificationModel/

        public ActionResult Index(string searchName)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var user =
                _userRepository.GetAllUsers()
                    .FirstOrDefault(x => x.Email == _sessionManagement.GetUserLoggedEmail());

            var notifications = _notificationRepository.Query(x => x).ToList();

            if (!_sessionManagement.GetUserLoggedRole().Equals("Administrador"))
                notifications = notifications.FindAll(x => user != null && x.UserCreatorId == user.Id);

            if (searchName != null)
                notifications = notifications.ToList().FindAll(x => x.NotificationName == searchName);

            var notificationsModel = notifications.Select(Mapper.Map<NotificationModel>);
            return View(notificationsModel);
        }


        // GET: /NotificationModel/Create
        [HttpGet]
        public ActionResult Add()
        {
            var notification = new NotificationModel();
            LoadTypeNotification(ref notification);
            ViewBag.Section = new SelectList(new List<string> { "Todos", "A", "B", "C" }, "Todos");
            return View("Add", notification);
        }

        [HttpPost]
        public ActionResult Add(NotificationModel eventNotification, string addressedTo)
        {
            var notificationIdentity = Mapper.Map<Notification>(eventNotification);
            notificationIdentity.Created = DateTime.Now;
            var notificationType = _notificationTypeRepository.GetById(eventNotification.NotificationTypeId);
            notificationIdentity.NotificationType = notificationType;
            notificationIdentity.Section = eventNotification.Section;
            if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.Id == Personal)
            {
                notificationIdentity.IdGradeAreaUserGeneralSelected = eventNotification.StudentId;
                notificationIdentity.GradeIdifNotificationTypePersonal = Convert.ToInt32(eventNotification.IdIsGradeAreaGeneralSelected);
                notificationIdentity.TargetStudent = _studentRepository.GetById(eventNotification.StudentId);
            }
            else if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.Id == Area)
            {
                notificationIdentity.IdGradeAreaUserGeneralSelected = Convert.ToInt32(eventNotification.IdIsGradeAreaGeneralSelected);
                notificationIdentity.GradeIdifNotificationTypePersonal = Convert.ToInt32(eventNotification.IdIsGradeAreaGeneralSelected);
            }
            if (addressedTo.IsEmpty() || addressedTo == null)
                addressedTo = "0";
            notificationIdentity.GradeIdifNotificationTypePersonal = Convert.ToInt32(addressedTo);
            notificationIdentity.Users = new List<User>();
            if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.Id == Personal)
            {
                AddUsersToPersonalNotification(notificationIdentity);
            }
            if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.Id == Grado)
            {
                AddUsersToGradeNotification(notificationIdentity);
            }
            if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.Id == Area)
            {
                AddUsersToLevelNotification(notificationIdentity);
            }
            var user =
                _userRepository.GetAllUsers()
                    .FirstOrDefault(x => x.Email == _sessionManagement.GetUserLoggedEmail());
            if (user != null)
            {
                notificationIdentity.UserCreatorId = user.Id;
                notificationIdentity.UserCreatorName = user.DisplayName;
            }
            notificationIdentity.Approved = false;
            if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.Id == Personal)
            {
                notificationIdentity.Approved = false;
            }

            if (notificationIdentity.NotificationType != null &&
                _sessionManagement.GetUserLoggedRole().Equals("Administrador"))
                notificationIdentity.Approved = true;

            var email = _sessionManagement.GetUserLoggedEmail();
            notificationIdentity.NotificationCreator =
                _userRepository.FirstOrDefault(x => x.Email.Equals(email));
            _notificationRepository.Create(notificationIdentity);
            const string title = "Notificación Agregado";
            var content = "El evento " + eventNotification.NotificationName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        private void AddUsersToLevelNotification(Notification notificationIdentity)
        {
           
            var area = _areaReporsitory.GetById(notificationIdentity.IdGradeAreaUserGeneralSelected);
            var grades = _gradeRepository.GetAllGrade().Where(x => x.EducationLevel.Equals(area.Name)).Select(x => x.Id).ToList();
            if (!grades.Any())
            {
                return;
            }
            foreach (var id in grades)
            {
                long id1 = id;
                var studentList = _enrollRepository.GetAllsEnrolls().Where(x => x.AcademicYear.Grade.Id == id1
                                                        && x.AcademicYear.Year == DateTime.Now.Year)
                                                        .Select(x => x.Student).ToList();
                if (!studentList.Any())
                {
                    continue;
                }
                foreach (var student in studentList)
                {
                    var student1 = student;
                    var notificationParentId = _studentRepository.Query(x => x).Where(x => x.Id == student1.Id
                                                                        && x.Tutor1 != null)
                                                                        .Select(x => x.Tutor1).Include(x => x.MyUser)
                                                                        .FirstOrDefault();
                    if (notificationParentId != null)
                    {
                        if (notificationParentId.MyUser != null)
                        {
                            notificationIdentity.Users.Add(notificationParentId.MyUser);
                        }
                    }
                    notificationParentId = _studentRepository.GetAllStudents().Where(x => x.Id == student.Id
                                                                && x.Tutor2 != null && x.Tutor2 != x.Tutor1)
                                                                .Select(x => x.Tutor2).FirstOrDefault();
                    if (notificationParentId != null)
                    {
                        User parents = _userRepository.GetById(notificationParentId.MyUser.Id);
                        if (parents != null)
                        {
                            notificationIdentity.Users.Add(parents);
                        }
                    }
                }
            }
        }

        private void AddUsersToGradeNotification(Notification notificationIdentity)
        {
            var estudiantes = _enrollRepository.Query(x => x).Where(x => x.AcademicYear.Grade.Id == notificationIdentity.IdGradeAreaUserGeneralSelected
                                       && x.AcademicYear.Year == DateTime.Now.Year).Select(s => s.Student).ToList();
            foreach (var estudiante in estudiantes)
            {
                if (estudiante.Tutor1.MyUser != null)
                {
                    var parents = _userRepository.GetById(estudiante.Tutor1.MyUser.Id);
                    if (parents != null)
                    {
                        notificationIdentity.Users.Add(parents);
                        var parents2 = _userRepository.GetById(estudiante.Tutor2.MyUser.Id);
                        if (parents2 != null && parents2.Id != parents.Id)
                        {
                            notificationIdentity.Users.Add(parents2);
                        }
                    }
                }
            }
        }

        private void AddUsersToPersonalNotification(Notification notificationIdentity)
        {
            var notificationParentId =
                _studentRepository.Filter(x => x.Id == notificationIdentity.IdGradeAreaUserGeneralSelected).Where(x => x.Tutor1 != null).Select(x => x.Tutor1).FirstOrDefault();
            if (notificationParentId != null)
            {
                var parents = _parentRepository.GetById(notificationParentId.Id);
                var user = parents.MyUser;
                if (user != null)
                {
                    notificationIdentity.Users.Add(user);
                    SendEmail.SendEmailToUsers(notificationIdentity.Users.ToList(),
                        "Se ha creado una notificacion en la cual usted ha sido incluido.Mensaje: " +
                        notificationIdentity.Message, "Notificacion creada");
                }
            }
        }

        public JsonResult OptiontList(string id)
        {
            var list = GetListOpcionTypeNotification(id);
            return Json(new SelectList(list, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListStudent(string id)
        {
            var list = LoadListStudent(Convert.ToInt32(id));
            return Json(new SelectList(list, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable GetListOpcionTypeNotification(string id)
        {
            var list = new List<SelectListItem>();
            //uh, no.
            switch (id)
            {
                case "1":
                    list.Add(new SelectListItem { Value = "0", Text = "N/A" });
                    break;
                case "2":
                    list = _areaReporsitory.Query(x => x).Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();
                    break;
                case "3":
                    if (_sessionManagement.GetUserLoggedRole().Equals("Administrador"))
                    {
                        list = _gradeRepository.Query(x => x).Select(c => new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.Id.ToString()
                        }).ToList();
                    }
                    else
                    {
                        var email = _sessionManagement.GetUserLoggedEmail();
                        var teacher = _teacherRepository.FirstOrDefault(x => x.MyUser.Email.Equals(email));
                        var academicYears = _academicYearDetailRepository.GetAllAcademicYear(teacher.Id);
                        var gradesTeacherGiveClasssesTo = new List<Grade>();

                        foreach (
                            var year in academicYears.Where(year => !gradesTeacherGiveClasssesTo.Contains(year.Grade)))
                        {
                            gradesTeacherGiveClasssesTo.Add(year.Grade);
                        }

                        list = gradesTeacherGiveClasssesTo.Select(c => new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.Id.ToString()
                        }).ToList();
                    }
                    break;

                case "4":
                    var user =
                        _userRepository.GetAllUsers()
                            .FirstOrDefault(x => x.Email == _sessionManagement.GetUserLoggedEmail());
                    var listGrade = new List<Grade>();
                    if (user != null)
                    {
                        var people = _peopleRepository.GetAllPeopleByUserId(user.Id).FirstOrDefault(x => x is Teacher);
                        if (people != null)
                        {
                            var meiser = _teacherRepository.GetById(people.Id);
                            if (meiser != null)
                            {
                                var academyYear = _academicYearDetailRepository.GetAllAcademicYear(meiser.Id).ToList();

                                foreach (var year in academyYear.Where(year => !listGrade.Contains(year.Grade)))
                                {
                                    listGrade.Add(year.Grade);
                                }
                            }
                            else
                            {
                                list = _gradeRepository.GetAllGrade().Select(c => new SelectListItem
                                {
                                    Text = c.Name,
                                    Value = c.Id.ToString()
                                }).ToList();
                                break;
                            }
                        }
                        else
                        {
                            list = _gradeRepository.GetAllGrade().Select(c => new SelectListItem
                            {
                                Text = c.Name,
                                Value = c.Id.ToString()
                            }).ToList();
                            break;
                        }
                    }
                    var allgrade = _gradeRepository.GetAllGrade().Select(x => x).ToList();
                    var query = from c in allgrade
                                join d in listGrade on c.Id equals d.Id
                                select new SelectListItem
                                {
                                    Text = c.Name,
                                    Value = d.Id.ToString()
                                };
                    list = query.ToList();
                    break;
            }
            return list;
        }

        public JsonResult GetGroupsAndEmails(string filter)
        {
            List<string> mails =
                Db.Users.Where(x => x.DisplayName.Contains(filter) || x.Email.Contains(filter))
                    .Select(x => x.Email)
                    .ToList();
            return Json(mails, JsonRequestBehavior.AllowGet);
        }

        // GET: /NotificationModel/Edit/5
        public ActionResult Edit(int id)
        {
            var toEdit = _notificationRepository.Query(x => x).Include("NotificationType").FirstOrDefault(x => x.Id.Equals(id));
            var toEditModel = Mapper.Map<NotificationModel>(toEdit);
            if (toEdit != null)
            {
                if (toEdit.NotificationType != null)
                    toEditModel.NotificationTypeId = toEdit.NotificationType.Id;
                if (toEdit.NotificationType != null && toEdit.NotificationType.Id == Personal)
                {
                    toEditModel.StudentId = toEdit.IdGradeAreaUserGeneralSelected;
                    toEditModel.IdIsGradeAreaGeneralSelected = toEdit.GradeIdifNotificationTypePersonal.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    toEditModel.IdIsGradeAreaGeneralSelected = toEdit.IdGradeAreaUserGeneralSelected.ToString(CultureInfo.InvariantCulture);
                }
            }
            LoadTypeNotification(ref toEditModel);
            return View(toEditModel);
        }

        // POST: /NotificationModel/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, NotificationModel eventNotification)
        {
            try
            {
                var toEdit = _notificationRepository.Query(x => x).Include("NotificationType").FirstOrDefault(x => x.Id.Equals(id));
                if (toEdit != null)
                {
                    var notificationBeforeEdit = _notificationRepository.GetById(toEdit.Id);
                    var notificationType = _notificationTypeRepository.First(c => c.Id == eventNotification.NotificationTypeId);
                    var receiverChanged = false;
                    toEdit.NotificationType = notificationType;
                    toEdit.NotificationName = eventNotification.NotificationName;
                    toEdit.Message = eventNotification.Message;
                    
                    if (toEdit.NotificationType != null && toEdit.NotificationType.Id == Personal)
                    {
                        toEdit.IdGradeAreaUserGeneralSelected = eventNotification.StudentId;
                        toEdit.GradeIdifNotificationTypePersonal = Convert.ToInt32(eventNotification.IdIsGradeAreaGeneralSelected);
                    }
                    else
                    {
                        toEdit.IdGradeAreaUserGeneralSelected = Convert.ToInt32(eventNotification.IdIsGradeAreaGeneralSelected);
                        toEdit.GradeIdifNotificationTypePersonal = 0;
                    }
                    if (toEdit.NotificationType != null && toEdit.NotificationType.Id == notificationBeforeEdit.NotificationType.Id)
                    {
                        //Something is weird here. Other than the empty ifs, I mean.

                            receiverChanged = true;
                    }
                    else
                    {
                        receiverChanged = true;
                    }
                    if (receiverChanged)
                    {
                        toEdit.Users = new List<User>();
                        if (toEdit.NotificationType != null && toEdit.NotificationType.Id == Personal)
                        {
                            AddUsersToPersonalNotification(toEdit);
                        }
                        if (toEdit.NotificationType != null && toEdit.NotificationType.Id == Grado)
                        {
                            AddUsersToGradeNotification(toEdit);
                        }
                        if (toEdit.NotificationType != null && toEdit.NotificationType.Id == Area)
                        {
                            AddUsersToLevelNotification(toEdit);
                        }
                    }
                }
                _notificationRepository.Update(toEdit);
                _notificationRepository.SaveChanges();
                _viewMessageLogic.SetNewMessage("Notificación Editada", "La notificación fue editada exitosamente.",
                    ViewMessageType.SuccessMessage);
            }
            catch
            {
                _viewMessageLogic.SetNewMessage("Error en edición",
                    "La notificación no pudo ser editada correctamente, por favor intente nuevamente.",
                    ViewMessageType.ErrorMessage);
            }
            return RedirectToAction("Index");
        }

        // POST: /NotificationModel/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Notification toDelete = _notificationRepository.GetById(id);
                _notificationRepository.Delete(toDelete);
                _notificationRepository.SaveChanges();
                IQueryable<long> notifications = _notificationRepository.Query(x => x).Select(x => x.Id);
                return RedirectToAction("Index", notifications);
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: /NotificationModel/
        [AllowAnonymous]
        public ActionResult Approve()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var notifications = _notificationRepository.Query(x => x).Include(c => c.NotificationType).Where(x => x.Approved == false)
                    .OrderByDescending(i => i.Created)
                    .Take(10);
            var notificationsModel = notifications.Select(Mapper.Map<NotificationModel>);
            return View("Approve", notificationsModel);
        }

        [HttpPost]
        public ActionResult Approve(int id)
        {
            try
            {
                var toApprove = _notificationRepository.GetById(id);
                if (toApprove != null)
                {
                    toApprove.Approved = true;
                    _notificationRepository.Update(toApprove);
                    _notificationRepository.SaveChanges();
                    _viewMessageLogic.SetNewMessage("Notificación Aprobada", "La notificación fue aprobada exitosamente.",
                    ViewMessageType.SuccessMessage);
                }
            }
            catch
            {
                _viewMessageLogic.SetNewMessage("Error en aprobacion",
                    "La notificación no pudo ser aprobada correctamente, por favor intente nuevamente.",
                    ViewMessageType.ErrorMessage);
            }
            return RedirectToAction("Approve");
        }
    }
}