using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.ParentSite.Models;
using Microsoft.Ajax.Utilities;

namespace Mhotivo.ParentSite.Controllers
{
    public class HomeworkController : Controller
    {
        //Bunch of unused repositories. Delete?
        private readonly IAcademicCourseRepository _academicCourseRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IHomeworkRepository _homeworkRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ICourseRepository _courseRepository;
        public static IStudentRepository StudentRepository;
        private readonly ISessionManagementService _sessionManagementService;
        public static ISecurityService SecurityService;
        private readonly ITutorRepository _tutorRepository;
        public static List<long> StudentsId;

        public HomeworkController(IHomeworkRepository homeworkRepository,
            IAcademicCourseRepository academicCourseRepository, IAcademicYearRepository academicYearRepository,
            IGradeRepository gradeRepository, ICourseRepository courseRepository, IStudentRepository studentRepository,
            ISessionManagementService sessionManagementService,
            ISecurityService securityService, ITutorRepository tutorRepository)
        {
            _homeworkRepository = homeworkRepository;
            _academicYearRepository = academicYearRepository;
            _gradeRepository = gradeRepository;
            _courseRepository = courseRepository;
            _academicCourseRepository = academicCourseRepository;
            StudentRepository = studentRepository;
            _sessionManagementService = sessionManagementService;
            SecurityService = securityService;
            _tutorRepository = tutorRepository;
        }

        public ActionResult Index(long student)
        {
            var students = GetAllStudents(GetTutorId());
            var homeworks = new List<Homework>();
            if (student != -1)
            {
                foreach (var academicCourse in students.Select(student1 => student1.MyGrade.CoursesDetails).SelectMany(courses => courses))
                {
                    homeworks.AddRange(academicCourse.Homeworks.Distinct());
                }
            }
            else
            {
                var student1 = students.FirstOrDefault(x => x.Id == student);
                if (student1 != null)
                {
                    foreach (var academicCourse in student1.MyGrade.CoursesDetails.Select(courses => courses))
                    {
                        homeworks.AddRange(academicCourse.Homeworks);
                    }
                }
            }
            var mappedHomeWorksModel = homeworks.Select(Mapper.Map<HomeworkModel>).ToList();
            return View(mappedHomeWorksModel);
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

        public static IEnumerable<Student> GetAllStudents(long tutorId)
        {
            IEnumerable<Student> allStudents =
                StudentRepository.GetAllStudents().Where(x => x.Tutor1.Id.Equals(tutorId));
            return allStudents;
        }


        public static List<string> GetStudentName(long academicyearId)
        {
            //var enroll = GetEnrollsbyAcademicYear(academicyearId);
            //return enroll.Select(e => e.Student.FirstName).ToList();
            return null;
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
    }
}