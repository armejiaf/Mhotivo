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
            public readonly IPeopleRepository _peopleRepository;

            public NotificationController(INotificationRepository notificationRepository, IAcademicYearRepository academicYearRepository,IPeopleRepository peopleRepository)
            {
                _notificationRepository = notificationRepository;
                _academicYearRepository = academicYearRepository;
                _peopleRepository = peopleRepository;
            }

            //
            // GET: /Notification/
            [HttpGet]
            public ActionResult Index()
            {
                var currentAcademicYear = Convert.ToInt32(_academicYearRepository.GetCurrentAcademicYear().Year.Year.ToString());
                var studentGradeId = 2;
                var studentId = 1;

                var notifications = _notificationRepository.GetPersonalNotifications(currentAcademicYear, studentId).OrderByDescending(x => x.Created).ToList();
                notifications.AddRange(_notificationRepository.GetGradeNotifications(currentAcademicYear, studentGradeId).OrderByDescending(x => x.Created).ToList());
                notifications.AddRange(_notificationRepository.GetAreaNotifications(currentAcademicYear, 3).OrderByDescending(x => x.Created).ToList());
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
                var selectedNotification = _notificationRepository.GetById(notificationId);
                var parentLogged = _peopleRepository.GetById(4);

                selectedNotification.NotificationComments.Add(new NotificationComments
                {
                    CommentText = commentText,
                    CreationDate = DateTime.Now,
                    Parent=parentLogged
                });

                _notificationRepository.SaveChanges();

                return RedirectToAction("Index");
            }

        }
}
