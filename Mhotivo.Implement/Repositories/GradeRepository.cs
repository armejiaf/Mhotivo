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
    public class GradeRepository : IGradeRepository
    {
        private readonly MhotivoContext _context;

        public GradeRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }
        
        public Grade GetById(long id)
        {
            return _context.Grades.FirstOrDefault(x => x.Id == id);
        }

        public Grade Create(Grade itemToCreate)
        {
            var grade = _context.Grades.Add(itemToCreate);
            _context.SaveChanges();
            return grade;
        }

        public IQueryable<Grade> Query(Expression<Func<Grade, Grade>> expression)
        {
            return _context.Grades.Select(expression);
        }

        public IQueryable<Grade> Filter(Expression<Func<Grade, bool>> expression)
        {
            return _context.Grades.Where(expression);
        }

        public Grade Update(Grade itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Grade Delete(Grade itemToDelete)
        {
            _context.Grades.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Grade> GetAllGrade()
        {
            return Query(g => g).ToList();
        }

        public Grade Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Grades.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }
    }
}