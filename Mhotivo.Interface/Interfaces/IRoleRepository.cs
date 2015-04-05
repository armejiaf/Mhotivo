using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IRoleRepository
    {
        Role First(Expression<Func<Role, Role>> query);
        Role GetById(long id);
        Role Create(Role itemToCreate);
        IQueryable<TResult> Query<TResult>(Expression<Func<Role, TResult>> expression);
        IQueryable<Role> Filter(Expression<Func<Role, bool>> expression);
        Role Update(Role itemToUpdate);
        void Delete(Role itemToDelete);
        void SaveChanges();
        IEnumerable<Role> GetAllRoles();
    }
}