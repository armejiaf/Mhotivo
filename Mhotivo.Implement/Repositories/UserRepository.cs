using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MhotivoContext _context;

        public UserRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public User First(Expression<Func<User, bool>> query)
        {
            var users = _context.Users.First(query);
            return users;
        }

        public User GetById(long id)
        {
            var users = _context.Users.Where(x => x.Id == id);
            return users.Count() != 0 ? users.First() : null;
        }

        public User Create(User itemToCreate, Role rol)
        {
            var userRolNew = new UserRol { User = itemToCreate, Role = rol };
            var user = _context.Users.Add(itemToCreate);

            var userRol = _context.UserRoles.Add(userRolNew);

            //_context.Entry(user.Groups).State = EntityState.Modified;
            _context.SaveChanges();
            return user;
        }

        public IQueryable<User> Query(Expression<Func<User, User>> expression)
        {
            var myUsers = _context.Users.Select(expression);
            return myUsers;
            //return myUsers.Count() != 0 ? myUsers : myUsers;
            
        }

        public IQueryable<User> Filter(Expression<Func<User, bool>> expression)
        {
            var myUsers = _context.Users.Where(expression);
            return myUsers.Count() != 0 ? myUsers.Include(x => x.Groups) : myUsers;
        }

        public User Update(User itemToUpdate, bool updateRole, Role rol)
        {
            if (updateRole)
            {
                var rolesExist = _context.UserRoles.Where(x => x.User != null && x.Role != null & x.User.Id == itemToUpdate.Id);
                if (!rolesExist.Any())
                {
                    var userRol = new UserRol { User = itemToUpdate, Role = rol };
                    _context.UserRoles.Add(userRol);
                }
            }
            _context.SaveChanges();
            return itemToUpdate;   
        }

        public User Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Users.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return Query(x => x).ToList().Select(x => new User
            {
                DisplayName = x.DisplayName,
                Email = x.Email,
                //Role = x.Role.Name,
                //Status = x.Status ? "Activo" : "Inactivo",
                //Role = x.Role,
                Status = x.Status,
                Id = x.Id
            });
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}