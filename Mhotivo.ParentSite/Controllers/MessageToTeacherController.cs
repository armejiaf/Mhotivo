using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Authorization;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    public class MessageToTeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUserRepository _userRepository;

        public MessageToTeacherController(ITeacherRepository teacherRepository, IUserRepository userRepository)
        {
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
        }

        //
        // GET: /MessageToTeacher/
        [VerifyEmail]
        public ActionResult Index()
        {
            var allTeachers = _teacherRepository.GetAllTeachers().ToList();
            var allTeachersModel = new List<TeacherModel>();
            foreach (var teacher in allTeachers)
            {

                allTeachersModel.Add(new TeacherModel()
                {
                    Name = teacher.FullName,
                    Email = teacher.MyUser.Email
                });
            }
            return View(new Tuple<IEnumerable<TeacherModel>, MessageToTeacherModel>(allTeachersModel,null));
        }

        public ActionResult SendNewMessage([Bind(Prefix = "Item2")] MessageToTeacherModel model)
        {
            if (HttpContext.Session != null)
            {
                SendEmail.SendEmailToSingleUser(model.To, HttpContext.Session["loggedUserEmail"].ToString(),
                    model.Message, model.Subject);
                ViewBag.Message = "Mensaje Enviado!";
            }
            else
            {
                ViewBag.Message = "Mensaje No Enviado!";
            }
            return RedirectToAction("Index");
        }
    }
}
