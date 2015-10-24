using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IRoleRepository
    {
        Role First(Expression<Func<Role, bool>> query);
        Role FirstOrDefault(Expression<Func<Role, bool>> query);
        Role GetById(int id);
        Role Create(Role itemToCreate);
        IQueryable<Role> Query(Expression<Func<Role, Role>> expression);
        IQueryable<Role> Filter(Expression<Func<Role, bool>> expression);
        Role Update(Role itemToUpdate);
        Role Delete(Role itemToDelete);
        void SaveChanges();
        IEnumerable<Role> GetAll();
    }
}