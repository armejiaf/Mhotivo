using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite.Controllers
{
    public class HomeworkController : Controller
    {
        //Bunch of unused repositories. Delete?
        private readonly IAcademicYearDetailsRepository _academicYearDetailRepository;
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
            IAcademicYearDetailsRepository academicYearDetailRepository, IAcademicYearRepository academicYearRepository,
            IGradeRepository gradeRepository, ICourseRepository courseRepository, IStudentRepository studentRepository,
            IEnrollRepository enrollsRepository, ISessionManagementRepository sessionManagementRepository, 
            ISecurityRepository securityRepository, IParentRepository parentRepository)
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
            var enumerable = allHomeworks as Homework[] ?? allHomeworks.ToArray();
            IEnumerable<HomeworkModel> allHomeworksModel =
                enumerable
                    .Where(
                        homework =>
                            enrolls.Any(enroll => enroll.AcademicYear.Id == homework.AcademicYearDetail.AcademicYear.Id))
                    .Select(Mapper.Map<Homework, HomeworkModel>)
                    .ToList();
            //Unused. Remove?
            /*if (date != null)
            {
                if (date.Equals("Dia"))
                {
                    allHomeworks = enumerable.Where(x => x.DeliverDate <= DateTime.Now.AddDays(1));
                }
                else if (date.Equals("Semana"))
                {
                    DateTime compareDate = DateTime.Today.AddDays((-(int)DateTime.Today.DayOfWeek) + 7);
                    allHomeworks = enumerable.Where(x => x.DeliverDate <= compareDate);
                }
                else if (date.Equals("Mes"))
                {
                    allHomeworks = enumerable.Where(x => x.DeliverDate.Month == DateTime.Now.Month);
                }
            }*/
            return View(allHomeworksModel);
        }

        private List<long> GetAllStudentsId(IEnumerable<Student> students)
        {
            var studentsId = new List<long>();
            var enumerable = students as Student[] ?? students.ToArray();
            for (int i = 0; i < enumerable.Count(); i++)
            {
                studentsId.Add(enumerable.ElementAt(i).Id);
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
    }
}
