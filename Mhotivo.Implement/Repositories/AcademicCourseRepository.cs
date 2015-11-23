using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class AcademicCourseRepository : IAcademicCourseRepository
    {
        private readonly MhotivoContext _context;

        public AcademicCourseRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<AcademicCourse> GetAllAcademicYearDetails()
        {
            return Query(x => x).ToList();
        }

        public AcademicCourse GetById(long id)
        {
            return _context.AcademicYearCourses.FirstOrDefault(x => x.Id == id);
        }

        public AcademicCourse Create(AcademicCourse itemToCreate)
        {
            var academicYearDetails = _context.AcademicYearCourses.Add(itemToCreate);
            _context.SaveChanges();
            return academicYearDetails;
        }

        public IQueryable<AcademicCourse> Query(Expression<Func<AcademicCourse, AcademicCourse>> expression)
        {
            return _context.AcademicYearCourses.Select(expression);
        }

        public IQueryable<AcademicCourse> Filter(Expression<Func<AcademicCourse, bool>> expression)
        {
            return _context.AcademicYearCourses.Where(expression);
        }

        public AcademicCourse Update(AcademicCourse itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public IEnumerable<AcademicCourse> GetAllAcademicYearsDetails(long academicYearId)
        {   
            return Query(x => x).ToList();
        }

        public AcademicCourse Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.AcademicYearCourses.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public AcademicCourse Delete(AcademicCourse itemToDelete)
        {
            _context.AcademicYearCourses.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }
    }
}
