using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;


namespace Mhotivo.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly ISessionManagementService _sessionManagement;
        private readonly IGradeRepository _gradeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPeopleRepository _peopleRepository;
        private readonly IParentRepository _parentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IAcademicGradeRepository _academicGradeRepository;
        private readonly IAcademicCourseRepository _academicCourseRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEnrollRepository _enrollRepository;
        private readonly IEducationLevelRepository _areaReporsitory;
        private readonly INotificationHandlerService _notificationHandlerService;

        public NotificationController(ISessionManagementService sessionManagement, IUserRepository userRepository,
            INotificationRepository notificationRepository, IPeopleRepository peopleRepository,
            ITeacherRepository teacherRepository,
            IAcademicCourseRepository academicCourseRepository, IStudentRepository studentRepository,
            IParentRepository parentRepository, IGradeRepository gradeRepository,
            IAcademicYearRepository academicYearRepository, IEnrollRepository enrollRepository,
            IEducationLevelRepository areaReporsitory, INotificationHandlerService notificationHandlerService, IAcademicGradeRepository academicGradeRepository)
        {
            _sessionManagement = sessionManagement;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _peopleRepository = peopleRepository;
            _teacherRepository = teacherRepository;
            _academicCourseRepository = academicCourseRepository;
            _parentRepository = parentRepository;
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
            _academicYearRepository = academicYearRepository;
            _enrollRepository = enrollRepository;
            _areaReporsitory = areaReporsitory;
            _notificationHandlerService = notificationHandlerService;
            _academicGradeRepository = academicGradeRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        public ActionResult Index(string searchName)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var user =
                _userRepository.GetById(Convert.ToInt64(_sessionManagement.GetUserLoggedId()));
            var notifications = _notificationRepository.Query(x => x).ToList();
            if (!_sessionManagement.GetUserLoggedRole().Equals("Administrador"))
                notifications = notifications.FindAll(x => user != null && x.NotificationCreator == user);
            if (searchName != null)
                notifications = notifications.ToList().FindAll(x => x.Title == searchName);

            var notificationsModel = notifications.Select(Mapper.Map<NotificationDisplayModel>);
            return View(notificationsModel);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var notification = new NotificationRegisterModel();
            var items = ((NotificationType[])Enum.GetValues(typeof(NotificationType))).Select(c => new SelectListItem
            {
                Text = c.ToString("G"),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.NotificationTypes = new List<SelectListItem>(items);
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            ViewBag.List1 = new List<SelectListItem>(list);
            ViewBag.List2 = new List<SelectListItem>(list);
            ViewBag.DestinationList = new List<SelectListItem>(list);
            return View("Add", notification);
        }

        [HttpPost]
        public ActionResult Add(NotificationRegisterModel eventNotification)
        {
            eventNotification.NotificationCreator = Convert.ToInt64(_sessionManagement.GetUserLoggedId());
            eventNotification.AcademicYear = _academicYearRepository.GetCurrentAcademicYear().Id;
            var notificationIdentity = Mapper.Map<Notification>(eventNotification);
            notificationIdentity.Approved = _sessionManagement.GetUserLoggedRole().Equals("Administrador");
            _notificationRepository.Create(notificationIdentity);
            _notificationHandlerService.SendAllPending();
            const string title = "Notificación Agregado";
            var content = "El evento " + eventNotification.Title + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(long id)
        {
            var toEdit =
                _notificationRepository.GetById(id);
            var toEditModel = Mapper.Map<NotificationPreApproveEditModel>(toEdit);
            return View(toEditModel);
        }

        [HttpPost]
        public ActionResult Edit(long id, NotificationPreApproveEditModel eventNotificationEdit)
        {
            try
            {
                var toEdit = _notificationRepository.GetById(eventNotificationEdit.Id);
                Mapper.Map(eventNotificationEdit, toEdit);
                _notificationRepository.Update(toEdit);
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

        [HttpPost]
        public ActionResult Delete(long id)
        {
            try
            {
                _notificationRepository.Delete(id);
                IQueryable<long> notifications = _notificationRepository.Query(x => x).Select(x => x.Id);
                return RedirectToAction("Index", notifications);
            }
            catch
            {
                return View("Index");
            }
        }

        [AllowAnonymous]
        public ActionResult Approve()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var notifications = _notificationRepository.Query(x => x)
                .Where(x => x.Approved == false)
                .OrderByDescending(i => i.CreationDate)
                .Take(10);
            var notificationsModel = notifications.Select(Mapper.Map<NotificationDisplayModel>);
            return View("Approve", notificationsModel);
        }

        [HttpPost]
        public ActionResult Approve(long id)
        {
            try
            {
                var toApprove = _notificationRepository.GetById(id);
                if (toApprove != null)
                {
                    toApprove.Approved = true;
                    _notificationHandlerService.SendNotification(toApprove);
                    _notificationRepository.Update(toApprove);
                    _viewMessageLogic.SetNewMessage("Notificación Aprobada",
                        "La notificación fue aprobada exitosamente.",
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

        private NotificationSelectListsModel LoadEducationLevels(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            toReturn.EducationLevels = new SelectList(_areaReporsitory.GetAllAreas(), "Id", "Name");
            return toReturn;
        }

        private NotificationSelectListsModel LoadGrades(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            toReturn.Grades = new SelectList(_gradeRepository.GetAllGrade(), "Id", "Name");
            return toReturn;
        }

        private NotificationSelectListsModel LoadAcademicGrades(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var items = _gradeRepository.GetAllGrade().ToList();
            toReturn.Grades = new SelectList(items, "Id", "Name");
            if (items.Any())
            {
                var first = items.First();
                var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == first.Id).ToList();
                toReturn.AcademicGrades =
                    new SelectList(sList
                        , "Id", "Section");
            }
            else
            {
                toReturn.AcademicGrades = new SelectList(list, "Value", "Text");
            }
            return toReturn;
        }

        private NotificationSelectListsModel LoadAcademicCourses(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var items = _gradeRepository.GetAllGrade().ToList();
            toReturn.Grades = new SelectList(items, "Id", "Name");
            if (items.Any())
            {
                var first = items.First();
                var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == first.Id).ToList();
                toReturn.AcademicGrades =
                    new SelectList(
                        sList, "Id", "Section");
                if (sList.Any())
                {
                    var first2 = sList.First();
                    var sList2 = _academicCourseRepository.Filter(
                        x => x.AcademicGrade.Id == first2.Id).ToList();
                    toReturn.AcademicCourses = new SelectList(
                        sList2, "Id", "Course.Name");
                }
                else
                {
                    toReturn.AcademicCourses = new SelectList(list, "Value", "Text");
                }
            }
            else
            {
                toReturn.AcademicGrades = new SelectList(list, "Value", "Text");
            }
            return toReturn;
        }

        private NotificationSelectListsModel LoadStudents(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var items = _gradeRepository.GetAllGrade().ToList();
            toReturn.Grades = new SelectList(items, "Id", "Name");
            if (items.Any())
            {
                var first = items.First();
                var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == first.Id).ToList();
                toReturn.AcademicGrades =
                    new SelectList(
                        sList, "Id", "Section");
                if (sList.Any())
                {
                    var first2 = sList.First();
                    var sList2 = _studentRepository.Filter(
                        x => x.MyGrade.Id == first2.Id).ToList();
                    toReturn.Personals = new SelectList(
                        sList2, "Id", "FullName");
                }
                else
                {
                    toReturn.Personals = new SelectList(list, "Value", "Text");
                }
            }
            else
            {
                toReturn.AcademicGrades = new SelectList(list, "Value", "Text");
            }
            return toReturn;
        }

        private NotificationSelectListsModel LoadAcademicGradesFromList1(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == model.Id1).ToList();
            toReturn.AcademicGrades =
                new SelectList(
                    sList, "Id", "Section");
            return toReturn;
        }

        private NotificationSelectListsModel LoadAcademicCoursesFromList1(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == model.Id1).ToList();
            toReturn.AcademicGrades =
                new SelectList(
                    sList, "Id", "Section");
            if (sList.Any())
            {
                var first2 = sList.First();
                var sList2 = _academicCourseRepository.Filter(
                    x => x.AcademicGrade.Id == first2.Id).ToList();
                toReturn.AcademicCourses = new SelectList(
                    sList2, "Id", "Course.Name");
            }
            else
            {
                toReturn.AcademicCourses = new SelectList(list, "Value", "Text");
            }
            return toReturn;
        }

        private NotificationSelectListsModel LoadStudentsFromList1(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var sList = _academicGradeRepository.Filter(
                    x => x.Grade.Id == model.Id1).ToList();
            toReturn.AcademicGrades =
                new SelectList(
                    sList, "Id", "Section");
            if (sList.Any())
            {
                var first2 = sList.First();
                var sList2 = _studentRepository.Filter(
                    x => x.MyGrade.Id == first2.Id).ToList();
                toReturn.Personals = new SelectList(
                    sList2, "Id", "FullName");
            }
            else
            {
                toReturn.Personals = new SelectList(list, "Value", "Text");
            }
            return toReturn;
        }

        private NotificationSelectListsModel LoadAcademicCoursesFromList2(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            var sList = _academicCourseRepository.Filter(
                    x => x.AcademicGrade.Id == model.Id2).ToList();
            toReturn.AcademicCourses =
                new SelectList(
                    sList, "Id", "Course.Name");
            return toReturn;
        }

        private NotificationSelectListsModel LoadStudentsFromList2(NotificationRegisterModel model, NotificationSelectListsModel toReturn)
        {
            var sList = _studentRepository.Filter(
                    x => x.MyGrade.Id == model.Id2).ToList();
            toReturn.Personals =
                new SelectList(
                    sList, "Id", "FullName");
            return toReturn;
        }

        public JsonResult LoadFromNotificationTypeList(NotificationRegisterModel registerModel)
        {
            var list = new List<SelectListItem> {new SelectListItem {Value = "-1", Text = "N/A"}};
            var selectListsModel = new NotificationSelectListsModel
            {
                EducationLevels = new SelectList(list, "Value", "Text"),
                Grades = new SelectList(list, "Value", "Text"),
                AcademicGrades = new SelectList(list, "Value", "Text"),
                AcademicCourses = new SelectList(list, "Value", "Text"),
                Personals = new SelectList(list, "Value", "Text")
            };
            var dict = new Dictionary<NotificationType, Func<NotificationRegisterModel, NotificationSelectListsModel, NotificationSelectListsModel>>
            {
                {NotificationType.General, LoadEducationLevels},
                {NotificationType.EducationLevel, LoadEducationLevels},
                {NotificationType.Grade, LoadGrades},
                {NotificationType.Section, LoadAcademicGrades},
                {NotificationType.Course, LoadAcademicCourses},
                {NotificationType.Personal, LoadStudents}
            };
            selectListsModel = dict[registerModel.NotificationType](registerModel, selectListsModel);
            return Json(selectListsModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadFromList1(NotificationRegisterModel registerModel)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var selectListsModel = new NotificationSelectListsModel
            {
                EducationLevels = new SelectList(list, "Value", "Text"),
                Grades = new SelectList(list, "Value", "Text"),
                AcademicGrades = new SelectList(list, "Value", "Text"),
                AcademicCourses = new SelectList(list, "Value", "Text"),
                Personals = new SelectList(list, "Value", "Text")
            };
            var dict = new Dictionary<NotificationType, Func<NotificationRegisterModel, NotificationSelectListsModel, NotificationSelectListsModel>>
            {
                {NotificationType.Section, LoadAcademicGradesFromList1},
                {NotificationType.Course, LoadAcademicCoursesFromList1},
                {NotificationType.Personal, LoadStudentsFromList1}
            };
            selectListsModel = dict[registerModel.NotificationType](registerModel, selectListsModel);
            return Json(selectListsModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadFromList2(NotificationRegisterModel registerModel)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var selectListsModel = new NotificationSelectListsModel
            {
                EducationLevels = new SelectList(list, "Value", "Text"),
                Grades = new SelectList(list, "Value", "Text"),
                AcademicGrades = new SelectList(list, "Value", "Text"),
                AcademicCourses = new SelectList(list, "Value", "Text"),
                Personals = new SelectList(list, "Value", "Text")
            };
            var dict = new Dictionary<NotificationType, Func<NotificationRegisterModel, NotificationSelectListsModel, NotificationSelectListsModel>>
            {
                {NotificationType.Course, LoadAcademicCoursesFromList2},
                {NotificationType.Personal, LoadStudentsFromList2}
            };
            selectListsModel = dict[registerModel.NotificationType](registerModel, selectListsModel);
            return Json(selectListsModel, JsonRequestBehavior.AllowGet);
        }
    }
}