using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
//using Mhotivo.App_Data;
using System.Web.WebPages;
using AutoMapper;
using Mhotivo.Logic;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Repositories;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Controllers
{
    public class NotificationController : Controller
    {
        public MhotivoContext db = new MhotivoContext();
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly ISessionManagement _sessionManagement;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationTypeRepository _notificationTypeRepository;
        //private readonly IGradeRepository _gradeRepository;
        //private readonly IAreaReporsitory _areaReporsitory;

        private void LoadTypeNotification(ref NotificationModel model)
        {
            var items = db.NotificationTypes.Select(c => new SelectListItem()
            {
                Text = c.TypeDescription,
                Value = c.NotificationTypeId.ToString()
            }).ToList();
            model.NotificationTypeSelectList = new SelectList(items, "Value", "Text", model.NotificationTypeId);


            var list = new List<SelectListItem>().ToList();
            switch (model.NotificationTypeId.ToString(CultureInfo.InvariantCulture))
            {
                case "1":
                    list.Add(new SelectListItem() { Value = "0", Text = "N/A" });
                    break;
                case "2":
                    list = db.Areas.Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();
                    break;
                case "3":
                    list = db.Grades.Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();
                    break;
                case "4":
                    list = db.Users.Select(c => new SelectListItem()
                    {
                        Text = c.DisplayName,
                        Value = c.Id.ToString()
                    }).ToList();
                    break;
            }

            model.NotificationTypeOpionSelectList = new SelectList(list, "Value", "Text", model.IdGradeAreaUserGeneralSelected);
        }

        public NotificationController(ISessionManagement sessionManagement, IUserRepository userRepository, INotificationRepository notificationRepository,INotificationTypeRepository notificationTypeRepository) 
        {
            _sessionManagement = sessionManagement;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _notificationTypeRepository = notificationTypeRepository;
            //_gradeRepository = gradeRepository;
            //_areaReporsitory = areaReporsitory;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        //
        // GET: /NotificationModel/

        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var notifications = db.Notifications.Where(x => true);
            var notificationsModel = notifications.Select(Mapper.Map<NotificationModel>);
            
            return View(notificationsModel);
        }

        //
        // GET: /NotificationModel/Create
        [HttpGet]
        public ActionResult Add()
        {
            var notification = new NotificationModel();
            LoadTypeNotification(ref notification);
    
            return View("Add", notification);
        }

        [HttpPost]
        public ActionResult Add(NotificationModel eventNotification)
        {
            var notificationIdentity = Mapper.Map<Notification>(eventNotification);
            notificationIdentity.Created = DateTime.Now;

            var notificationType = db.NotificationTypes.FirstOrDefault(c => c.NotificationTypeId ==eventNotification.NotificationTypeId);
            notificationIdentity.NotificationType = notificationType;

            //if (AddressedTo.IsEmpty() || AddressedTo == null)
            //    AddressedTo = "0";

            //notificationIdentity.IdGradeAreaUserGeneralSelected = Convert.ToInt64(AddressedTo);

            db.Notifications.Add(notificationIdentity);
            db.SaveChanges();
            const string title = "Notificación Agregado";
            var content = "El evento " + eventNotification.NotificationName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);

            return RedirectToAction("Index");
        }

        public JsonResult OptiontList(string Id)
        {
            var list = new List<SelectListItem>();
            switch (Id)
            {
                case "1":
                    list.Add(new SelectListItem(){Value = "0",Text = "N/A"});
                    break;
                case "2":
                    list = db.Areas.Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();
                    break;
                case "3":
                    list = db.Grades.Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList();
                    break;
                case "4":
                    list = db.Users.Select(c => new SelectListItem()
                    {
                        Text = c.DisplayName,
                        Value = c.Id.ToString()
                    }).ToList();
                    break;
            }
            return Json(new SelectList(list, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroupsAndEmails(string filter)
        {
            List<string> groups = db.Groups.Where(x => x.Name.Contains(filter)).Select(x => x.Name).ToList();
            List<string> mails =
                db.Users.Where(x => x.DisplayName.Contains(filter) || x.Email.Contains(filter))
                    .Select(x => x.Email)
                    .ToList();
            groups = groups.Union(mails).ToList();
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /NotificationModel/Edit/5

        public ActionResult Edit(int id)
        {
            var toEdit = _notificationRepository.Query(x => x).Include("NotificationType").FirstOrDefault(x => x.Id.Equals(id));
            //var toEdit = db.Notifications.FirstOrDefault(x => x.Id.Equals(id));
            var toEditModel = Mapper.Map<NotificationModel>(toEdit);
            if (toEdit != null)
            {
                if (toEdit.NotificationType != null)
                    toEditModel.NotificationTypeId = toEdit.NotificationType.NotificationTypeId;

                toEditModel.IdGradeAreaUserGeneralSelected = toEdit.IdGradeAreaUserGeneralSelected;
            }
            LoadTypeNotification(ref toEditModel);

            return View(toEditModel);
        }

        //
        // POST: /NotificationModel/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, NotificationModel eventNotification)
        {
            try
            {
                var toEdit = _notificationRepository.Query(x => x).Include("NotificationType").FirstOrDefault(x => x.Id.Equals(id));

                var notificationType = _notificationTypeRepository.First(c => c.NotificationTypeId == eventNotification.NotificationTypeId);

                if (toEdit != null)
                {
                    toEdit.NotificationType = notificationType;
                    toEdit.NotificationName = eventNotification.NotificationName;
                    toEdit.Message = eventNotification.Message;
                    toEdit.IdGradeAreaUserGeneralSelected = eventNotification.IdGradeAreaUserGeneralSelected;
                }

                _notificationRepository.Update(toEdit);
                _notificationRepository.SaveChanges();
                _viewMessageLogic.SetNewMessage("Notificación Editada", "La notificación fue editada exitosamente.",
                    ViewMessageType.SuccessMessage);
                //}
            }
            catch
            {
                _viewMessageLogic.SetNewMessage("Error en edición",
                    "La notificación no pudo ser editada correctamente, por favor intente nuevamente.",
                    ViewMessageType.ErrorMessage);
            }
            IQueryable<Group> g = db.Groups.Select(x => x);
            return RedirectToAction("Index", g);
        }


        //
        // POST: /NotificationModel/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Notification toDelete = db.Notifications.FirstOrDefault(x => x.Id.Equals(id));
                db.Notifications.Remove(toDelete);
                db.SaveChanges();
                IQueryable<long> notifications = db.Notifications.Select(x => x.Id);
                return RedirectToAction("Index", notifications);
            }
            catch
            {
                return View("Index");
            }
        }


        public bool SendEmailForGeneralNotifications(AcademicYear currentAcademicYear)
        {
            var currentYear = currentAcademicYear.Year;
            var generalNotifications = db.Notifications.Where(n => n.Created.Year.Equals(currentYear) && n.NotificationType.NotificationTypeId == 1);

            foreach (var notification in generalNotifications)
            {


            }


            return false;

        }

    }
}