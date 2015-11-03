using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IGradeRepository
    {
        Grade Create(Grade itemToCreate);
        Grade Delete(long id);
        Grade Delete(Grade itemToDelete);
        IEnumerable<Grade> GetAllGrade();
        Grade GetById(long id);
        IQueryable<Grade> Query(Expression<Func<Grade, Grade>> expression);
        IQueryable<Grade> Filter(Expression<Func<Grade, bool>> expression);
        Grade Update(Grade itemToUpdate);
    }
}