using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using Mhotivo.Authorizations;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class NotificationCommentController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationCommentRepository _notificationCommentRepository;

        public NotificationCommentController(INotificationRepository notificationRepository, INotificationCommentRepository notificationCommentRepository)
        {
            _notificationRepository = notificationRepository;
            _notificationCommentRepository = notificationCommentRepository;
        }

        [AuthorizeNewUser]
        public ActionResult Index(long notificationId)
        {
            var notification = _notificationRepository.GetById(notificationId);
            var commentsForNotifications = notification.NotificationComments.Select(Mapper.Map<NotificationCommentDisplayModel>);
            ViewBag.NotificationId = notificationId;
            return View(commentsForNotifications);
        }
        [AuthorizeNewUser]
        public ActionResult Delete(long notificationId, long commentId)
        {
            var comments = _notificationCommentRepository.GetById(commentId);
            _notificationCommentRepository.Delete(comments);
            return RedirectToAction("Index",notificationId);
        }

    }
}
