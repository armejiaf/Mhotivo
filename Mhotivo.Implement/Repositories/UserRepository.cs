using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface.Interfaces;
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

        public User GetById(long id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User Create(User itemToCreate)
        {
            itemToCreate.HashPassword();
            _context.PeopleWithUsers.Attach(itemToCreate.UserOwner);
            var user = _context.Users.Add(itemToCreate);
            _context.SaveChanges();
            return user;
        }

        public IQueryable<User> Query(Expression<Func<User, User>> expression)
        {
            var myUsers = _context.Users.Select(expression);
            return myUsers;
        }

        public IQueryable<User> Filter(Expression<Func<User, bool>> expression)
        {
            var myUsers = _context.Users.Where(expression);
            return myUsers;
        }

        public User Update(User itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
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

        public User Delete(User itemToDelete)
        {
            _context.Users.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return Query(x => x).ToList();
        }

        public Role GetUserRole(long idUser)
        {
            var userTemp = GetById(idUser);
            return userTemp == null ? null : userTemp.Role;
        }
    }
}