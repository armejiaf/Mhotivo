using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Implement;
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
            private readonly ISessionManagementRepository _sessionManagementRepository;
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

            //
            // GET: /Notification/
            [HttpGet]
            public ActionResult Index()
            {
                var currentAcademicYear = Convert.ToInt32(_academicYearRepository.GetCurrentAcademicYear().Year.Year.ToString());
                var loggedUserEmail = _securityRepository.GetUserLoggedEmail();

                _loggedParent = _parentRepository.Filter(y => y.User.Email == loggedUserEmail).FirstOrDefault();
                
                var userId = _securityRepository.GetUserLogged().Id;
               
                var personalNotifications = _notificationRepository.GetPersonalNotifications(currentAcademicYear, userId).ToList();

                var notifications=_notificationRepository.GetGeneralNotifications(currentAcademicYear).ToList();
                var gradeNotifications = _notificationRepository.GetGradeNotifications(currentAcademicYear, userId).ToList();
                var areaNotifications = _notificationRepository.GetAreaNotifications(currentAcademicYear, userId).ToList();

                var personalNotificationsModel = new List<NotificationModel>();
                var notificationsModel = new List<NotificationModel>();
                var gradeNotificationsModel = new List<NotificationModel>();
                var areaNotificationsModel = new List<NotificationModel>();

                foreach (var notification in personalNotifications)
                {
                    var noti = Mapper.Map<NotificationModel>(notification);

                    noti.CommentsAmount = notification.NotificationComments.Count;
                    noti.NotificationCreator = notification.UserCreatorName;                    
                    //noti.NotificationCreator = "Random person";
                    personalNotificationsModel.Add(noti);
                }

                foreach (var notification in notifications)
                {
                    var noti = Mapper.Map<NotificationModel>(notification);
                    
                    noti.CommentsAmount = notification.NotificationComments.Count;
                    noti.NotificationCreator = notification.UserCreatorName;
                    //noti.NotificationCreator = "Random person";
                    notificationsModel.Add(noti);
                }

                foreach (var notification in gradeNotifications)
                {
                    var noti = Mapper.Map<NotificationModel>(notification);

                    noti.CommentsAmount = notification.NotificationComments.Count;
                    noti.NotificationCreator = notification.UserCreatorName;
                    //noti.NotificationCreator = "Random person";
                    gradeNotificationsModel.Add(noti);
                }

                foreach (var notification in areaNotifications)
                {
                    var noti = Mapper.Map<NotificationModel>(notification);

                    noti.CommentsAmount = notification.NotificationComments.Count;
                    noti.NotificationCreator = notification.UserCreatorName;
                    //noti.NotificationCreator = "Random person";
                    areaNotificationsModel.Add(noti);
                }

                personalNotificationsModel = personalNotificationsModel.OrderByDescending(x => x.Created).ToList();
                notificationsModel = notificationsModel.OrderByDescending(x => x.Created).ToList();
                gradeNotificationsModel = gradeNotificationsModel.OrderByDescending(x => x.Created).ToList();
                areaNotificationsModel = areaNotificationsModel.OrderByDescending(x => x.Created).ToList();

                return View(new Tuple<List<NotificationModel>, List<NotificationModel>, List<NotificationModel>, List<NotificationModel>>
                    (personalNotificationsModel, notificationsModel, gradeNotificationsModel, areaNotificationsModel));
            }

            public ActionResult AddCommentToNotification(int notificationId,string commentText)
            {
                var loggedUserEmail = System.Web.HttpContext.Current.Session["loggedUserEmail"].ToString();//Session["Email"].ToString();
                //var loggedUserEmail = _sessionManagementRepository.GetUserLoggedEmail();
                _loggedParent = _parentRepository.Filter(y => y.User.Email == loggedUserEmail).FirstOrDefault();
                var selectedNotification = _notificationRepository.GetById(notificationId);
                
                selectedNotification.NotificationComments.Add(new NotificationComments
                {
                    CommentText = commentText,
                    CreationDate = DateTime.Now,
                    Parent=_loggedParent
                });

                _notificationRepository.SaveChanges();

                return RedirectToAction("Index");
            }

        }
}
