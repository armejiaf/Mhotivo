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
            selectedNotificationModel.NotificationCreator = selectedNotification.UserCreatorName; 
            var commentsList = selectedNotification.NotificationComments.ToList();
            List<NotificationCommentsModel> commentsModelList = commentsList.Select(comment =>
            {
                var firstOrDefault = selectedNotification.NotificationComments.FirstOrDefault(x => x.Id==comment.Id);
                return firstOrDefault != null ? new NotificationCommentsModel
                {
                    CommentText = comment.CommentText,
                    CreationDate = comment.CreationDate,
                    Parent = firstOrDefault.Parent.FullName,
                    ParentPhotoUrl = firstOrDefault.Parent.UrlPicture
                } : null;
            }).ToList();
            selectedNotificationModel.CommentsAmount = commentsList.Count;
            return View(new Tuple<NotificationModel, List<NotificationCommentsModel>>(selectedNotificationModel, commentsModelList));
        }
    }
}
