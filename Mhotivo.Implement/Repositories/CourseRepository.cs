using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly MhotivoContext _context;

        public CourseRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Course> GetAllCourse()
        {
            return Query(c => c).ToList().Select(c => new Course
            {
                Id = c.Id,
                Name = c.Name,
                Area = new EducationLevel
                {
                    Id = c.Area.Id,
                    Name = c.Area.Name
                }
            });
        }

        public IEnumerable<EducationLevel> GetAllAreas()
        {
            return _context.EducationLevels.Select(a => a).ToList().Select(a => new EducationLevel
            {
                Id = a.Id,
                Name = a.Name
            });
        }

        public IQueryable<Course> Filter(Expression<Func<Course, bool>> expression)
        {
            var myCourses = _context.Courses.Where(expression);
            return myCourses;
        }

        public Course GenerateCourseFromRegisterModel(Course courseRegisterModel)
        {
            return new Course
            {
                Id = courseRegisterModel.Id,
                Name = courseRegisterModel.Name,
                Area = courseRegisterModel.Area
            };
        }

        public Course GetCourseEditModelById(long id)
        {
            var course = GetById(id);
            return new Course
            {
                Id = course.Id,
                Name = course.Name,
                Area = course.Area
            };
        }

        public Course UpdateCourseFromCourseEditModel(Course courseEditModel, Course course)
        {
            course.Id = courseEditModel.Id;
            course.Name = courseEditModel.Name;
            course.Area = courseEditModel.Area;

            return Update(course);
        }

        public Course Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Courses.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Course GetById(long id)
        {
            var courses = _context.Courses.Where(x => x.Id == id);
            return courses.Count() != 0 ? courses.First() : null;
        }

        public Course Create(Course itemToCreate)
        {
            var role = _context.Courses.Add(itemToCreate);
            _context.SaveChanges();
            return role;
        }

        public IQueryable<TResult> Query<TResult>(Expression<Func<Course, TResult>> expression)
        {
            return _context.Courses.Select(expression);

        }

        public IQueryable<TResult> QueryAreaResults<TResult>(Expression<Func<EducationLevel, TResult>> expression)
        {
            return _context.EducationLevels.Select(expression);
        }

        public Course Update(Course itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}