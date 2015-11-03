using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IUserRepository
    {
        User GetById(long id);
        User Create(User itemToCreate);
        IQueryable<User> Query(Expression<Func<User, User>> expression);
        IQueryable<User> Filter(Expression<Func<User, bool>> expression);
        User Update(User itemToUpdate);
        User Delete(long id);
        User Delete(User itemToDelete);
        IEnumerable<User> GetAllUsers();
        Role GetUserRole(long idUser);
    }
}