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
        private readonly ISecurityService _securityService; //Will this be used?
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly IRoleRepository _rolesRepository;

        public UserController(IUserRepository userRepository, ISecurityService securityService,
            IPasswordGenerationService passwordGenerationService, IRoleRepository rolesRepository)
        {
            _userRepository = userRepository;
            _securityService = securityService;
            _passwordGenerationService = passwordGenerationService;
            _rolesRepository = rolesRepository;
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
                 var role = _userRepository.GetUserRole(usuario.Id);
                 usuario.Role = role;
                 usuario.RoleName = usuario.Role.Name;
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
            var role = _userRepository.GetUserRole(thisUser.Id);
            var directions = from Role d in _rolesRepository.GetAll()
                             where d != null
                             select new { ID = d.Id, d.Name };
            user.RoleId = role.Id;
            ViewBag.RoleId = new SelectList(directions, "ID", "Name", user.RoleId);
            return View("Edit", user);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(UserEditModel modelUser)
        {
            User myUser = _userRepository.GetById(modelUser.Id);
            Mapper.Map(modelUser, myUser);
            _userRepository.Update(myUser);
            const string title = "Usuario Actualizado";
            var content = "El usuario " + myUser.DisplayName + " - " + myUser.Email +
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
            var directions = from Role d in _rolesRepository.GetAll()
                             where d != null
                             select new { ID = d.Id, Name = d.Name};
            ViewBag.Id = new SelectList(directions, "ID", "Name");
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(UserRegisterModel modelUser)
        {
            if (_userRepository.Filter(x => x.Email == modelUser.Email).Any())
            {
                _viewMessageLogic.SetNewMessage("Dato Invalido", "El Correo Electronico ya esta en uso", ViewMessageType.ErrorMessage);
                return RedirectToAction("Index");
            }
            var myUser = new User
            {
                DisplayName = modelUser.DisplayName,
                Email = modelUser.Email,
                Password = _passwordGenerationService.GenerateTemporaryPassword(),
                IsUsingDefaultPassword = true,
                IsActive = modelUser.Status,
                Role = _rolesRepository.Filter(x => x.Name == "Administrador").FirstOrDefault()
            };
            myUser.DefaultPassword = myUser.Password;
            var user = _userRepository.Create(myUser);
            const string title = "Usuario Agregado";
            var content = "El usuario " + user.DisplayName + " - " + user.Email + " ha sido agregado exitosamente.";
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