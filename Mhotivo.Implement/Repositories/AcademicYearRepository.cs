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
    public class AcademicYearRepository : IAcademicYearRepository
    {
        private readonly MhotivoContext _context;

        public AcademicYearRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public AcademicYear GetById(long id)
        {
            return _context.AcademicYears.FirstOrDefault(x => x.Id == id);
        }

        public AcademicYear Create(AcademicYear academicYearGradeToCreate)
        {
            var academicYear = _context.AcademicYears.Add(academicYearGradeToCreate);
            _context.SaveChanges();
            return academicYear;
        }

        public IQueryable<AcademicYear> Query(Expression<Func<AcademicYear, AcademicYear>> expression)
        {
            return _context.AcademicYears.Select(expression);
        }

        public IQueryable<AcademicYear> Filter(Expression<Func<AcademicYear, bool>> expression)
        {
            return _context.AcademicYears.Where(expression);
        }

        public AcademicYear Update(AcademicYear itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public AcademicYear Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.AcademicYears.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public AcademicYear Delete(AcademicYear itemToDelete)
        {
            _context.AcademicYears.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<AcademicYear> GetAllAcademicYears()
        {
            return Query(x => x).ToList();
        }

        public AcademicYear GetCurrentAcademicYear()
        {
            return _context.AcademicYears.FirstOrDefault(ay => ay.IsActive);
        }
    }
}