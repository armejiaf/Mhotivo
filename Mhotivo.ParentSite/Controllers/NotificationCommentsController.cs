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
    public class NotificationCommentsController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
           
            public NotificationCommentsController(INotificationRepository notificationRepository)
            {
                _notificationRepository = notificationRepository;
                
            }
        //
        // GET: /NotificationComments/
        [HttpGet]
        public ActionResult Index(int notificationId)
        {
            
            var selectedNotification = _notificationRepository.GetById(notificationId);
            
            var selectedNotificationModel = Mapper.Map<NotificationModel>(selectedNotification); 

            var commentsList = selectedNotification.NotificationComments.ToList();
            List<NotificationCommentsModel> commentsModelList;

            commentsModelList = commentsList.Select(comment => new NotificationCommentsModel
            {
                CommentText = comment.CommentText, CreationDate = comment.CreationDate,
                Parent = selectedNotification.NotificationComments.Where(x=>x.Id==comment.Id).FirstOrDefault().Parent.FullName,
                ParentPhotoUrl = selectedNotification.NotificationComments.Where(x => x.Id == comment.Id).FirstOrDefault().Parent.UrlPicture
            }).ToList();

            selectedNotificationModel.CommentsAmount = commentsList.Count;

            return View(new Tuple<NotificationModel, List<NotificationCommentsModel>>(selectedNotificationModel, commentsModelList));
        }

    }
}
