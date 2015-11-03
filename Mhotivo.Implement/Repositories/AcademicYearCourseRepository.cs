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
    public class AcademicYearCourseRepository : IAcademicYearCourseRepository
    {
        private readonly MhotivoContext _context;

        public AcademicYearCourseRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<AcademicYearCourse> GetAllAcademicYearDetails()
        {
            return Query(x => x).ToList();
        }

        public AcademicYearCourse GetById(long id)
        {
            return _context.AcademicYearCourses.FirstOrDefault(x => x.Id == id);
        }

        public AcademicYearCourse Create(AcademicYearCourse academicYearToCreate)
        {
            var academicYearDetails = _context.AcademicYearCourses.Add(academicYearToCreate);
            _context.Entry(academicYearToCreate.Course).State = EntityState.Modified;
            _context.Entry(academicYearToCreate.Teacher).State = EntityState.Modified;
            _context.SaveChanges();
            return academicYearDetails;
        }

        public IQueryable<AcademicYearCourse> Query(Expression<Func<AcademicYearCourse, AcademicYearCourse>> expression)
        {
            return _context.AcademicYearCourses.Select(expression);
        }

        public IQueryable<AcademicYearCourse> Filter(Expression<Func<AcademicYearCourse, bool>> expression)
        {
            return _context.AcademicYearCourses.Where(expression);
        }

        public AcademicYearCourse Update(AcademicYearCourse itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public IEnumerable<AcademicYearCourse> GetAllAcademicYearsDetails(long academicYearId)
        {
            return Query(x => x).ToList();
        }

        public AcademicYearCourse Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.AcademicYearCourses.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public AcademicYearCourse Delete(AcademicYearCourse itemToDelete)
        {
            _context.AcademicYearCourses.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }
    }
}
