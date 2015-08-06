using System.Linq;
using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using AutoMapper;
using Mhotivo.Authorizations;

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

         [AuthorizeAdmin]
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var listaRoles = _roleRepository.GetAllRoles();
            var listaRolesModel = listaRoles.Select(Mapper.Map<DisplayRolModel>);
            return View(listaRolesModel);
        }

        // GET: /Role/
        [HttpGet]
        [AuthorizeAdmin]
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
        [AuthorizeAdmin]
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