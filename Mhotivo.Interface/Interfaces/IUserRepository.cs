using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IUserRepository
    {
        User First(Expression<Func<User, bool>> query);
        User FirstOrDefault(Expression<Func<User,bool>> query);
        User GetById(long id);
        User Create(User itemToCreate, Roles rol);
        IQueryable<User> Query(Expression<Func<User, User>> expression);
        IQueryable<User> Filter(Expression<Func<User, bool>> expression);
        User Update(User itemToUpdate, bool updateRole, Roles rol);
        User Delete(long id);
        void SaveChanges();
        IEnumerable<User> GetAllUsers();
        Roles GetUserRole(long idUser);
        User UpdateUserFromUserEditModel(User userModel, User user, bool updateRole, Roles rol);
        bool ExistEmail(string userName);
    }
}