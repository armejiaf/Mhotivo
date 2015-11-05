using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        private readonly IAcademicYearGradeRepository _academicYearGradeRepository;
        private readonly IAcademicYearCourseRepository _academicYearCourseRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEnrollRepository _enrollRepository;
        private readonly IEducationLevelRepository _areaReporsitory;
        private readonly INotificationHandlerService _notificationHandlerService;

        public NotificationController(ISessionManagementService sessionManagement, IUserRepository userRepository,
            INotificationRepository notificationRepository, IPeopleRepository peopleRepository,
            ITeacherRepository teacherRepository,
            IAcademicYearCourseRepository academicYearCourseRepository, IStudentRepository studentRepository,
            IParentRepository parentRepository, IGradeRepository gradeRepository,
            IAcademicYearRepository academicYearRepository, IEnrollRepository enrollRepository,
            IEducationLevelRepository areaReporsitory, INotificationHandlerService notificationHandlerService, IAcademicYearGradeRepository academicYearGradeRepository)
        {
            _sessionManagement = sessionManagement;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _peopleRepository = peopleRepository;
            _teacherRepository = teacherRepository;
            _academicYearCourseRepository = academicYearCourseRepository;
            _parentRepository = parentRepository;
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
            _academicYearRepository = academicYearRepository;
            _enrollRepository = enrollRepository;
            _areaReporsitory = areaReporsitory;
            _notificationHandlerService = notificationHandlerService;
            _academicYearGradeRepository = academicYearGradeRepository;
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

            var notificationsModel = notifications.Select(Mapper.Map<NotificationModel>);
            return View(notificationsModel);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var notification = new NotificationMainModel();
            var items = ((NotificationType[])Enum.GetValues(typeof(NotificationType))).Select(c => new SelectListItem
            {
                Text = c.ToString("G"),
                Value = c.ToString("D")
            }).ToList();

            ViewBag.NotificationTypes = new SelectList(items, "Value", "Text", notification.NotificationType);
            return View("Add", notification);
        }

        [HttpPost]
        public ActionResult Add(NotificationMainModel eventNotification)
        {
            var notificationIdentity = Mapper.Map<Notification>(eventNotification);
            var user = _userRepository.Filter(x => x.Email == _sessionManagement.GetUserLoggedEmail()).FirstOrDefault();
            notificationIdentity.NotificationCreator = user;
            notificationIdentity.AcademicYear = _academicYearRepository.GetCurrentAcademicYear();
            notificationIdentity.Approved = _sessionManagement.GetUserLoggedRole().Equals("Administrador");
            _notificationRepository.Create(notificationIdentity);
            const string title = "Notificación Agregado";
            var content = "El evento " + eventNotification.Title + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(long id)
        {
            var toEdit =
                _notificationRepository.GetById(id);
            var toEditModel = Mapper.Map<NotificationEditModel>(toEdit);
            return View(toEditModel);
        }

        [HttpPost]
        public ActionResult Edit(long id, NotificationModel eventNotification)
        {
            try
            {
                var toEdit = _notificationRepository.GetById(eventNotification.Id);
                Mapper.Map(eventNotification, toEdit);
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
                .Include(c => c.NotificationType)
                .Where(x => x.Approved == false)
                .OrderByDescending(i => i.CreationDate)
                .Take(10);
            var notificationsModel = notifications.Select(Mapper.Map<NotificationModel>);
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

        private SelectListNotificationRegisterModel LoadEducationLevels(SelectListNotificationRegisterModel model)
        {
            var toReturn = model as EducationLevelNotificationRegisterModel;
            if (toReturn == null) return model;
            toReturn.EducationLevelSelectList = new SelectList(_areaReporsitory.GetAllAreas(), "Id", "Name",
                model.DestinationId);
            return toReturn;
        }

        private SelectListNotificationRegisterModel LoadGrades(SelectListNotificationRegisterModel model)
        {
            var toReturn = model as GradeNotificationRegisterModel;
            if (toReturn == null) return model;
            toReturn.GradeSelectList = new SelectList(_gradeRepository.GetAllGrade(), "Id", "Name", model.DestinationId);
            return toReturn;
        }

        private SelectListNotificationRegisterModel LoadAcademicGrades(SelectListNotificationRegisterModel model)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var toReturn = model as AcademicGradeNotificationRegisterModel;
            if (toReturn == null) return model;
            toReturn.GradeSelectList = new SelectList(_gradeRepository.GetAllGrade(), "Id", "Name", toReturn.GradeId);
            if (toReturn.GradeId != -1)
            {
                toReturn.AcademicGradeSelectList =
                    new SelectList(_academicYearGradeRepository.Filter(x => x.Grade.Id == toReturn.GradeId), "Id",
                        "Name", model.DestinationId);
            }
            else
            {
                toReturn.AcademicGradeSelectList = new SelectList(list, "Value", "Text");
            }
            return toReturn;
        }

        private SelectListNotificationRegisterModel LoadAcademicCourses(SelectListNotificationRegisterModel model)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var toReturn = model as AcademicCourseNotificationRegisterModel;
            if (toReturn == null) return model;
            toReturn.GradeSelectList = new SelectList(_gradeRepository.GetAllGrade(), "Id", "Name", toReturn.GradeId);
            if (toReturn.GradeId != -1)
            {
                toReturn.AcademicGradeSelectList =
                    new SelectList(_academicYearGradeRepository.Filter(x => x.Grade.Id == toReturn.GradeId), "Id",
                        "Name", toReturn.AcademicGradeId);
            }
            else
            {
                toReturn.AcademicGradeSelectList = new SelectList(list, "Value", "Text");
            }
            if (toReturn.AcademicGradeId != -1)
            {
                toReturn.AcademicCourseSelectList =
                    new SelectList(
                        _academicYearCourseRepository.Filter(x => x.AcademicYearGrade.Id == toReturn.AcademicGradeId),
                        "Id", "Name", toReturn.DestinationId);
            }
            else
            {
                toReturn.AcademicCourseSelectList = new SelectList(list, "Value", "Text");
            }
            return toReturn;
        }

        private SelectListNotificationRegisterModel LoadStudents(SelectListNotificationRegisterModel model)
        {
            var list = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "N/A" } };
            var toReturn = model as PersonalNotificationRegisterModel;
            if (toReturn == null) return model;
            toReturn.GradeSelectList = new SelectList(_gradeRepository.GetAllGrade(), "Id", "Name", toReturn.GradeId);
            if (toReturn.GradeId != -1)
            {
                toReturn.AcademicGradeSelectList =
                    new SelectList(_academicYearGradeRepository.Filter(x => x.Grade.Id == toReturn.GradeId), "Id",
                        "Name", toReturn.AcademicGradeId);
            }
            else
            {
                toReturn.AcademicGradeSelectList = new SelectList(list, "Value", "Text");
            }
            if (toReturn.AcademicGradeId != -1)
            {
                toReturn.PersonalSelectList =
                    new SelectList(_studentRepository.Filter(x => x.MyGrade.Id == toReturn.AcademicGradeId), "Id",
                        "FullName", toReturn.DestinationId);
            }
            else
            {
                toReturn.PersonalSelectList = new SelectList(list, "Value", "Text");
            }
            return toReturn;
        }

        public JsonResult LoadTypeNotification(SelectListNotificationRegisterModel model, long notificationType)
        {
            var dictType = new Dictionary<Type, NotificationType>
            {
                {typeof (EducationLevelNotificationRegisterModel), NotificationType.EducationLevel},
                {typeof (GradeNotificationRegisterModel), NotificationType.Grade},
                {typeof (AcademicGradeNotificationRegisterModel), NotificationType.Section},
                {typeof (AcademicCourseNotificationRegisterModel), NotificationType.Course},
                {typeof (PersonalNotificationRegisterModel), NotificationType.Personal}
            };
            if (model == null || dictType[model.GetType()] != (NotificationType)notificationType)
            {
                var dictModel = new Dictionary<NotificationType, SelectListNotificationRegisterModel>
                {
                    {NotificationType.EducationLevel, new EducationLevelNotificationRegisterModel()},
                    {NotificationType.Grade, new GradeNotificationRegisterModel()},
                    {NotificationType.Section, new AcademicGradeNotificationRegisterModel()},
                    {NotificationType.Course, new AcademicCourseNotificationRegisterModel()},
                    {NotificationType.Personal, new PersonalNotificationRegisterModel()}
                };
                model = dictModel[(NotificationType) notificationType];
            }
            var dict =
                new Dictionary<Type, Func<SelectListNotificationRegisterModel, SelectListNotificationRegisterModel>>
                {
                    {typeof(EducationLevelNotificationRegisterModel), LoadEducationLevels},
                    {typeof(GradeNotificationRegisterModel), LoadGrades},
                    {typeof(AcademicGradeNotificationRegisterModel), LoadAcademicGrades},
                    {typeof(AcademicCourseNotificationRegisterModel), LoadAcademicCourses},
                    {typeof(PersonalNotificationRegisterModel), LoadStudents}
                };
            return Json(dict[model.GetType()](model), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroupsAndEmails(string filter)
        {
            List<string> mails =
                _userRepository.Filter(x => x.DisplayName == filter || x.Email == filter).Select(x => x.Email).ToList();
            return Json(mails, JsonRequestBehavior.AllowGet);
        }
    }
}