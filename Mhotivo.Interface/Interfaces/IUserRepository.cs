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
        User GetById(long id);
        User Create(User itemToCreate, Role rol);
        IQueryable<User> Query(Expression<Func<User, User>> expression);
        IQueryable<User> Filter(Expression<Func<User, bool>> expression);
        User Update(User itemToUpdate, bool updateRole, Role rol);
        User Delete(long id);
        void SaveChanges();
        IEnumerable<User> GetAllUsers();

        ICollection<Role> GetUserRoles(int idUser);

    }
}