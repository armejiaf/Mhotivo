using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IEnrollRepository
    {
        Enroll GetById(long id);
        Enroll Create(Enroll itemToCreate);
        IQueryable<Enroll> Query(Expression<Func<Enroll, Enroll>> expression);
        IQueryable<Enroll> Filter(Expression<Func<Enroll, bool>> expression);
        Enroll Update(Enroll itemToUpdate);
        Enroll Delete(long id);
        Enroll Delete(Enroll itemToDelete);
        IEnumerable<Enroll> GetAllsEnrolls(); 
    }
}