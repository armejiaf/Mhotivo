using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using Mhotivo.Implement.Context;
using Mhotivo.Data.Entities;

namespace Mhotivo.Controllers
{
    public class GroupController : Controller
    {
        private readonly MhotivoContext _db = new MhotivoContext(); //Dependencies. Remove this.
        private readonly ViewMessageLogic _viewMessageLogic;

        public GroupController()
        {
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        // GET: /Group/
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            IQueryable<Group> groups = _db.Groups.Select(x => x);
            return View(groups); //compilation magic!
        }

        public JsonResult GetMembers(string filter)
        {
            var members =
                _db.Users.Where(x => x.DisplayName.Contains(filter) || x.Email.Contains(filter))
                    .Select(x => new {name = x.DisplayName + " <" + x.Email + ">", value = x.Id})
                    .ToList();
            return Json(members, JsonRequestBehavior.AllowGet);
        }

        // GET: /Group/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /Group/Create
        [HttpPost]
        public ActionResult Add(AddGroup group)
        {
            try
            {
                List<long> usersId = group.Users.Split(',').Select(Int64.Parse).ToList();
                IQueryable<User> users = _db.Users.Where(x => usersId.Contains( x.Id));
                var g = new Group {Name = group.Name, Users = users.ToList()};
                if (ModelState.IsValid && IsNameAvailble(g.Name))
                {
                    _db.Groups.Add(g);
                    _db.SaveChanges();
                    _viewMessageLogic.SetNewMessage("Grupo Agregado", "El grupo fue agregado exitosamente.", ViewMessageType.SuccessMessage);
                }
                else
                {
                    _viewMessageLogic.SetNewMessage("Validación de Información", "La información es inválida.", ViewMessageType.InformationMessage);
                }
            }
            catch(Exception ex)
            {
                _viewMessageLogic.SetNewMessage("Error", ex.Message+" salió mal, por favor intente de nuevo.", ViewMessageType.ErrorMessage);
            }
            IQueryable<Group> groups = _db.Groups.Select(x => x);
            return RedirectToAction("Index", groups);
        }

        // GET: /Group/Edit/5
        public ActionResult Edit(int id)
        {
            Group group = _db.Groups.FirstOrDefault(x => x.Id.Equals(id));
            return View("Edit", group); //Compilation magic!
        }

        // POST: /Group/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AddGroup group)
        {
            try
            {
                Group g = _db.Groups.FirstOrDefault(x => x.Id.Equals(id));
                g.Name = group.Name;
                if (!group.Users.IsEmpty())
                {
                    List<long> usersId = group.Users.Split(',').Select(Int64.Parse).ToList();
                    IQueryable<User> users = _db.Users.Where(x => usersId.Contains(x.Id));
                    g.Users = g.Users.Concat(users).ToList();
                }
                if (ModelState.IsValid )
                {
                    _db.Entry(g).State = EntityState.Modified;
                    _db.SaveChanges();
                    _viewMessageLogic.SetNewMessage("Grupo Editado", "El grupo fue editado exitosamente.", ViewMessageType.SuccessMessage);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                _viewMessageLogic.SetNewMessage("Error en edición", "El grupo no pudo ser editado correctamente, por favor intente nuevamente.", ViewMessageType.ErrorMessage);
                return View("Index");
            }
        }

        // POST: /Group/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Group group = _db.Groups.FirstOrDefault(x => x.Id == id);
                _db.Groups.Remove(group);
                _db.SaveChanges();
                _viewMessageLogic.SetNewMessage("Grupo eliminado", "Grupo eliminado exitosamente.", ViewMessageType.SuccessMessage);
                return RedirectToAction("Index");
            }
            catch
            {
                _viewMessageLogic.SetNewMessage("Error en eliminación", "El grupo no pudo ser eliminado correctamente, por favor intente nuevamente.", ViewMessageType.ErrorMessage);
                return View("Index");
            }
        }

        [HttpPost]
        public bool DeleteUser(int id, int groupId)
        {
            try
            {
                _db.Database.ExecuteSqlCommand("Delete From UserGroups where User_UserId=" + id + " and Group_Id=" +
                                              groupId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsNameAvailble(string name)
        {
            var tag = _db.Groups.First(g => g.Name.CompareTo(name) == 0);
            if (tag == null)
            {
                return true;
            }
            return false;
        }
    }
}