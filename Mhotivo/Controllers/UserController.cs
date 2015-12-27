using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using AutoMapper;
using Mhotivo.Authorizations;
using PagedList;

namespace Mhotivo.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly IRoleRepository _rolesRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEducationLevelRepository _educationLevelRepository;

        public UserController(IUserRepository userRepository, IPasswordGenerationService passwordGenerationService,
            IRoleRepository rolesRepository, IRoleRepository roleRepository, IEducationLevelRepository educationLevelRepository)
        {
            _userRepository = userRepository;
            _passwordGenerationService = passwordGenerationService;
            _rolesRepository = rolesRepository;
            _roleRepository = roleRepository;
            _educationLevelRepository = educationLevelRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var listaUsuarios = _userRepository.GetAllUsers();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                listaUsuarios = _userRepository.Filter(x => x.UserOwner.FirstName.Contains(searchString) || x.Email.Contains(searchString)).ToList();
            }

            var listaUsuariosModel = listaUsuarios.Select(Mapper.Map<UserDisplayModel>);
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "name_desc":
                    listaUsuariosModel = listaUsuariosModel.OrderByDescending(s => s.UserOwner).ToList();
                    break;
                case "Email":
                    listaUsuariosModel = listaUsuariosModel.OrderBy(s => s.Email).ToList();
                    break;
                case "email_desc":
                    listaUsuariosModel = listaUsuariosModel.OrderByDescending(s => s.Email).ToList();
                    break;
                default:  // Name ascending 
                    listaUsuariosModel = listaUsuariosModel.OrderBy(s => s.UserOwner).ToList();
                    break;
            }

            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(listaUsuariosModel.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            User thisUser = _userRepository.GetById(id);
            var user = Mapper.Map<UserEditModel>(thisUser);
            var role = _userRepository.GetUserRole(thisUser.Id);
            ViewBag.Role = role.Name;
            ViewBag.Roleid = new SelectList(_roleRepository.Filter(x => x.Name.Equals("Administrador") || x.Name.Equals("Director")), "Id", "Name", role);
            return View("Edit", user);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(UserEditModel modelUser)
        {
            User myUser = _userRepository.GetById(modelUser.Id);
            var directorRole = _roleRepository.Filter(x => x.Name.Equals("Director")).FirstOrDefault();
            if (directorRole != null && myUser.Role.Id == directorRole.Id)
            {
                if (modelUser.Role != myUser.Role.Id)
                {
                    if (_educationLevelRepository.Filter(x => x.Director != null && x.Director.Id == myUser.Id).Any())
                    {
                        const string title2 = "Error";
                        var content2 = "Directores no pueden ser promovidos a administradores mientras tienen un nivel de educacion asignado.";
                        _viewMessageLogic.SetNewMessage(title2, content2, ViewMessageType.ErrorMessage);
                        return RedirectToAction("Index");
                    }
                }
            }
            Mapper.Map(modelUser, myUser);
            _userRepository.Update(myUser);
            const string title = "Usuario Actualizado";
            var content = "El usuario " + myUser.UserOwner.FirstName + " - " + myUser.Email +
                             " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            var user = _userRepository.Delete(id);
            const string title = "Usuario Eliminado";
            var content = "El usuario " + user.UserOwner.FirstName + " - " + user.Email + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DoesUserNameExist(string email)
        {
            var user = _userRepository.Filter(x => x.Email == email).FirstOrDefault();
            return Json(user == null);
        }
    }
}