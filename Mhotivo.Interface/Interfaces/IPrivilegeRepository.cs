using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IPrivilegeRepository
    {
        Privilege First(Expression<Func<Privilege, bool>> query);
        Privilege FirstOrDefault(Expression<Func<Privilege, bool>> query);
        Privilege GetById(int id);
        Privilege Create(Privilege itemToCreate);
        IQueryable<Privilege> Query(Expression<Func<Privilege, Privilege>> expression);
        IQueryable<Privilege> Filter(Expression<Func<Privilege, bool>> expression);
        Privilege Update(Privilege itemToUpdate);
        Privilege Delete(Privilege itemToDelete);
        void SaveChanges();
        IEnumerable<Privilege> GetAll();
    }
}