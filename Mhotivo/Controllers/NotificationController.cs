using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Mvc;
//using Mhotivo.App_Data;
using System.Web.WebPages;
using AutoMapper;
using Mhotivo.Logic;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Repositories;
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

        public MhotivoContext db = new MhotivoContext();
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly ISessionManagement _sessionManagement;
        private readonly IUserRepository _userRepository;
        private readonly IMeisterRepository _meisterRepository;
        private readonly IPeopleRepository _peopleRepository;
        private readonly IAcademicYearDetailRepository _academicYearDetailRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationTypeRepository _notificationTypeRepository;
        private static string searchText = string.Empty;
        
        //private readonly IGradeRepository _gradeRepository;
        //private readonly IAreaReporsitory _areaReporsitory;

        private void LoadTypeNotification(ref NotificationModel model)
        {
           
            var items = db.NotificationTypes.Select(c => new SelectListItem()
            {
                Text = c.TypeDescription,
                Value = c.NotificationTypeId.ToString()
            }).ToList();
            model.NotificationTypeSelectList = new SelectList(items, "Value", "Text", model.NotificationTypeId);


            if (model.NotificationTypeId == Personal)
            {
                var list = GetListOpcionTypeNotification(model.NotificationTypeId.ToString(CultureInfo.InvariantCulture));
                model.NotificationTypeOpionSelectList = new SelectList(list, "Value", "Text",
                    model.GradeIdifNotificationTypePersonal);


                IEnumerable<SelectListItem> listStudent = LoadListStudent(model.NotificationTypeId == Personal ? model.IdIsGradeAreaGeneralSelected : 0);
                model.StudentOptionSelectList = new SelectList(listStudent, "Value", "Text", model.IdIsGradeAreaGeneralSelected);
            }
            else
            {
                var list = GetListOpcionTypeNotification(model.NotificationTypeId.ToString(CultureInfo.InvariantCulture));
                model.NotificationTypeOpionSelectList = new SelectList(list, "Value", "Text",
                    model.IdIsGradeAreaGeneralSelected);

                var lis2 = new List<SelectListItem>();
                lis2.Add(new SelectListItem() { Value = "0", Text = "N/A" });
                model.StudentOptionSelectList = new SelectList(lis2, "Value", "Text", model.IdIsGradeAreaGeneralSelected);
            }

        }

        
        private IEnumerable<SelectListItem> LoadListStudent(int idGrade)
        {
            var list = new List<SelectListItem>();
            if (idGrade != 0)
            {
                var grade = db.Grades.FirstOrDefault(g => g.Id == idGrade);
                var academyYear =
                    db.AcademicYears.Select(x => x).Include("Grade").FirstOrDefault(x => x.Grade.Id.Equals(grade.Id));

                var query =
                    from a in db.Enrolls
                    join b in db.Students on a.Student.Id equals b.Id
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
                        list = query.Select(c => new SelectListItem()
                        {
                            Text = c.Field1,
                            Value = c.Field2.ToString()
                        }).ToList();
                    }
                }
                catch (TargetException ex)
                { }
            }
            if (list.Count <= 0)
            {
                list.Add(new SelectListItem() { Value = "0", Text = "N/A" });
            }

            return list;
        }


        public NotificationController(ISessionManagement sessionManagement, IUserRepository userRepository, INotificationRepository notificationRepository, INotificationTypeRepository notificationTypeRepository,IPeopleRepository peopleRepository,IMeisterRepository meisterRepository,IAcademicYearDetailRepository academicYearDetailRepository)
        {
            _sessionManagement = sessionManagement;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _notificationTypeRepository = notificationTypeRepository;
            _peopleRepository = peopleRepository;
            _meisterRepository = meisterRepository;
            _academicYearDetailRepository = academicYearDetailRepository;
            //_gradeRepository = gradeRepository;
            //_areaReporsitory = areaReporsitory;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        //
        // GET: /NotificationModel/
        [AllowAnonymous]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();

            var user =
                _userRepository.GetAllUsers()
                    .FirstOrDefault(x => x.Email == _sessionManagement.GetUserLoggedEmail());

            var notifications =
                db.Notifications.Include(c => c.NotificationCreator).Where(x => x.UserCreatorId== user.Id)//cuando es personal
                    .OrderByDescending(i => i.Created)
                    .Take(10);

            var notificationsModel = notifications.Select(Mapper.Map<NotificationModel>);

            return View(notificationsModel);
        }

        //
        // GET: /NotificationModel/
        [HttpPost]
        public ActionResult Index(FormCollection text)
        {
            searchText = text["Name"];
            if (string.IsNullOrEmpty(searchText))
            {
                var notifications =
                    db.Notifications.Where(x => true)
                        .OrderByDescending(i => i.Created)
                        .Take(10);


                var notificationsModel = notifications.Select(Mapper.Map<NotificationModel>);

                return View("Index", notificationsModel);
            }
            else
            {
                var notifications =
                    db.Notifications.Where(x => x.NotificationName.Contains(searchText.Trim()))
                        .OrderByDescending(i => i.Created).Take(10);
                var notificationsModel = notifications.Select(Mapper.Map<NotificationModel>);

                return View("Index", notificationsModel);
            }
        }

        //
        // GET: /NotificationModel/Create
        [HttpGet]
        public ActionResult Add()
        {
            var notification = new NotificationModel();
            LoadTypeNotification(ref notification);

            return View("Add", notification);
        }

        [HttpPost]
        public ActionResult Add(NotificationModel eventNotification,string AddressedTo)
        {
            var notificationIdentity = Mapper.Map<Notification>(eventNotification);
            notificationIdentity.Created = DateTime.Now;

            var notificationType =
                db.NotificationTypes.FirstOrDefault(c => c.NotificationTypeId == eventNotification.NotificationTypeId);
            notificationIdentity.NotificationType = notificationType;


            //notificationIdentity.StudentId = eventNotification.StudentId;

            if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.NotificationTypeId == Personal)
            {
                notificationIdentity.IdGradeAreaUserGeneralSelected = eventNotification.StudentId;
                notificationIdentity.GradeIdifNotificationTypePersonal = eventNotification.IdIsGradeAreaGeneralSelected;
            }
            else
            {
                notificationIdentity.IdGradeAreaUserGeneralSelected = eventNotification.IdIsGradeAreaGeneralSelected;
                notificationIdentity.GradeIdifNotificationTypePersonal = eventNotification.IdIsGradeAreaGeneralSelected;
            }
            if (AddressedTo.IsEmpty() || AddressedTo == null)
                AddressedTo = "0";

            notificationIdentity.GradeIdifNotificationTypePersonal = Convert.ToInt32(AddressedTo);

            notificationIdentity.Users = new List<User>();

            if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.NotificationTypeId == Personal)
            {
                AddUsersToPersonalNotification(notificationIdentity);
            }

            if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.NotificationTypeId == Grado)
            {
                AddUsersToGradeNotification(notificationIdentity);
            }

            if (notificationIdentity.NotificationType != null && notificationIdentity.NotificationType.NotificationTypeId == Area)
            {
                AddUsersToLevelNotification(notificationIdentity);
            }

            //if (AddressedTo.IsEmpty() || AddressedTo == null)
            //    AddressedTo = "0";


            var user =
                _userRepository.GetAllUsers()
                    .FirstOrDefault(x => x.Email == _sessionManagement.GetUserLoggedEmail());

            if (user != null) notificationIdentity.UserCreatorId = user.Id;

            db.Notifications.Add(notificationIdentity);
            db.SaveChanges();
            const string title = "Notificación Agregado";
            var content = "El evento " + eventNotification.NotificationName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);

            return RedirectToAction("Index");
        }

        private void AddUsersToLevelNotification(Notification notificationIdentity)
        {
            var grades = new List<int>();
            if (notificationIdentity.IdGradeAreaUserGeneralSelected == General)
            {
                grades = db.Grades.Where(x => x.EducationLevel == "Kinder").Select(x => x.Id).ToList();
            }
            else if (notificationIdentity.IdGradeAreaUserGeneralSelected == Area)
            {
                grades = db.Grades.Where(x => x.EducationLevel == "Primaria").Select(x => x.Id).ToList();
            }
            else if (notificationIdentity.IdGradeAreaUserGeneralSelected == Grado)
            {
                grades = db.Grades.Where(x => x.EducationLevel == "Secundaria").Select(x => x.Id).ToList();
            }

            if (!grades.Any())
            {
                return;
            }

            foreach (var id in grades)
            {
                int id1 = id;
                var studentList = db.Enrolls.Where(x => x.AcademicYear.Grade.Id == id1
                                                        && x.AcademicYear.Year.Year == DateTime.Now.Year)
                                                        .Select(x => x.Student).ToList();

                if (!studentList.Any())
                {
                    continue;
                }

                foreach (var student in studentList)
                {
                    var notificationParentId = db.Students.Where(x => x.Id == student.Id
                                                                        && x.Tutor1 != null)
                                                                        .Select(x => x.Tutor1)
                                                                        .FirstOrDefault();

                    if (notificationParentId != null)
                    {
                        var parents = db.Users.FirstOrDefault(x => x.Id == notificationParentId.UserId.Id);
                        if (parents != null)
                        {
                            notificationIdentity.Users.Add(parents);
                        }

                    }

                    notificationParentId = db.Students.Where(x => x.Id == student.Id
                                                                && x.Tutor2 != null && x.Tutor2 != x.Tutor1)
                                                                .Select(x => x.Tutor2).FirstOrDefault();

                    if (notificationParentId != null)
                    {
                        User parents = db.Users.FirstOrDefault(x => x.Id == notificationParentId.UserId.Id);
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
           var estudiantes =
                db.Enrolls.Where(x => x.AcademicYear.Grade.Id == notificationIdentity.IdGradeAreaUserGeneralSelected
                                      && x.AcademicYear.Year.Year == DateTime.Now.Year
                                      && x.Student.Tutor1 != null).Select(s => s.Student).ToList();

            foreach (var estudiante in estudiantes)
            {
                var parents = db.Users.FirstOrDefault(x => x.Id == estudiante.Tutor1.UserId.Id);
                if (parents != null)
                {
                    notificationIdentity.Users.Add(parents);

                    var parents2 = db.Users.FirstOrDefault(x => x.Id == estudiante.Tutor2.UserId.Id);
                    if (parents2 != null && parents2.Id != parents.Id)
                    {
                        notificationIdentity.Users.Add(parents2);
                    }
                }
            }


        }

        private void AddUsersToPersonalNotification(Notification notificationIdentity)
        {
            //var notificationParentId = db.Students.Include(c=>c.User).Where(x => x.Id == notificationIdentity.IdGradeAreaUserGeneralSelected
            //                                                && x.Tutor1 != null).Select(x => x.Tutor1).FirstOrDefault();
            //if (notificationParentId != null)
            //{
            //    var parents = db.Users.FirstOrDefault(x => x.Id == notificationParentId.UserId.Id);

            //    if (parents != null)
            //    {
            //        notificationIdentity.Users.Add(parents);
            //    }

            //}

            //notificationParentId = db.Students.Where(x => x.Id == notificationIdentity.IdGradeAreaUserGeneralSelected
            //                                                && x.Tutor2 != null && x.Tutor2 != x.Tutor1).Select
            //                                                (x => x.Tutor1).FirstOrDefault();

            //if (notificationParentId != null)
            //{
            //    var parents = db.Users.FirstOrDefault(x => x.Id == notificationParentId.UserId.Id);

            //    if (parents != null)
            //    {
            //        notificationIdentity.Users.Add(parents);
            //    }
            //}

        }

        public JsonResult OptiontList(string Id)
        {
            var list = GetListOpcionTypeNotification(Id);
            return Json(new SelectList(list, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListStudent(string Id)
        {

            var list = LoadListStudent(Convert.ToInt32(Id));
            return Json(new SelectList(list, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable GetListOpcionTypeNotification(string id)
        {
            var list = new List<SelectListItem>();
            switch (id)
            {
                case "1":
                    list.Add(new SelectListItem() {Value = "0", Text = "N/A"});
                    break;
                case "2":
                    list = db.Areas.Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();
                    break;
                case "3":
                case "4":

                    var user =
                        _userRepository.GetAllUsers()
                            .FirstOrDefault(x => x.Email == _sessionManagement.GetUserLoggedEmail());
                    var listGrade = new List<Grade>();
                    if (user != null)
                    {
                        var people = _peopleRepository.GetPeopleUser(user.Id);
                        if (people != null)
                        {
                            var meiser = _meisterRepository.GetById(people.Id);

                            if (meiser != null)
                            {
                                var academyYear = _academicYearDetailRepository.GetAllAcademicYear(meiser.Id);
                                listGrade.AddRange(
                                    academyYear.Select(
                                        item =>
                                            new Grade()
                                            {
                                                EducationLevel = item.Grade.EducationLevel,
                                                Id = item.Id,
                                                Name = item.Grade.Name
                                            }));
                            }
                            else
                            {
                                list = db.Grades.Select(c => new SelectListItem()
                                {
                                    Text = c.Name,
                                    Value = c.Id.ToString()
                                }).ToList();
                                break;
                            }
                        }
                        else
                        {
                            list = db.Grades.Select(c => new SelectListItem()
                            {
                                Text = c.Name,
                                Value = c.Id.ToString()
                            }).ToList();
                            break;
                        }
                    }

                      var allgrade = db.Grades.Select(x => x).ToList();

                     var query = from c in allgrade
                                join d in listGrade on c.Id equals d.Id
                                 select new SelectListItem {
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
            List<string> groups = db.Groups.Where(x => x.Name.Contains(filter)).Select(x => x.Name).ToList();
            List<string> mails =
                db.Users.Where(x => x.DisplayName.Contains(filter) || x.Email.Contains(filter))
                    .Select(x => x.Email)
                    .ToList();
            groups = groups.Union(mails).ToList();
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /NotificationModel/Edit/5

        public ActionResult Edit(int id)
        {
            var toEdit = _notificationRepository.Query(x => x).Include("NotificationType").FirstOrDefault(x => x.Id.Equals(id));
            //var toEdit = db.Notifications.FirstOrDefault(x => x.Id.Equals(id));
            var toEditModel = Mapper.Map<NotificationModel>(toEdit);
            if (toEdit != null)
            {
                if (toEdit.NotificationType != null)
                    toEditModel.NotificationTypeId = toEdit.NotificationType.NotificationTypeId;

               
                if (toEdit.NotificationType != null && toEdit.NotificationType.NotificationTypeId == Personal)
                {
                    toEditModel.StudentId = toEdit.IdGradeAreaUserGeneralSelected;
                    toEditModel.IdIsGradeAreaGeneralSelected = toEdit.GradeIdifNotificationTypePersonal;
                }
                else
                {
                    toEditModel.IdIsGradeAreaGeneralSelected = toEdit.IdGradeAreaUserGeneralSelected;
                }
                
            }
            LoadTypeNotification(ref toEditModel);

            return View(toEditModel);
        }

        //
        // POST: /NotificationModel/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, NotificationModel eventNotification)
        {
            try
            {
                var toEdit = _notificationRepository.Query(x => x).Include("NotificationType").FirstOrDefault(x => x.Id.Equals(id));

                var notificationBeforeEdit = _notificationRepository.GetById(toEdit.Id);

                var notificationType = _notificationTypeRepository.First(c => c.NotificationTypeId == eventNotification.NotificationTypeId);

                var receiverChanged = false;

                if (toEdit != null)
                {
                    toEdit.NotificationType = notificationType;
                    toEdit.NotificationName = eventNotification.NotificationName;
                    toEdit.Message = eventNotification.Message;


                   
                    if (toEdit.NotificationType != null && toEdit.NotificationType.NotificationTypeId == Personal)
                    {
                        toEdit.IdGradeAreaUserGeneralSelected = eventNotification.StudentId;
                        toEdit.GradeIdifNotificationTypePersonal = eventNotification.IdIsGradeAreaGeneralSelected;
                    }
                    else
                    {
                        toEdit.IdGradeAreaUserGeneralSelected = eventNotification.IdIsGradeAreaGeneralSelected;
                        toEdit.GradeIdifNotificationTypePersonal = 0;
                    }
                }

                if (toEdit.NotificationType != null && toEdit.NotificationType.NotificationTypeId == notificationBeforeEdit.NotificationType.NotificationTypeId)
                {
                    if (toEdit.NotificationType.NotificationTypeId == Personal && toEdit.StudentId == notificationBeforeEdit.StudentId)
                    {
                        receiverChanged = false;

                    }
                    else if (toEdit.NotificationType.NotificationTypeId != Personal && toEdit.IdGradeAreaUserGeneralSelected == notificationBeforeEdit.IdGradeAreaUserGeneralSelected)
                    {
                        receiverChanged = false;
                    }
                    else
                    {
                        receiverChanged = true;
                    }

                }
                else
                {
                    receiverChanged = true;
                }

                if (receiverChanged) 
                {
                    toEdit.Users  = new List<User>();

                    if (toEdit.NotificationType != null && toEdit.NotificationType.NotificationTypeId == Personal)
                    {
                        AddUsersToPersonalNotification(toEdit);
                    }

                    if (toEdit.NotificationType != null && toEdit.NotificationType.NotificationTypeId == Grado)
                    {
                        AddUsersToGradeNotification(toEdit);
                    }

                    if (toEdit.NotificationType != null && toEdit.NotificationType.NotificationTypeId == Area)
                    {
                        AddUsersToLevelNotification(toEdit);
                    }
                }

                _notificationRepository.Update(toEdit);
                _notificationRepository.SaveChanges();
                _viewMessageLogic.SetNewMessage("Notificación Editada", "La notificación fue editada exitosamente.",
                    ViewMessageType.SuccessMessage);
                //}
            }
            catch
            {
                _viewMessageLogic.SetNewMessage("Error en edición",
                    "La notificación no pudo ser editada correctamente, por favor intente nuevamente.",
                    ViewMessageType.ErrorMessage);
            }
            IQueryable<Group> g = db.Groups.Select(x => x);
            return RedirectToAction("Index", g);
        }


        //
        // POST: /NotificationModel/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Notification toDelete = db.Notifications.FirstOrDefault(x => x.Id.Equals(id));
                db.Notifications.Remove(toDelete);
                db.SaveChanges();
                IQueryable<long> notifications = db.Notifications.Select(x => x.Id);
                return RedirectToAction("Index", notifications);
            }
            catch
            {
                return View("Index");
            }
        }


        //
        // GET: /NotificationModel/
        [AllowAnonymous]
        public ActionResult Approve()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            
            var notifications =
                db.Notifications.Include(c=>c.NotificationType).Where(x => x.Approved == false && x.NotificationType.NotificationTypeId==Personal)
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
            IQueryable<Group> g = db.Groups.Select(x => x);
            return RedirectToAction("Approve");
        }


        //public bool UserCanApprove()
        //{
        //    //var loggedUser = 
        //    return true;
        //}

        public bool SendEmailForGeneralNotifications(AcademicYear currentAcademicYear)
        {
            var currentYear = currentAcademicYear.Year;
            var generalNotifications = db.Notifications.Where(n => false);

            foreach (var notification in generalNotifications)
            {


            }


            return false;

        }

    }
}