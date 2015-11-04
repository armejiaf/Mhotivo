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
        private readonly IAcademicYearCourseRepository _academicYearCourseRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ICourseRepository _courseRepository;
        public static IStudentRepository StudentRepository;
        public static IEnrollRepository EnrollsRepository;
        private readonly ISessionManagementService _sessionManagementService;
        public static ISecurityService SecurityService;
        private readonly IParentRepository _parentRepository;
        public static List<long> StudentsId;

        public HomeworkController(IHomeworkRepository homeworkRepository,
            IAcademicYearCourseRepository academicYearCourseRepository, IAcademicYearRepository academicYearRepository,
            IGradeRepository gradeRepository, ICourseRepository courseRepository, IStudentRepository studentRepository,
            IEnrollRepository enrollsRepository, ISessionManagementService sessionManagementService, 
            ISecurityService securityService, IParentRepository parentRepository)
        {
            _homeworkRepository = homeworkRepository;
            _academicYearRepository = academicYearRepository;
            _gradeRepository = gradeRepository;
            _courseRepository = courseRepository;
            _academicYearCourseRepository = academicYearCourseRepository;
            StudentRepository = studentRepository;
            EnrollsRepository = enrollsRepository;
            _sessionManagementService = sessionManagementService;
            SecurityService = securityService;
            _parentRepository = parentRepository;
        }

        public ActionResult Index(string param,string student, string date)
        {
            var students = GetAllStudents(GetParentId());
            StudentsId = GetAllStudentsId(students);
            var enrolls = new List<Enroll>();
            enrolls.AddRange(GetAllEnrolls(StudentsId));
            if (student != null)
                enrolls = enrolls.FindAll(x => x.Student.Id == Convert.ToInt32(student));
           var allHomeworks = _homeworkRepository.Filter(x => x.DeliverDate >= DateTime.Today).ToList();
            switch (date)
            {
                case "Dia":
                    allHomeworks =
                        allHomeworks.FindAll(
                            x => x.DeliverDate == DateTime.Today.AddDays(1) || x.DeliverDate == DateTime.Today).ToList();
                    break;
                case "Semana":
                    allHomeworks =
                        allHomeworks.FindAll(
                            x => x.DeliverDate >= DateTime.Today && x.DeliverDate <= DateTime.Today.AddDays(7)).ToList();
                    break;
                case "Mes":
                    allHomeworks = allHomeworks.FindAll(x => x.DeliverDate.Month == DateTime.Today.Month).ToList();
                    break;
            }

            var mappedHomeWorksModel = allHomeworks.Select(Mapper.Map<HomeworkModel>).ToList();
            var allHomeworksModel = new List<HomeworkModel>();
            foreach (var enroll in enrolls)
            {
                allHomeworksModel.AddRange(mappedHomeWorksModel.FindAll(x => x.AcademicYearCourse.AcademicYearGrade.Id == enroll.AcademicYearGrade.AcademicYear.Id));
            }

            if (param != null)
                allHomeworksModel =
                    allHomeworksModel.FindAll(x => x.AcademicYearCourse.Course.Id == Convert.ToInt32(param));

            return View(allHomeworksModel);
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
                EnrollsRepository.GetAllsEnrolls().Where(x => x.AcademicYearGrade.AcademicYear.Id == academicyear && StudentsId.Contains(x.Student.Id));
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
    }
}
