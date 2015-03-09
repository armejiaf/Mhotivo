using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class GradeController : Controller
    {
        #region private members
        private readonly IGradeRepository _gradeRepository;
        private readonly ViewMessageLogic _viewMessageLogic;

        public GradeController(IGradeRepository gradeRepository)
        {
            if (gradeRepository == null) throw new ArgumentNullException("gradeRepository");

            _gradeRepository = gradeRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }
        #endregion

        #region public methods

        /// <summary>
        /// GET: /Grade/
        /// </summary>
        /// <returns />
        public ActionResult Index()
        {
            _viewMessageLogic.SetViewMessageIfExist();
            var grades = _gradeRepository.GetAllGrade();

            Mapper.CreateMap<DisplayGradeModel, Grade>().ReverseMap();
            var displayGradeModels = grades.Select(Mapper.Map<Grade, DisplayGradeModel>).ToList();

            return View(displayGradeModels);
        }

        /// <summary>
        /// GET: /Grade/Add
        /// </summary>
        /// <returns />
        [HttpGet]
        public ActionResult Add()
        {
            return View("Create");
        }

        /// <summary>
        /// POST: /Grade/Add
        /// </summary>
        /// <param name="modelGrade" />
        /// <returns />
        [HttpPost]
        public ActionResult Add(GradeRegisterModel modelGrade)
        {
            string title;
            string content;

            Mapper.CreateMap<Grade, GradeRegisterModel>().ReverseMap();
            var gradeModel = Mapper.Map<GradeRegisterModel, Grade>(modelGrade);

            var myGrade = _gradeRepository.GenerateGradeFromRegisterModel(gradeModel);

            var existGrade =
                _gradeRepository.GetAllGrade()
                    .FirstOrDefault(
                        g => g.Name.Equals(modelGrade.Name) && g.EducationLevel.Equals(modelGrade.EducationLevel));

            if (existGrade != null)
            {
                title = "Grado";
                content = "El grado " + existGrade.Name + " ya existe.";
                _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);
                return RedirectToAction("Index");
            }


            var grade = _gradeRepository.Create(myGrade);
            title = "Grado Agregado";
            content = grade.Name + " grado ha sido guardado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.SuccessMessage);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// POST: /Grade/Delete/5
        /// </summary>
        /// <param name="id" />
        /// <returns />
        [HttpPost]
        public ActionResult Delete(long id)
        {
            var grade = _gradeRepository.Delete(id);

            const string title = "Grado ha sido Eliminado";
            var content = grade.Name + " ha sido eliminado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// GET: /Grade/Edit/5
        /// </summary>
        /// <param name="id" />
        /// <returns />
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var grade = _gradeRepository.GetGradeEditModelById(id);
            Mapper.CreateMap<GradeEditModel, Grade>().ReverseMap();
            var gradeModel = Mapper.Map<Grade, GradeEditModel>(grade);

            return View("Edit", gradeModel);
        }

        /// <summary>
        ///  POST: /Grade/Edit/5
        /// </summary>
        /// <param name="modelGrade" />
        /// <returns />
        [HttpPost]
        public ActionResult Edit(GradeEditModel modelGrade)
        {
            var myGrade = _gradeRepository.GetById(modelGrade.Id);
            Mapper.CreateMap<Grade, GradeEditModel>().ReverseMap();

            var gradeModel = Mapper.Map<GradeEditModel, Grade>(modelGrade);
            _gradeRepository.UpdateGradeFromGradeEditModel(gradeModel, myGrade);

            const string title = "Grado Actualizado";
            var content = myGrade.Name + " grado ha sido actualizado exitosamente.";
            _viewMessageLogic.SetNewMessage(title, content, ViewMessageType.InformationMessage);

            return RedirectToAction("Index");
        }

        #endregion
    }
}