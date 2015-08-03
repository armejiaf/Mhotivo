using System;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAreaReporsitory
    {
        Area First(Expression<Func<Area, bool>> query);
        Area GetById(long id);
        Area Create(Area itemToCreate);
        IQueryable<Area> Query(Expression<Func<Area, Area>> expression);
        IQueryable<Area> Where(Expression<Func<Area, bool>> expression);
        IQueryable<Area> Filter(Expression<Func<Area, bool>> expression);
        Area Update(Area itemToUpdate);
        void Delete(Area itemToDelete);
        void SaveChanges();
    }
}
