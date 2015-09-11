using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IEducationLevelRepository: IDisposable
    {
        EducationLevel First(Expression<Func<EducationLevel, EducationLevel>> query);
        EducationLevel GetById(long id);
        EducationLevel Create(EducationLevel itemToCreate);
        IQueryable<EducationLevel> Query(Expression<Func<EducationLevel, EducationLevel>> expression);
        IQueryable<EducationLevel> Filter(Expression<Func<EducationLevel, bool>> expression);
        EducationLevel Update(EducationLevel itemToUpdate);
        EducationLevel UpdateAreaFromAreaEditModel(EducationLevel areaEditModel, EducationLevel area);
        EducationLevel Delete(long id);
        void SaveChanges();
        IEnumerable<EducationLevel> GetAllAreas();
    }
}
