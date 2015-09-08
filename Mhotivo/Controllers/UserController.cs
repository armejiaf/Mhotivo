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
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityRepository _securityRepository; //Will this be used?
        private readonly ViewMessageLogic _viewMessageLogic;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository, ISecurityRepository securityRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _securityRepository = securityRepository;
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
                listaUsuarios = _userRepository.Filter(x => x.DisplayName.Contains(searchString) || x.Email.Contains(searchString)).ToList();

            }

            var listaUsuariosModel = listaUsuarios.Select(Mapper.Map<DisplayUserModel>);
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "name_desc":
                    listaUsuariosModel = listaUsuariosModel.OrderByDescending(s => s.DisplayName).ToList();
                    break;
                case "Email":
                    listaUsuariosModel = listaUsuariosModel.OrderBy(s => s.Email).ToList();
                    break;
                case "email_desc":
                    listaUsuariosModel = listaUsuariosModel.OrderByDescending(s => s.Email).ToList();
                    break;
                default:  // Name ascending 
                    listaUsuariosModel = listaUsuariosModel.OrderBy(s => s.DisplayName).ToList();
                    break;
            }

             var displayUserModels = (IList<DisplayUserModel>) listaUsuariosModel ?? listaUsuariosModel.ToList();
             foreach (var usuario in displayUserModels)
             {
                 var role = _userRepository.GetUserRoles(usuario.Id).FirstOrDefault();
                 usuario.Role = role;
                 if (role != null) usuario.RoleName = role.Description;
             }

            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(displayUserModels.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(long id)
        {
            User thisUser = _userRepository.GetById(id);
            var user = Mapper.Map<UserEditModel>(thisUser);
            var roles = _userRepository.GetUserRoles(thisUser.Id);
            ViewBag.RoleId = new SelectList(_roleRepository.Query(x => x), "Id", "Name", roles.First().Id);
            var firstOrDefault = roles.FirstOrDefault();
            if (firstOrDefault != null) user.RoleId = (int) firstOrDefault.Id;
            return View("Edit", user);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(UserEditModel modelUser)
        {
            bool updateRole = false;
            User myUsers = _userRepository.GetById(modelUser.Id);
            var myUser = Mapper.Map<User>(modelUser);
            var rol = _roleRepository.GetById(modelUser.RoleId);
            var rolesUser = _userRepository.GetUserRoles(myUser.Id);
            if (rolesUser.Any() && rolesUser.First().Id != modelUser.RoleId)
            {
                updateRole = true;
            }
            var user = _userRepository.UpdateUserFromUserEditModel(myUser,myUsers, updateRole, rol);
            const string title = "Usuario Actualizado";
            var content = "El usuario " + user.DisplayName + " - " + user.Email +
                             " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            var user = _userRepository.Delete(id);
            const string title = "Usuario Eliminado";
            var content = "El usuario " + user.DisplayName + " - " + user.Email + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            ViewBag.Id = new SelectList(_roleRepository.Query(x => x), "Id", "Name");
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(UserRegisterModel modelUser)
        {
            var rol = _roleRepository.Filter(x => x.Name == "Administrador").FirstOrDefault();
            var myUser = new User
            {
                DisplayName = modelUser.DisplaName,
                Email = modelUser.UserName,
                Password = modelUser.Password,
                IsActive = modelUser.Status
            };

            if (_userRepository.ExistEmail(modelUser.UserName))
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            
            var user = _userRepository.Create(myUser, rol);
            const string title = "Usuario Agregado";
            var content = "El usuario " + user.DisplayName + " - " + user.Email + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DoesUserNameExist(string email)
        {

            var user = _userRepository.FirstOrDefault(x => x.Email == email);

            return Json(user == null);
        }
    }
}