using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using PagedList;

namespace Mhotivo.Controllers
{
    public class AcademicGradeController : Controller
    {
        private readonly IAcademicGradeRepository _academicGradeRepository;
        private readonly ViewMessageLogic _viewMessageLogic;
        private readonly IAcademicYearRepository _academicYearRepository;

        public AcademicGradeController(IAcademicGradeRepository academicGradeRepository, IAcademicYearRepository academicYearRepository)
        {
            _academicGradeRepository = academicGradeRepository;
            _academicYearRepository = academicYearRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        public ActionResult Index(long yearId, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var grades = _academicGradeRepository.Filter(x => x.AcademicYear.Id == yearId).ToList();
            ViewBag.IdAcademicYear = yearId;
            ViewBag.Year = _academicYearRepository.GetById(yearId).Year;
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    grades = _academicGradeRepository.Filter(x => x.AcademicYear.Id == yearId && (x.Section.Equals(searchString) || x.Grade.Name.Contains(searchString))).ToList();
                }
                catch (Exception)
                {
                    grades = _academicGradeRepository.Filter(x => x.AcademicYear.Id == yearId).ToList();
                }
            }
            ViewBag.CurrentFilter = searchString;
            var model = grades.Select(Mapper.Map<AcademicGradeDisplayModel>);
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

    }
}
