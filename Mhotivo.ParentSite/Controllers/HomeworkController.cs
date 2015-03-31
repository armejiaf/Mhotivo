using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.WebPages;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Repositories;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;
using Microsoft.Ajax.Utilities;

namespace Mhotivo.ParentSite.Controllers
{
    public class HomeworkController : Controller
    {
        // GET: /Homework/
        private readonly IAcademicYearDetailRepository _academicYearDetailRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ICourseRepository _courseRepository;
        public static IStudentRepository StudentRepository;
        public static IEnrollRepository EnrollsRepository;
        private readonly ISessionManagementRepository _sessionManagementRepository;
        public static ISecurityRepository SecurityRepository;
        private readonly IParentRepository _parentRepository;
        public static List<long> StudentsId;

        public HomeworkController(IHomeworkRepository homeworkRepository,
            IAcademicYearDetailRepository academicYearDetailRepository, IAcademicYearRepository academicYearRepository,
            IGradeRepository gradeRepository, ICourseRepository courseRepository, IStudentRepository studentRepository,
            IEnrollRepository enrollsRepository, ISessionManagementRepository sessionManagementRepository, ISecurityRepository securityRepository, IParentRepository parentRepository
            )
        {
            _homeworkRepository = homeworkRepository;
            _academicYearRepository = academicYearRepository;
            _gradeRepository = gradeRepository;
            _courseRepository = courseRepository;
            _academicYearDetailRepository = academicYearDetailRepository;
            StudentRepository = studentRepository;
            EnrollsRepository = enrollsRepository;
            _sessionManagementRepository = sessionManagementRepository;
            SecurityRepository = securityRepository;
            _parentRepository = parentRepository;
        }

        public ActionResult Index(string param, string student, string date)
        {
            var students = GetAllStudents(GetParentId());
            StudentsId = GetAllStudentsId(students);

            var enrolls = GetAllEnrolls(StudentsId).ToList();

            if (!student.IsEmpty())
                enrolls = enrolls.Where(x => x.Student.Id == Convert.ToInt32(student)).ToList();

            IEnumerable<Homework> allHomeworks = _homeworkRepository.GetAllHomeworks().Where(x => x.DeliverDate.Date >= DateTime.Now);
            Mapper.CreateMap<HomeworkModel, Homework>().ReverseMap();
            DateTime compareDate = DateTime.Now.AddDays(1);
            IEnumerable<HomeworkModel> allHomeworksModel =
                allHomeworks
                    .Where(
                        homework =>
                            enrolls.Any(enroll => enroll.AcademicYear.Id == homework.AcademicYearDetail.AcademicYear.Id))
                    .Select(Mapper.Map<Homework, HomeworkModel>)
                    .ToList();
            //                .Where(x => enrolls.Any(enroll => enroll.AcademicYear.Id == x.AcademicYearDetail.AcademicYear.Id));
            if (date != null)
            {
                if (date.Equals("Dia"))
                {
                    allHomeworks = allHomeworks.Where(x => x.DeliverDate <= DateTime.Now.AddDays(1));
                }
                else if (date.Equals("Semana"))
                {
                    compareDate = DateTime.Today.AddDays((-(int)DateTime.Today.DayOfWeek) + 7);
                    allHomeworks = allHomeworks.Where(x => x.DeliverDate <= compareDate);
                }
                else if (date.Equals("Mes"))
                {
                    allHomeworks = allHomeworks.Where(x => x.DeliverDate.Month == DateTime.Now.Month);
                }
            }
            return View(allHomeworksModel);
        }

        private List<long> GetAllStudentsId(IEnumerable<Student> students)
        {
            var studentsId = new List<long>();
            for (int i = 0; i < students.Count(); i++)
            {
                studentsId.Add(students.ElementAt(i).Id);
            }
            return studentsId;
        }


        public static IEnumerable<Student> GetAllStudents(long parentId)
        {
            IEnumerable<Student> allStudents =
                StudentRepository.GetAllStudents().Where(x => x.Tutor1.Id.Equals(parentId));
            return allStudents;
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

        public static List<string> GetStudentName(long AcademicyearId)
        {
            var enroll = GetEnrollsbyAcademicYear(AcademicyearId);
            List<string> studentsNames = new List<string>();
            foreach (var e in enroll)
            {
                studentsNames.Add(e.Student.FirstName);
            }
            return studentsNames;
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
    }
}
