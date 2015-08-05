using System;
using System.Linq;
//using Mhotivo.App_Data.Repositories;
//using Mhotivo.App_Data.Repositories.Interfaces;

using Mhotivo.Interface.Interfaces;
using Mhotivo.Implement.Context;
using Mhotivo.Implement.Repositories;
using Mhotivo.Data.Entities;

using Mhotivo.Models;

//TODO: Seemingly deprecated. Confirm if it will serve any purpose at all.
namespace Mhotivo.Logic
{
    public class AcademicYearLogic
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IPensumRepository _pensumRepo;
        private readonly IAcademicYearRepository _academicYearRepo;

        public AcademicYearLogic(ICourseRepository courseRepo, IPensumRepository pensumRepo, IAcademicYearRepository academicYearRepo)
        {
            _courseRepo = courseRepo;
            _pensumRepo = pensumRepo;
            _academicYearRepo = academicYearRepo;
        }

        public void GenerateSectionForGrades()
        {
            //var courses = _courseRepo.Query(x => x).ToList();

            //foreach (var currentCourse in courses)
            //{
            //    Course course = currentCourse;
            //    var pensums = _pensumRepo.Filter(x => x.Course.Id == course.Id).ToList();
            //    foreach (var pensum in pensums)
            //    {
            //        var academicYear = new AcademicYear
            //        {
            //            Teacher = null,
            //            Approved = false,
            //            Course = currentCourse,
            //            Grade = pensum.Grade,
            //            IsActive = true,
            //            Room = "",
            //            Section = 'A',
            //            StudentsCount = 0,
            //            StudentsLimit = 25,
            //            Year = DateTime.Now,
            //            Schedule = null,
            //            TeacherEndDate = null,
            //            TeacherStartDate = null
            //        };
            //        _academicYearRepo.Create(academicYear);
            //    }
            //}
        }
    }
}