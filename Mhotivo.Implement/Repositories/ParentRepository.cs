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
    public class ParentRepository : IParentRepository
    {
        private readonly MhotivoContext _context;

        public ParentRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public MhotivoContext GetContext()
        {
            return _context;
        }
       

        public Parent GetById(long id)
        {
            return _context.Parents.FirstOrDefault(x => x.Id == id);
        }

        public Parent Create(Parent itemToCreate)
        {
            itemToCreate.Disable = false;
            var parent = _context.Parents.Add(itemToCreate);
            _context.SaveChanges();
            return parent;
        }

        public IQueryable<Parent> Query(Expression<Func<Parent, Parent>> expression)
        {
            return _context.Parents.Select(expression);
        }

        public IQueryable<Parent> Filter(Expression<Func<Parent, bool>> expression)
        {
            return _context.Parents.Where(expression);
        }

        public Parent Update(Parent itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Parent Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Parents.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Parent Delete(Parent itemToDelete)
        {
            _context.Parents.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Parent> GetAllParents()
        {
            return Query(x => x).Where(x => !x.Disable).ToList();
        }
    }
}
