using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAreaRepository
    {
        Area First(Expression<Func<Area, Area>> query);
        Area GetById(long id);
        Area Create(Area itemToCreate);
        IQueryable<Area> Query(Expression<Func<Area, Area>> expression);
        IQueryable<Area> Filter(Expression<Func<Area, bool>> expression);
        Area Update(Area itemToUpdate);
        Area UpdateAreaFromAreaEditModel(Area areaEditModel, Area area);
        Area Delete(long id);
        void SaveChanges();
        IEnumerable<Area> GetAllAreas();
    }
}
