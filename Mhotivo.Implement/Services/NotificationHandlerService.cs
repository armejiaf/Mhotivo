using System.Collections.Generic;
using System.Linq;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Services
{
    public class NotificationHandlerService : INotificationHandlerService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IStudentRepository _suStudentRepository;
        private readonly IAcademicYearGradeRepository _academicYearGradeRepository;
        private readonly IAcademicYearCourseRepository _academicYearCourseRepository;
        private readonly IUserRepository _userRepository;

        public NotificationHandlerService(INotificationRepository notificationRepository, IStudentRepository suStudentRepository, 
            IAcademicYearGradeRepository academicYearGradeRepository, IAcademicYearCourseRepository academicYearCourseRepository, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _suStudentRepository = suStudentRepository;
            _academicYearGradeRepository = academicYearGradeRepository;
            _academicYearCourseRepository = academicYearCourseRepository;
            _userRepository = userRepository;
        }

        public List<Notification> GetAllActiveOfType(NotificationType type)
        {
            return _notificationRepository.Filter(x => x.NotificationType == type && x.AcademicYear.IsActive).ToList();
        }

        public List<Notification> GetAllOfTypeAndYear(NotificationType type, long yearId)
        {
            return _notificationRepository.Filter(x => x.NotificationType == type && x.AcademicYear.Id == yearId).ToList();
        }

        public void SendNotification(Notification notification)
        {
            if (notification.Sent)
                return;
            switch (notification.NotificationType)
            {
                case NotificationType.General:
                    var allGrades = _academicYearGradeRepository.Filter(x => x.AcademicYear == notification.AcademicYear);
                    foreach (var grade in allGrades)
                    {
                        SendToStudents(grade.Students, notification);
                    }
                    notification.Sent = true;
                    break;
                case NotificationType.EducationLevel:
                    var gradesForLevel =
                        _academicYearGradeRepository.Filter(
                            x => x.Grade.EducationLevel.Id == notification.DestinationId &&
                                 x.AcademicYear == notification.AcademicYear);
                    foreach (var grade in gradesForLevel)
                    {
                        SendToStudents(grade.Students, notification);
                    }
                    notification.Sent = true;
                    break;
                case NotificationType.Grade:
                    var grades =
                        _academicYearGradeRepository.Filter(x => x.Grade.Id == notification.DestinationId &&
                                                                 x.AcademicYear == notification.AcademicYear);
                    foreach (var grade in grades)
                    {
                        SendToStudents(grade.Students, notification);
                    }
                    notification.Sent = true;
                    break;
                case NotificationType.Section:
                    var singleGrade =
                        _academicYearGradeRepository.Filter(x => x.Id == notification.DestinationId &&
                                                                 x.AcademicYear == notification.AcademicYear).FirstOrDefault();
                    if (singleGrade != null)
                    {
                        SendToStudents(singleGrade.Students, notification);
                        notification.Sent = true;
                    }
                    break;
                case NotificationType.Course:
                    var course = _academicYearCourseRepository.Filter(x => x.Id == notification.DestinationId &&
                                                                           x.AcademicYearGrade.AcademicYear ==
                                                                           notification.AcademicYear).FirstOrDefault();
                    if (course != null)
                    {
                        SendToStudents(course.AcademicYearGrade.Students, notification);
                        notification.Sent = true;
                    }
                    break;
                case NotificationType.Personal:
                    var singleStudent = _suStudentRepository.Filter(x => x.Id == notification.DestinationId).FirstOrDefault();
                    if (singleStudent != null)
                    {
                        SendToStudent(singleStudent, notification);
                        notification.Sent = true;
                    }
                    break;
            }
        }

        private void SendToStudent(Student student, Notification notification)
        {
            if (student.Tutor1 != null && !student.Tutor1.User.Notifications.Contains(notification))
            {
                student.Tutor1.User.Notifications.Add(notification);
                _userRepository.Update(student.Tutor1.User);
                if (notification.SendEmail)
                    EmailService.SendEmailToUser(student.Tutor1.User, notification);
            }
            if (student.Tutor2 != null && !student.Tutor2.User.Notifications.Contains(notification))
            {
                student.Tutor2.User.Notifications.Add(notification);
                _userRepository.Update(student.Tutor2.User);
                if (notification.SendEmail)
                    EmailService.SendEmailToUser(student.Tutor2.User, notification);
            }
        }

        private void SendToStudents(IEnumerable<Student> students, Notification notification)
        {
            foreach (var student in students)
            {
                SendToStudent(student, notification);
            }
        }

        public void SendAllPending()
        {
            var notifications =
                _notificationRepository.Filter(x => x.Approved && !x.Sent && x.AcademicYear.IsActive);
            foreach (var notification in notifications)
            {
                SendNotification(notification);
            }
        }
    }
}
