using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ISessionManagementService _sessionManagementService;
        private readonly IUserRepository _userRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public MyAccountController(ISessionManagementService sessionManagementService, IUserRepository userRepository)
        {
            _sessionManagementService = sessionManagementService;
            _userRepository = userRepository;
            _viewMessageLogic  = new ViewMessageLogic(this);
        }

        //
        // GET: /MyAccount/

        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var userId = Convert.ToInt64(_sessionManagementService.GetUserLoggedId());
            var user = _userRepository.GetById(userId);
            var myaccountmodel = new AccountEditModel()
            {
                Id = user.Id,
                Email = user.Email
            };
            return View(myaccountmodel);
        }

        [HttpPost]
        public ActionResult Index(AccountEditModel model)
        {
            var userId = Convert.ToInt64(_sessionManagementService.GetUserLoggedId());
            var user = _userRepository.GetById(userId);

            if (model.Email != null)
            {
                user.Email = model.Email;
            }

            if (model.NewPassword != null)
            {
                if (user.CheckPassword(model.OldPassword))
                {
                    user.Password = model.NewPassword;
                    user.HashPassword();
                }
                else
                {
                    const string title = "Error!";
                    const string content = "Contraseña Incorrecta.";
                    _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                    return View(model);
                }
            }

            try
            {
                if (model.UploadPhoto != null)
                {
                    using (var binaryReader = new BinaryReader(model.UploadPhoto.InputStream))
                    {
                        user.UserOwner.Photo = binaryReader.ReadBytes(model.UploadPhoto.ContentLength);

                    }
                }
            }
            catch (Exception e)
            {
                const string title = "Error!";
                const string content = "Formato de Imagen Incorrecto";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.ErrorMessage);
                return View(model);
            }

            _userRepository.Update(user);
            const string title2 = "Usuario Actualizado";
            const string content2 = "El usuario ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title2, content2, ViewMessageType.SuccessMessage);
            
            return RedirectToAction("Index");
        }

    }
}
