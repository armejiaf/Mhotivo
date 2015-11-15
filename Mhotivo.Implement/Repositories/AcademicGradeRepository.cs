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
    public class AcademicGradeRepository : IAcademicGradeRepository
    {
        private readonly MhotivoContext _context;
        public AcademicGradeRepository(MhotivoContext context)
        {
            _context = context;
        }

        public AcademicGrade GetById(long id)
        {
            return _context.AcademicYearGrades.FirstOrDefault(x => x.Id == id);
        }

        public AcademicGrade Create(AcademicGrade itemToCreate)
        {
            itemToCreate = _context.AcademicYearGrades.Add(itemToCreate);
            _context.SaveChanges();
            return itemToCreate;
        }

        public IQueryable<AcademicGrade> Query(Expression<Func<AcademicGrade, AcademicGrade>> expression)
        {
            return _context.AcademicYearGrades.Select(expression);
        }

        public IQueryable<AcademicGrade> Filter(Expression<Func<AcademicGrade, bool>> expression)
        {
            return _context.AcademicYearGrades.Where(expression);
        }

        public AcademicGrade Update(AcademicGrade itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public AcademicGrade Delete(long id)
        {
            var item = GetById(id);
            _context.AcademicYearGrades.Remove(item);
            _context.SaveChanges();
            return item;
        }

        public AcademicGrade Delete(AcademicGrade grade)
        {
            _context.AcademicYearGrades.Remove(grade);
            _context.SaveChanges();
            return grade;
        }

        public IEnumerable<AcademicGrade> GetAllGrades()
        {
            return Query(x => x).ToList();
        }
    }
}
