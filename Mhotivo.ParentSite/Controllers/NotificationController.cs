using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Authorization;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly ISecurityService _securityService;
        private Tutor _loggedTutor;
        public static IStudentRepository StudentRepository;
        public static ISecurityService SecurityService;

        public static List<long> StudentsId;

        public NotificationController(INotificationRepository notificationRepository, IAcademicYearRepository academicYearRepository,
            ITutorRepository tutorRepository, ISecurityService securityService,
            IStudentRepository studentRepository)
        {
            _notificationRepository = notificationRepository;
            _academicYearRepository = academicYearRepository;
            _tutorRepository = tutorRepository;
            _securityService = securityService;
            StudentRepository = studentRepository;
            SecurityService = securityService;
        }

        // GET: /Notification/
        [HttpGet]
        [AuthorizeNewUser]
        public ActionResult Index(string filter)
        {
            var loggedUserEmail = _securityService.GetUserLoggedEmail();
            _loggedTutor = _tutorRepository.Filter(y => y.User.Email == loggedUserEmail).FirstOrDefault();
            if (_loggedTutor == null)
                return RedirectToAction("Index", "Home");

            List<Notification> notifications;

            switch (filter)
            {
                case "NDE":
                    notifications =
                        _loggedTutor
                            .User.Notifications.Where(x => x.NotificationType == NotificationType.EducationLevel && x.AcademicYear.IsActive)
                            .ToList();
                    break;
                case "Grado":
                    notifications =
                        _loggedTutor
                            .User.Notifications.Where(x => x.NotificationType == NotificationType.Grade && x.AcademicYear.IsActive)
                            .ToList();
                    break;

                case "Personal":
                    notifications =
                        _loggedTutor
                            .User.Notifications.Where(x => x.NotificationType == NotificationType.Personal && x.AcademicYear.IsActive)
                            .ToList();
                    break;
                default:
                    notifications =
                        _loggedTutor
                            .User.Notifications.Where(x => x.NotificationType == NotificationType.General && x.AcademicYear.IsActive)
                            .ToList();
                    break;
            }


            var notificationsModel = notifications.Select(Mapper.Map<Notification, NotificationModel>).ToList();


            notificationsModel = notificationsModel.OrderByDescending(x => x.Created).ToList();

            return View(notificationsModel);
        }

        public static IEnumerable<Student> GetAllStudents(long tutorId)
        {
            IEnumerable<Student> allStudents =
                StudentRepository.GetAllStudents().Where(x => x.Tutor1.Id.Equals(tutorId));
            return allStudents;
        }

        private static List<long> GetAllStudentsId(IEnumerable<Student> students)
        {
            var studentsId = new List<long>();
            var enumerable = students as Student[] ?? students.ToArray();
            for (int i = 0; i < enumerable.Count(); i++)
            {
                studentsId.Add(enumerable.ElementAt(i).Id);
            }
            return studentsId;
        }

        public static string GetStudenById(long studentId)
        {
            return StudentRepository.GetById(studentId).FirstName;
        }

        public static long GetTutorId()
        {
            var people = SecurityService.GetUserLoggedPeoples();
            long id = 0;
            foreach (var p in people)
            {
                if (p is Tutor)
                    id = p.Id;
            }
            return id;
        }
        [AuthorizeNewUser]
        public ActionResult AddCommentToNotification(int notificationId, string commentText)
        {
            var loggedUserEmail = System.Web.HttpContext.Current.Session["loggedUserEmail"].ToString();
            _loggedTutor = _tutorRepository.Filter(y => y.User.Email == loggedUserEmail).FirstOrDefault();
            var selectedNotification = _notificationRepository.GetById(notificationId);
            selectedNotification.NotificationComments.Add(new NotificationComment
            {
                CommentText = commentText,
                CreationDate = DateTime.Now,
                Commenter = _loggedTutor.User
            });
            _notificationRepository.Update(selectedNotification);
            return RedirectToAction("Index");
        }
    }
}