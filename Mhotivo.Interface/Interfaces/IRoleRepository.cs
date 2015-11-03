using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IRoleRepository
    {
        Role GetById(long id);
        Role Create(Role itemToCreate);
        IQueryable<Role> Query(Expression<Func<Role, Role>> expression);
        IQueryable<Role> Filter(Expression<Func<Role, bool>> expression);
        Role Update(Role itemToUpdate);
        Role Delete(long id);
        Role Delete(Role itemToDelete);
        IEnumerable<Role> GetAll();
    }
}