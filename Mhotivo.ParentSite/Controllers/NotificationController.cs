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

            public NotificationController(INotificationRepository notificationRepository, IAcademicYearRepository academicYearRepository)
            {
                _notificationRepository = notificationRepository;
                _academicYearRepository = academicYearRepository;
            }

            //
            // GET: /Notification/
            [HttpGet]
            public ActionResult Index()
            {
                var currentAcademicYear = Convert.ToInt32(_academicYearRepository.GetCurrentAcademicYear().Year.Year.ToString());
                
                var notifications = _notificationRepository.GetGeneralNotifications(currentAcademicYear).ToList();
                notifications.AddRange(_notificationRepository.GetAreaNotifications(currentAcademicYear, 3).ToList()); 
                notifications.AddRange(_notificationRepository.GetGradeNotifications(currentAcademicYear, 2).ToList());
                notifications.AddRange(_notificationRepository.GetPersonalNotifications(currentAcademicYear, 1).ToList());
                
                var notificationsModel = new List<NotificationModel>();

                foreach (var notification in notifications)
                {
                    var noti = Mapper.Map<NotificationModel>(notification);
                    
                    noti.CommentsAmount = notification.NotificationComments.Count;
                    //noti.NotificationCreator = notification.NotificationCreator.DisplayName;                    

                    notificationsModel.Add(noti);
                }

                notificationsModel=notificationsModel.OrderByDescending(x => x.Created).ToList();

                return View(notificationsModel);
            }

            public ActionResult AddCommentToNotification(int notificationId,string commentText)
            {
                var selectedNotification = _notificationRepository.GetById(notificationId);
                
                selectedNotification.NotificationComments.Add(new NotificationComments
                {
                    CommentText = commentText,
                    CreationDate = DateTime.Now
                });

                _notificationRepository.SaveChanges();

                return RedirectToAction("Index");
            }

        }
}
