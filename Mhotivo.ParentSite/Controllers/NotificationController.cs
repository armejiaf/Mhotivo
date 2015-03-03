using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
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
                var currentAcademicYear = _academicYearRepository.GetCurrentAcademicYear();
                var notifications = _notificationRepository.GetGeneralNotifications(currentAcademicYear);

                var notificationsModel = notifications.Select(Mapper.Map<NotificationModel>);
                notificationsModel=notificationsModel.OrderByDescending(x => x.Created);

                return View(notificationsModel);
            }

        }
}
