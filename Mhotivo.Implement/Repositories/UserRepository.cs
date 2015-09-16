using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

        public User First(Expression<Func<User, bool>> query)
        {
            var users = _context.Users.First(query);
            return users;
        }

        public User FirstOrDefault(Expression<Func<User, bool>> query)
        {
            var users = _context.Users.FirstOrDefault(query);
            return users;
        }

        public User GetById(long id)
        {
            var users = _context.Users.Where(x => x.Id == id);
            return users.Count() != 0 ? users.First() : null;
        }

        public User Create(User itemToCreate, Roles rol)
        {
            itemToCreate.Role = rol;
            itemToCreate.EncryptPassword();
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
            return myUsers.Count() != 0 ? myUsers.Include(x => x.Groups) : myUsers;
        }

        public User Update(User itemToUpdate, bool updateRole, Roles rol)
        {
            if (updateRole)
            {
                if(!itemToUpdate.Role.Equals(rol))
                    itemToUpdate.Role = rol;
            }
            SaveChanges();
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
                IsActive = x.IsActive,
                Id = x.Id
            });
        }

        public Roles GetUserRole(long idUser)
        {
            var userTemp = GetById(idUser);
            return userTemp == null ? Roles.Invalid : userTemp.Role;
        }

        public User UpdateUserFromUserEditModel(User userModel, User user, bool updateRole, Roles rol)
        {
            user.DisplayName = userModel.DisplayName;
            user.Email = userModel.Email;
            user.Notifications = userModel.Notifications;
            user.Parents = userModel.Parents;
            user.IsActive = userModel.IsActive;
            return Update(user,updateRole,rol);
        }

        public bool ExistEmail(string userName)
        {
            var user = _context.Users.Where(x => x.Email.Equals(userName));
            return user.Any();
        }
    }
}