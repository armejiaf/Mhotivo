using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class NotificationTypeController : Controller
    {
        private readonly INotificationTypeRepository _notificationtypeReporRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public NotificationTypeController(INotificationTypeRepository notificationtypeReporRepository)
        {
            _notificationtypeReporRepository = notificationtypeReporRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        // GET: /NotificationTypeSelectList/
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            IEnumerable<NotificationTypeModel> notificationType = _notificationtypeReporRepository.Query(x => x).ToList().Select(x => new NotificationTypeModel
            { 
                NotificationTypeId = x.Id,
                TypeDescription = x.Description
            });
            return View(notificationType);
        }
        
        // GET: /NotificationTypeSelectList/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /NotificationTypeSelectList/Create
        [HttpPost]
        public ActionResult Add(NotificationType notificationType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _notificationtypeReporRepository.Create(notificationType);
                    _notificationtypeReporRepository.SaveChanges();
                    _viewMessageLogic.SetNewMessage("Agregado", "El Tipo de notificacion fue agregado exitosamente.", ViewMessageType.SuccessMessage);
                }
                else
                {
                    _viewMessageLogic.SetNewMessage("Validación de Información", "La información no es válida.", ViewMessageType.InformationMessage);
                }
            }
            catch
            {
                _viewMessageLogic.SetNewMessage("Error", "Algo salió mal, por favor intente de nuevo.", ViewMessageType.ErrorMessage);
            }
            IQueryable<NotificationType> notificationTypes = _notificationtypeReporRepository.Query(x => x);
            return RedirectToAction("Index", notificationTypes);
        }

        // GET: /NotificationTypeSelectList/Edit/5
        public ActionResult Edit(int id)
        {
            NotificationType c = _notificationtypeReporRepository.GetById(id);
            return View(c); //Compile magic!
        }

        // POST: /NotificationTypeSelectList/Edit/5
        [HttpPost]
        public ActionResult Edit(NotificationType notificationTypes)
        {
            NotificationType role = _notificationtypeReporRepository.Update(notificationTypes);
            const string title = "Tipo Notificacion Actualizado";
            var content = "El Tipo Notificacion " + role.Description + " ha sido modificado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        // GET: /NotificationTypeSelectList/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                NotificationType group = _notificationtypeReporRepository.GetById(id);
                _notificationtypeReporRepository.Delete(group);
                _notificationtypeReporRepository.SaveChanges();
                _viewMessageLogic.SetNewMessage("Eliminado", "Eliminado exitosamente.", ViewMessageType.SuccessMessage);
                return RedirectToAction("Index");
            }
            catch
            {
                _viewMessageLogic.SetNewMessage("Error en eliminación",
                    "El Tipo de Notificacion no pudo ser eliminado correctamente, por favor intente nuevamente.",
                    ViewMessageType.ErrorMessage);
                return View("Index");
            }
        }
    }
}
