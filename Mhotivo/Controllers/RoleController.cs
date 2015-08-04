using System.Linq;
using System.Web.Mvc;
//using Mhotivo.App_Data.Repositories;
//using Mhotivo.App_Data.Repositories.Interfaces;
using Mhotivo.Implement.Repositories;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using AutoMapper;

namespace Mhotivo.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }


        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();

            var listaRoles = _roleRepository.GetAllRoles();

            var listaRolesModel = listaRoles.Select(Mapper.Map<DisplayRolModel>);

            return View(listaRolesModel);
        }

        //
        // GET: /Role/

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var r = _roleRepository.GetById(id);
            var role = new RoleEditModel
                       {
                           Id = r.Id,
                           Description = r.Description,
                           Name = r.Name
                       };

            return View("_Edit", role);
        }

        [HttpPost]
        public ActionResult Edit(RoleEditModel modelRole)
        {
            var rol = _roleRepository.GetById(modelRole.Id);

            rol.Name = modelRole.Name;
            rol.Description = modelRole.Description;

            var role = _roleRepository.Update(rol);
            const string title = "Role Actualizado";
            var content = "El role " + role.Name + " ha sido modificado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);

            return RedirectToAction("Index");
        }
    }
}