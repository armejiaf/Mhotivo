using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IEducationLevelRepository
    {
        EducationLevel GetById(long id);
        EducationLevel Create(EducationLevel itemToCreate);
        IQueryable<EducationLevel> Query(Expression<Func<EducationLevel, EducationLevel>> expression);
        IQueryable<EducationLevel> Filter(Expression<Func<EducationLevel, bool>> expression);
        EducationLevel Update(EducationLevel itemToUpdate);
        EducationLevel Delete(long id);
        EducationLevel Delete(EducationLevel itemToDelete);
        IEnumerable<EducationLevel> GetAllAreas();
    }
}
