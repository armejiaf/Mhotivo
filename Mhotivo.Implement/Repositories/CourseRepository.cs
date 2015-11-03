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

        public Course Delete(Course itemToDelete)
        {
            _context.Courses.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Course> GetAllCourse()
        {
            return Query(c => c).ToList();
        }

        public IQueryable<Course> Filter(Expression<Func<Course, bool>> expression)
        {
            return _context.Courses.Where(expression);
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
            return _context.Courses.FirstOrDefault(x => x.Id == id);
        }

        public Course Create(Course itemToCreate)
        {
            var role = _context.Courses.Add(itemToCreate);
            _context.SaveChanges();
            return role;
        }

        public IQueryable<Course> Query(Expression<Func<Course, Course>> expression)
        {
            return _context.Courses.Select(expression);

        }

        public Course Update(Course itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }
    }
}