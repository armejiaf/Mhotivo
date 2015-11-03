using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IParentRepository
    {
        Parent GetById(long id);
        Parent Create(Parent itemToCreate);
        IQueryable<Parent> Query(Expression<Func<Parent, Parent>> expression);
        IQueryable<Parent> Filter(Expression<Func<Parent, bool>> expression);
        Parent Update(Parent itemToUpdate);
        Parent Delete(long id);
        Parent Delete(Parent itemToDelete);
        IEnumerable<Parent> GetAllParents();
    }
}