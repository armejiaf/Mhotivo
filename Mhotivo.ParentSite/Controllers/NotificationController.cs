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
        private readonly ISessionManagementRepository _sessionManagementRepository; //Will this be used?
        private readonly ISecurityRepository _securityRepository;
        private Parent _loggedParent;
        public static IStudentRepository StudentRepository;
        public static IEnrollRepository EnrollsRepository;
        public static ISecurityRepository SecurityRepository;

        public static List<long> StudentsId;

        public NotificationController(INotificationRepository notificationRepository, IAcademicYearRepository academicYearRepository,
            IParentRepository parentRepository, ISessionManagementRepository sessionManagementRepository, ISecurityRepository securityRepository,
            IStudentRepository studentRepository, IEnrollRepository enrollsRepository)
        {
            _notificationRepository = notificationRepository;
            _academicYearRepository = academicYearRepository;
            _parentRepository = parentRepository;
            _sessionManagementRepository = sessionManagementRepository;
            _securityRepository = securityRepository;
            StudentRepository = studentRepository;
            EnrollsRepository = enrollsRepository;
            SecurityRepository = securityRepository;
        }

        // GET: /Notification/
        [HttpGet]
        public ActionResult Index()
        {
            var currentAcademicYear = Convert.ToInt32(_academicYearRepository.GetCurrentAcademicYear().Year.ToString(CultureInfo.InvariantCulture));
            var loggedUserEmail = _securityRepository.GetUserLoggedEmail();
            _loggedParent = _parentRepository.Filter(y => y.MyUser.Email == loggedUserEmail).FirstOrDefault();
            var userId = _securityRepository.GetUserLogged().Id;
            var personalNotifications = _notificationRepository.GetPersonalNotifications(currentAcademicYear, userId).ToList();
            var notifications = _notificationRepository.GetGradeNotifications(currentAcademicYear, userId).ToList();
            notifications.AddRange(_notificationRepository.GetAreaNotifications(currentAcademicYear, userId).ToList());
            notifications.AddRange(_notificationRepository.GetGeneralNotifications(currentAcademicYear).ToList());
            var personalNotificationsModel = new List<NotificationModel>();
            var notificationsModel = new List<NotificationModel>();
            foreach (var notification in personalNotifications)
            {
                var noti = Mapper.Map<NotificationModel>(notification);
                noti.CommentsAmount = notification.NotificationComments.Count;
                noti.NotificationCreator = notification.UserCreatorName;
               // if(noti.Id != )
                personalNotificationsModel.Add(noti);
                
            }
            foreach (var notification in notifications)
            {
                var noti = Mapper.Map<Notification, NotificationModel>(notification);
                noti.CommentsAmount = notification.NotificationComments.Count;
                noti.NotificationCreator = notification.UserCreatorName;
                notificationsModel.Add(noti);
            }
            personalNotificationsModel = personalNotificationsModel.OrderByDescending(x => x.Created).ToList();
            notificationsModel = notificationsModel.OrderByDescending(x => x.Created).ToList();
            return View(new Tuple<List<NotificationModel>, List<NotificationModel>>(personalNotificationsModel, notificationsModel));
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
                EnrollsRepository.GetAllsEnrolls().Where(x => x.AcademicYear.Id == academicyear && StudentsId.Contains(x.Student.Id));
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
            var people = SecurityRepository.GetUserLoggedPeoples();
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
            _loggedParent = _parentRepository.Filter(y => y.MyUser.Email == loggedUserEmail).FirstOrDefault();
            var selectedNotification = _notificationRepository.GetById(notificationId);
            selectedNotification.NotificationComments.Add(new NotificationComments
            {
                CommentText = commentText,
                CreationDate = DateTime.Now,
                Parent = _loggedParent
            });
            _notificationRepository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
