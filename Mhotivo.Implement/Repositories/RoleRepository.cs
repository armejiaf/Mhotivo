using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

        public Role First(Expression<Func<Role, bool>> query)
        {
            var roles = _context.Roles.First(query);
            return roles;
        }

        public Role FirstOrDefault(Expression<Func<Role, bool>> query)
        {
            var roles = _context.Roles.FirstOrDefault(query);
            return roles;
        }

        public Role GetById(int id)
        {
            var roles = _context.Roles.Where(x => x.RoleId == id);
            return roles.Count() != 0 ? roles.First() : null;
        }

        public Role Create(Role itemToCreate)
        {
            var role = _context.Roles.Add(itemToCreate);
            _context.SaveChanges();
            return role;
        }

        public IQueryable<Role> Query(Expression<Func<Role, Role>> expression)
        {
            var myUsers = _context.Roles.Select(expression);
            return myUsers;
        }

        public IQueryable<Role> Filter(Expression<Func<Role, bool>> expression)
        {
            var myUsers = _context.Roles.Where(expression);
            return myUsers;
        }

        public Role Update(Role itemToUpdate)
        {
            _context.Roles.AddOrUpdate(itemToUpdate);
            SaveChanges();
            return itemToUpdate;   
        }

        public Role Delete(Role itemToDelete)
        {
            var itemToDeleteInternal = FirstOrDefault(x => x.Equals(itemToDelete));
            if (itemToDeleteInternal == null)
            {
                throw new Exception("itemToDeleteNotFound");
            }
            _context.Roles.Remove(itemToDeleteInternal);
            return itemToDeleteInternal;
        }

        public Role Delete(int id)
        {
            var itemToDelete = GetById(id);
            _context.Roles.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Role> GetAll()
        {
            return Query(x => x).ToList().Select(x => new Role
            {
                Name = x.Name,
                Privileges = x.Privileges,
                Users = x.Users,
                Value = x.Value,
                RoleId = x.RoleId
            });
        }
    }
}