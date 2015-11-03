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
    public class RoleRepository : IRoleRepository
    {
        private readonly MhotivoContext _context;

        public RoleRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Role GetById(long id)
        {
            return _context.Roles.FirstOrDefault(x => x.Id == id);
        }

        public Role Create(Role itemToCreate)
        {
            var role = _context.Roles.Add(itemToCreate);
            _context.SaveChanges();
            return role;
        }

        public IQueryable<Role> Query(Expression<Func<Role, Role>> expression)
        {
            return _context.Roles.Select(expression);
        }

        public IQueryable<Role> Filter(Expression<Func<Role, bool>> expression)
        {
            return _context.Roles.Where(expression);
        }

        public Role Update(Role itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;   
        }

        public Role Delete(Role itemToDelete)
        {
            _context.Roles.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Role Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Roles.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Role> GetAll()
        {
            return Query(x => x).ToList();
        }
    }
}