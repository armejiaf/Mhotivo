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
    public class EnrollRepository : IEnrollRepository
    {
        private readonly MhotivoContext _context;

        public EnrollRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Enroll GetById(long id)
        {
            return _context.Enrolls.FirstOrDefault(x => x.Id == id);
        }

        public Enroll Create(Enroll itemToCreate)
        {
            var enroll = _context.Enrolls.Add(itemToCreate);
            _context.SaveChanges();
            return enroll;
        }

        public IQueryable<Enroll> Query(Expression<Func<Enroll, Enroll>> expression)
        {
            return _context.Enrolls.Select(expression);
        }

        public IQueryable<Enroll> Filter(Expression<Func<Enroll, bool>> expression)
        {
            return _context.Enrolls.Where(expression);
        }

        public Enroll Update(Enroll itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Enroll Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Enrolls.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Enroll Delete(Enroll itemToDelete)
        {
            _context.Enrolls.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Enroll> GetAllsEnrolls()
        {
            return Query(x => x).ToList();
        }
    }
}
