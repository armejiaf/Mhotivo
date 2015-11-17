using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class AcademicGradeController : Controller
    {
        private readonly IAcademicGradeRepository _academicGradeRepository;
        

        public ActionResult Index(long yearId)
        {
            var model = _academicGradeRepository.Filter(x => x.AcademicYear.Id == yearId).Select(Mapper.Map<AcademicGradeDisplayModel>);
            return View(model);
        }

    }
}
