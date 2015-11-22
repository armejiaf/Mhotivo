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
    public class TutorRepository : ITutorRepository
    {
        private readonly MhotivoContext _context;

        public TutorRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public MhotivoContext GetContext()
        {
            return _context;
        }
       

        public Tutor GetById(long id)
        {
            return _context.Tutors.FirstOrDefault(x => x.Id == id);
        }

        public Tutor Create(Tutor itemToCreate)
        {
            var tutor = _context.Tutors.Add(itemToCreate);
            _context.SaveChanges();
            return tutor;
        }

        public IQueryable<Tutor> Query(Expression<Func<Tutor, Tutor>> expression)
        {
            return _context.Tutors.Select(expression);
        }

        public IQueryable<Tutor> Filter(Expression<Func<Tutor, bool>> expression)
        {
            return _context.Tutors.Where(expression);
        }

        public Tutor Update(Tutor itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Tutor Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Tutors.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Tutor Delete(Tutor itemToDelete)
        {
            _context.Tutors.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Tutor> GetAllTutors()
        {
            return Query(x => x).ToList();
        }
    }
}
