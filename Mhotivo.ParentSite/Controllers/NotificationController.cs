using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IParentRepository _parentRepository;
        private readonly ISessionManagementService _sessionManagementService; //Will this be used?
        private readonly ISecurityService _securityService;
        private Parent _loggedParent;
        public static IStudentRepository StudentRepository;
        public static IEnrollRepository EnrollsRepository;
        public static ISecurityService SecurityService;

        public static List<long> StudentsId;

        public NotificationController(INotificationRepository notificationRepository, IAcademicYearRepository academicYearRepository,
            IParentRepository parentRepository, ISessionManagementService sessionManagementService, ISecurityService securityService,
            IStudentRepository studentRepository, IEnrollRepository enrollsRepository)
        {
            _notificationRepository = notificationRepository;
            _academicYearRepository = academicYearRepository;
            _parentRepository = parentRepository;
            _sessionManagementService = sessionManagementService;
            _securityService = securityService;
            StudentRepository = studentRepository;
            EnrollsRepository = enrollsRepository;
            SecurityService = securityService;
        }

        // GET: /Notification/
        [HttpGet]
        public ActionResult Index(string filter)
        {
            var loggedUserEmail = _securityService.GetUserLoggedEmail();
            _loggedParent = _parentRepository.Filter(y => y.User.Email == loggedUserEmail).FirstOrDefault();
            if (_loggedParent == null)
                return RedirectToAction("Index", "Home");

            List<Notification> notifications;

            switch (filter)
            {
                case "NDE":
                    notifications =
                        _loggedParent
                            .User.Notifications.Where(x => x.NotificationType == NotificationType.EducationLevel && x.AcademicYear.IsActive)
                            .ToList();
                    break;
                case "Grado":
                    notifications =
                        _loggedParent
                            .User.Notifications.Where(x => x.NotificationType == NotificationType.Grade && x.AcademicYear.IsActive)
                            .ToList();
                    break;

                case "Personal":
                    notifications =
                        _loggedParent
                            .User.Notifications.Where(x => x.NotificationType == NotificationType.Personal && x.AcademicYear.IsActive)
                            .ToList();
                    break;
                default:
                    notifications =
                        _loggedParent
                            .User.Notifications.Where(x => x.NotificationType == NotificationType.General && x.AcademicYear.IsActive)
                            .ToList();
                    break;
            }
           

            var notificationsModel = notifications.Select(Mapper.Map<Notification, NotificationModel>).ToList();


            notificationsModel = notificationsModel.OrderByDescending(x => x.Created).ToList();

            return View(notificationsModel);
        }

        public static IEnumerable<Student> GetAllStudents(long parentId)
        {
            IEnumerable<Student> allStudents =
                StudentRepository.GetAllStudents().Where(x => x.Tutor1.Id.Equals(parentId));
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

        public static IEnumerable<Enroll> GetAllEnrolls(long studentId)
        {
            IEnumerable<Enroll> allEnrolls =
                EnrollsRepository.GetAllsEnrolls().Where(x => x.Student.Id == studentId);
            return allEnrolls;
        }

        public static IEnumerable<Enroll> GetAllEnrolls(List<long> studentId)
        {
            IEnumerable<Enroll> allEnrolls =
                EnrollsRepository.GetAllsEnrolls().Where(x => studentId.Contains(x.Student.Id));
            return allEnrolls;
        }

        public static IEnumerable<Enroll> GetEnrollsbyAcademicYear(long academicyear)
        {
            IEnumerable<Enroll> allEnrolls =
                EnrollsRepository.GetAllsEnrolls().Where(x => x.AcademicGrade.AcademicYear.Id == academicyear && StudentsId.Contains(x.Student.Id));
            return allEnrolls;
        }

        public static List<string> GetStudentName(long academicyearId)
        {
            var enroll = GetEnrollsbyAcademicYear(academicyearId);
            return enroll.Select(e => e.Student.FirstName).ToList();
        }

        public static string GetStudenById(long studentId)
        {
            return StudentRepository.GetById(studentId).FirstName;
        }

        public static long GetParentId()
        {
            var people = SecurityService.GetUserLoggedPeoples();
            long id = 0;
            foreach (var p in people)
            {
                if (p is Parent)
                    id = p.Id;
            }
            return id;
        }

        public ActionResult AddCommentToNotification(int notificationId, string commentText)
        {
            var loggedUserEmail = System.Web.HttpContext.Current.Session["loggedUserEmail"].ToString();
            _loggedParent = _parentRepository.Filter(y => y.User.Email == loggedUserEmail).FirstOrDefault();
            var selectedNotification = _notificationRepository.GetById(notificationId);
            selectedNotification.NotificationComments.Add(new NotificationComment
            {
                CommentText = commentText,
                CreationDate = DateTime.Now,
                Commenter = _loggedParent.User
            });
            _notificationRepository.Update(selectedNotification);
            return RedirectToAction("Index");
        }
    }
}
