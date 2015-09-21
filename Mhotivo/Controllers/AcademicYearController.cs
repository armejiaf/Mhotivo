using System;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Authorizations;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using PagedList;

namespace Mhotivo.Controllers
{
    public class AcademicYearController : Controller
    {
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public AcademicYearController(IAcademicYearRepository academicYearRepository, IGradeRepository gradeRepository)
        {
            _academicYearRepository = academicYearRepository;
            _gradeRepository = gradeRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        [AuthorizeAdmin]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var allAcademicYears = _academicYearRepository.GetAllAcademicYears();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "year_desc" : "";
            ViewBag.GradeSortParm = sortOrder == "Grade" ? "grade_desc" : "Grade";
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
                try
                {
                    var year = Convert.ToInt32(searchString);
                    allAcademicYears = _academicYearRepository.Filter(x => x.Year.Equals(year)).ToList();
                }
                catch (Exception e)
                {
                   //ignore
                }
            }
            var academicYears = allAcademicYears.Select(academicYear => new DisplayAcademicYearModel
            {
                Id = academicYear.Id,
                Year = academicYear.Year,
                Section = academicYear.Section,
                Approved = academicYear.Approved,
                IsActive = academicYear.IsActive,
                EducationLevel = academicYear.Grade.EducationLevel,
                Grade = academicYear.Grade.Name
            }).ToList();
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "year_desc":
                    academicYears = academicYears.OrderByDescending(s => s.Year).ToList();
                    break;
                case "Grade":
                    academicYears = academicYears.OrderBy(s => s.Grade).ToList();
                    break;
                case "grade_desc":
                    academicYears = academicYears.OrderByDescending(s => s.Grade).ToList();
                    break;
                default:  // Name ascending 
                    academicYears = academicYears.OrderBy(s => s.Year).ToList();
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(academicYears.ToPagedList(pageNumber,pageSize));
        }


        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Edit(int id)
        {
            var academicYear = _academicYearRepository.GetById(id);
            var academicYearModel = new AcademicYearEditModel
            {
                Id =academicYear.Id,
                Year =  academicYear.Year,
                Grade = academicYear.Grade,
                Section = academicYear.Section,
                EducationLevel = academicYear.Grade.EducationLevel,
                Approved = academicYear.Approved.ToString()
            };
            ViewBag.GradeId = new SelectList(_gradeRepository.Query(x => x), "Id", "Name", academicYearModel.Grade.Id);
            return View("Edit", academicYearModel);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Edit(AcademicYearEditModel modelAcademicYear)
        {
            var myAcademicYear = _academicYearRepository.GetById(modelAcademicYear.Id);
            var year = myAcademicYear.Year;
            var yearModel = new DateTime(modelAcademicYear.Year, 01, 01);
            myAcademicYear.Year = yearModel.Year;
            if (modelAcademicYear.Approved.Equals("1") || modelAcademicYear.Approved.Equals("Sí"))
                myAcademicYear.Approved = true;
            else
                myAcademicYear.Approved = false;

            if (modelAcademicYear.Approved.Equals("1") || modelAcademicYear.Approved.Equals("Sí"))
                myAcademicYear.IsActive = true;
            else
                myAcademicYear.IsActive = false;
            myAcademicYear.Grade = _gradeRepository.GetById(modelAcademicYear.Grade.Id);
            myAcademicYear.Section = modelAcademicYear.Section;
            _academicYearRepository.Update(myAcademicYear);
            const string title = "Año Académico Actualizado ";
            var content = "El año académico " + year + " ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Delete(long id)
        {
            var academicYear = _academicYearRepository.Delete(id);
            const string title = "Año Académico Eliminado";
            var content = "El año académico " + academicYear.Year + ", "+academicYear.Grade.Name+", "+academicYear.Section+" ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult Add()
        {
            ViewBag.GradeId = new SelectList(_gradeRepository.Query(x => x), "Id", "Name", 0);
            return View("Create");
        }

        [HttpPost]
        [AuthorizeAdmin]
        public ActionResult Add(AcademicYearRegisterModel academicYearModel)
        {
            var year = new DateTime(academicYearModel.Year, 01, 01);
            var approved = false;
            var isActive = false;
            if (academicYearModel.Approved == "1")
                approved = true;
            if (academicYearModel.IsActive == "1")
                isActive = true;
            var academicYear = new AcademicYear
            {
                Year = year.Year,
                Grade = _gradeRepository.GetById(academicYearModel.Grade.Id),
                Section = academicYearModel.Section,
                Approved = approved,
                IsActive = isActive
            };
            _academicYearRepository.Create(academicYear);
            const string title = "Año Académico Agregado";
            var content = "El año académico " + academicYearModel.Year + " ha sido agregado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);
            return RedirectToAction("Index");
        }
    }
}