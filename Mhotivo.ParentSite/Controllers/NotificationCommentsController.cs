using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    public class NotificationCommentsController : Controller
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationCommentsController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // GET: /NotificationComments/
        [HttpGet]
        public ActionResult Index(int notificationId)
        {
            var selectedNotification = _notificationRepository.GetById(notificationId);
            var selectedNotificationModel = Mapper.Map<NotificationModel>(selectedNotification);
            var commentsList = selectedNotification.NotificationComments.Select(Mapper.Map<NotificationCommentsModel>).ToList();
            return View(new Tuple<NotificationModel, List<NotificationCommentsModel>>(selectedNotificationModel, commentsList));
        }
    }
}
