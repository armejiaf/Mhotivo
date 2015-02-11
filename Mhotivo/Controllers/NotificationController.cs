using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
//using Mhotivo.App_Data;
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

        private void LoadTypeNotification()
        {

            var items = db.NotificationTypes.Select(c => new SelectListItem()
            {
                Text = c.TypeDescription,
                Value = c.NotificationTypeId.ToString()
            }).ToList();
            var list = new SelectList(items, "Value", "Text");
            ViewData["NotificationTypes"] = list;
        }

        public NotificationController(ISessionManagement sessionManagement, IUserRepository userRepository)
        {
            _sessionManagement = sessionManagement;
            _userRepository = userRepository;
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
            LoadTypeNotification();
    
            return View("Add", notification);
        }

        [HttpPost]
        public ActionResult Add(NotificationModel eventNotification, int NotificationTypes)
        {
            var template = Mapper.Map<Notification>(eventNotification);
            template.Created = DateTime.Now;

            // Recuperamos la ciudad ==> Consulta a BBDD
            var notificationTypes = db.NotificationTypes.FirstOrDefault(c => c.NotificationTypeId == NotificationTypes);
            template.NotificationTypeId = notificationTypes;

            //string userEmail = _sessionManagement.GetUserLoggedEmail();
            //User creator = _userRepository.First(x => x.Email == userEmail);
            //template.NotificationCreator = creator;

            db.Notifications.Add(template);
            db.SaveChanges();
            const string title = "Notificación Agregado";
            var content = "El evento " + eventNotification.NotificationName + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);

            return RedirectToAction("Index");
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
            var toEdit = db.Notifications.FirstOrDefault(x => x.Id.Equals(id));

            var toEditModel = Mapper.Map<NotificationModel>(toEdit);

            LoadTypeNotification();

            return View(toEditModel);
        }

        //
        // POST: /NotificationModel/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Notification notification, int NotificationTypes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Recuperamos la ciudad ==> Consulta a BBDD
                    var notificationTypes = db.NotificationTypes.FirstOrDefault(c => c.NotificationTypeId == NotificationTypes);
                    notification.NotificationTypeId = notificationTypes;

                    db.Entry(notification).State = EntityState.Modified;
                    db.SaveChanges();
                    _viewMessageLogic.SetNewMessage("Notificación Editada", "La notificación fue editada exitosamente.", ViewMessageType.SuccessMessage);
                }
            }
            catch
            {
                _viewMessageLogic.SetNewMessage("Error en edición", "La notificación no pudo ser editada correctamente, por favor intente nuevamente.", ViewMessageType.ErrorMessage);
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
            var generalNotifications = db.Notifications.Where(n => n.Created.Year.Equals(currentYear) && n.NotificationTypeId.NotificationTypeId == 1);

            foreach (var notification in generalNotifications)
            {


            }


            return false;

        }

    }
}