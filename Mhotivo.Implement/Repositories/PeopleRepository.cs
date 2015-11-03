using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using System.Data.Entity;

namespace Mhotivo.Implement.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly MhotivoContext _context;

        public PeopleRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public People GetById(long id)
        {
            return _context.Peoples.FirstOrDefault(x => x.Id == id);
        }

        public People Create(People itemToCreate)
        {
            var people = _context.Peoples.Add(itemToCreate);
            _context.SaveChanges();
            return people;
        }

        public IQueryable<People> Query(Expression<Func<People, People>> expression)
        {
            return _context.Peoples.Select(expression);
        }

        public IQueryable<People> Filter(Expression<Func<People, bool>> expression)
        {
            return _context.Peoples.Where(expression);
        }
            
        public People Update(People itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;   
        }

        public People Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Peoples.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public People Delete(People itemToDelete)
        {
            _context.Peoples.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<People> GetAllPeople()
        {
            return Query(x => x).ToList();
        }
    }
}