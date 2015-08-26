using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IParentRepository _parentRepository;
        private readonly ISessionManagementRepository _sessionManagementRepository; //Will this be used?
        private readonly ISecurityRepository _securityRepository;
        private Parent _loggedParent;

        public NotificationController(INotificationRepository notificationRepository, IAcademicYearRepository academicYearRepository,
            IParentRepository parentRepository, ISessionManagementRepository sessionManagementRepository, ISecurityRepository securityRepository)
        {
            _notificationRepository = notificationRepository;
            _academicYearRepository = academicYearRepository;
            _parentRepository = parentRepository;
            _sessionManagementRepository = sessionManagementRepository;
            _securityRepository = securityRepository;
        }

        // GET: /Notification/
        [HttpGet]
        public ActionResult Index()
        {
            var currentAcademicYear = Convert.ToInt32(_academicYearRepository.GetCurrentAcademicYear().Year.Year.ToString(CultureInfo.InvariantCulture));
            var loggedUserEmail = _securityRepository.GetUserLoggedEmail();
            _loggedParent = _parentRepository.Filter(y => y.User.Email == loggedUserEmail).FirstOrDefault();
            var userId = _securityRepository.GetUserLogged().Id;
            var personalNotifications = _notificationRepository.GetPersonalNotifications(currentAcademicYear, userId).ToList();
            var notifications = _notificationRepository.GetGradeNotifications(currentAcademicYear, userId).ToList();
            notifications.AddRange(_notificationRepository.GetAreaNotifications(currentAcademicYear, userId).ToList());
            notifications.AddRange(_notificationRepository.GetGeneralNotifications(currentAcademicYear).ToList());
            var personalNotificationsModel = new List<NotificationModel>();
            var notificationsModel = new List<NotificationModel>();
            foreach (var notification in personalNotifications)
            {
                var noti = Mapper.Map<NotificationModel>(notification);
                noti.CommentsAmount = notification.NotificationComments.Count;
                noti.NotificationCreator = notification.UserCreatorName;
                personalNotificationsModel.Add(noti);
            }
            foreach (var notification in notifications)
            {
                var noti = Mapper.Map<Notification, NotificationModel>(notification);
                noti.CommentsAmount = notification.NotificationComments.Count;
                noti.NotificationCreator = notification.UserCreatorName;
                notificationsModel.Add(noti);
            }
            personalNotificationsModel = personalNotificationsModel.OrderByDescending(x => x.Created).ToList();
            notificationsModel = notificationsModel.OrderByDescending(x => x.Created).ToList();
            return View(new Tuple<List<NotificationModel>, List<NotificationModel>>(personalNotificationsModel, notificationsModel));
        }

        public ActionResult AddCommentToNotification(int notificationId, string commentText)
        {
            var loggedUserEmail = System.Web.HttpContext.Current.Session["loggedUserEmail"].ToString();
            _loggedParent = _parentRepository.Filter(y => y.User.Email == loggedUserEmail).FirstOrDefault();
            var selectedNotification = _notificationRepository.GetById(notificationId);
            selectedNotification.NotificationComments.Add(new NotificationComments
            {
                CommentText = commentText,
                CreationDate = DateTime.Now,
                Parent = _loggedParent
            });
            _notificationRepository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
