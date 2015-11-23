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
    public class PrivilegeRepository : IPrivilegeRepository
    {
        private readonly MhotivoContext _context;

        public PrivilegeRepository(MhotivoContext context)
        {
            _context = context;
        }

        public Privilege First(Expression<Func<Privilege, bool>> query)
        {
            return _context.Privileges.First(query);
        }

        public Privilege FirstOrDefault(Expression<Func<Privilege, bool>> query)
        {
            return _context.Privileges.FirstOrDefault(query);
        }

        public Privilege GetById(long id)
        {
            return _context.Privileges.FirstOrDefault(x => x.Id == id);
        }

        public Privilege Create(Privilege itemToCreate)
        {
            var privilege = _context.Privileges.Add(itemToCreate);
            _context.SaveChanges();
            return privilege;
        }

        public IQueryable<Privilege> Query(Expression<Func<Privilege, Privilege>> expression)
        {
            return _context.Privileges.Select(expression);
        }

        public IQueryable<Privilege> Filter(Expression<Func<Privilege, bool>> expression)
        {
            return _context.Privileges.Where(expression);
        }

        public Privilege Update(Privilege itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Privilege Delete(Privilege itemToDelete)
        {
            _context.Privileges.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Privilege Delete(long id)
        {
            var itemToDelete = GetById(id);
            return Delete(itemToDelete);
        }

        public IEnumerable<Privilege> GetAll()
        {
            return Query(x => x).ToList();
        }
    }
}
