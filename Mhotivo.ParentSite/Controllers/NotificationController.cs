using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    public class NotificationController : Controller
        {
            private readonly INotificationRepository _notificationRepository;
            private readonly IAcademicYearRepository _academicYearRepository;
            private readonly IParentRepository _parentRepository;
            private readonly ISessionManagementRepository _sessionManagementRepository;
            private Parent _loggedParent;

            public NotificationController(INotificationRepository notificationRepository, IAcademicYearRepository academicYearRepository,
                IParentRepository parentRepository, ISessionManagementRepository sessionManagementRepository)
            {
                _notificationRepository = notificationRepository;
                _academicYearRepository = academicYearRepository;
                _parentRepository = parentRepository;
                _sessionManagementRepository = sessionManagementRepository;
                
            }

            //
            // GET: /Notification/
            [HttpGet]
            public ActionResult Index()
            {
                var currentAcademicYear = Convert.ToInt32(_academicYearRepository.GetCurrentAcademicYear().Year.Year.ToString());
                var loggedUserEmail = Session["Email"].ToString();
                //var loggedUserEmail = _sessionManagementRepository.GetUserLoggedEmail();
                _loggedParent = _parentRepository.Filter(y => y.User.Email == loggedUserEmail).FirstOrDefault();
                var userId = _loggedParent.UserId.Id;
               
                var notifications = _notificationRepository.GetPersonalNotifications(currentAcademicYear, userId).OrderByDescending(x => x.Created).ToList();
                notifications.AddRange(_notificationRepository.GetGradeNotifications(currentAcademicYear, userId).OrderByDescending(x => x.Created).ToList());
                notifications.AddRange(_notificationRepository.GetAreaNotifications(currentAcademicYear, userId).OrderByDescending(x => x.Created).ToList());
                notifications.AddRange(_notificationRepository.GetGeneralNotifications(currentAcademicYear).OrderByDescending(x => x.Created).ToList());
                
                var notificationsModel = new List<NotificationModel>();

                foreach (var notification in notifications)
                {
                    var noti = Mapper.Map<NotificationModel>(notification);
                    
                    noti.CommentsAmount = notification.NotificationComments.Count;
                    //noti.NotificationCreator = notification.NotificationCreator.DisplayName;                    

                    notificationsModel.Add(noti);
                }

                notificationsModel = notificationsModel.Take(5).ToList();

                return View(notificationsModel);
            }

            public ActionResult AddCommentToNotification(int notificationId,string commentText)
            {
                var loggedUserEmail = Session["Email"].ToString();
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
