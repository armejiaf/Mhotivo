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
    public class AcademicYearGradeRepository : IAcademicYearGradeRepository
    {
        private readonly MhotivoContext _context;
        public AcademicYearGradeRepository(MhotivoContext context)
        {
            _context = context;
        }

        public AcademicYearGrade GetById(long id)
        {
            return _context.AcademicYearGrades.FirstOrDefault(x => x.Id == id);
        }

        public AcademicYearGrade Create(AcademicYearGrade itemToCreate)
        {
            itemToCreate = _context.AcademicYearGrades.Add(itemToCreate);
            _context.SaveChanges();
            return itemToCreate;
        }

        public IQueryable<AcademicYearGrade> Query(Expression<Func<AcademicYearGrade, AcademicYearGrade>> expression)
        {
            return _context.AcademicYearGrades.Select(expression);
        }

        public IQueryable<AcademicYearGrade> Filter(Expression<Func<AcademicYearGrade, bool>> expression)
        {
            return _context.AcademicYearGrades.Where(expression);
        }

        public AcademicYearGrade Update(AcademicYearGrade itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public AcademicYearGrade Delete(long id)
        {
            var item = GetById(id);
            _context.AcademicYearGrades.Remove(item);
            _context.SaveChanges();
            return item;
        }

        public AcademicYearGrade Delete(AcademicYearGrade grade)
        {
            _context.AcademicYearGrades.Remove(grade);
            _context.SaveChanges();
            return grade;
        }

        public IEnumerable<AcademicYearGrade> GetAllGrades()
        {
            return Query(x => x).ToList();
        }
    }
}
