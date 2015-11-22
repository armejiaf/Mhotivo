using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface ITutorRepository
    {
        Tutor GetById(long id);
        Tutor Create(Tutor itemToCreate);
        IQueryable<Tutor> Query(Expression<Func<Tutor, Tutor>> expression);
        IQueryable<Tutor> Filter(Expression<Func<Tutor, bool>> expression);
        Tutor Update(Tutor itemToUpdate);
        Tutor Delete(long id);
        Tutor Delete(Tutor itemToDelete);
        IEnumerable<Tutor> GetAllTutors();
    }
}