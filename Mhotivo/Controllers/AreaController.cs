using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Repositories;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using AutoMapper;
using Mhotivo.Encryption;

namespace Mhotivo.Controllers
{
    public class AreaController : Controller
    {
        //
        // GET: /Area/
        private readonly IAreaRepository _areaReposity;
        private readonly ViewMessageLogic _viewMessageLogic;

        public AreaController(IAreaRepository areaReposity)
        {
            this._areaReposity = areaReposity;
            _viewMessageLogic = new ViewMessageLogic(this);
        }
        public ActionResult Index()
        {
            this._viewMessageLogic.SetViewMessageIfExist();

            var listaArea = this._areaReposity.GetAllAreas();
            Mapper.CreateMap<DisplayAreaModel, Area>().ReverseMap();
            //var listaAreas = listaArea.Select(Mapper.Map<DisplayAreaModel>);
            var listaAreaDisplaysModel = listaArea.Select(Mapper.Map<Area, DisplayAreaModel>).ToList();
            return View(listaAreaDisplaysModel);

            
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(this._areaReposity.Query(x => x), "Id", "Name");
            return View("Create");
        }

        
        [HttpPost]
        public ActionResult Create(AreaRegisterModel modelArea)
        {

            var area = new Mhotivo.Data.Entities.Area
            {
                Name = modelArea.DisplayName,
            };
            

            var myarea = this._areaReposity.Create(area);

            const string title = "Area Agregada";
            var content = "El area " + area.Name + " ha sido agregada exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            Area area = this._areaReposity.Delete(id);

            const string title = "Area Eliminada";
            var content = "El area " + area.Name + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            Area thisArea = this._areaReposity.GetById(id);

            //var area = Mapper.Map<AreaEditModel>(thisArea);
            Mapper.CreateMap<AreaEditModel, Area>().ReverseMap();
            var area = Mapper.Map<Area,AreaEditModel>(thisArea);

            area.DisplayName = thisArea.Name;

            return View("Edit", area);
        }

        [HttpPost]
        public ActionResult Edit(AreaEditModel modelArea)
        {           

            //var myArea = this._areaReposity.GetById(modelArea.Id);
            //Mapper.CreateMap<Area,AreaEditModel>().ReverseMap();
            var myArea = Mapper.Map<Area>(modelArea);

            myArea.Name = modelArea.DisplayName;
            
            Area area = this._areaReposity.Update(myArea);

            const string title = "Area Actualizada";
            var content = "El area" + area.Name + " ha sido actualizado exitosamente.";

            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);

            return RedirectToAction("Index");
        }
    }
}
