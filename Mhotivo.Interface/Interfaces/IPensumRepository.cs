using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IPensumRepository : IDisposable
    {
        Pensum First(Expression<Func<Pensum, Pensum>> query);
        Pensum GetById(long id);
        Pensum Create(Pensum itemToCreate);
        IQueryable<Pensum> Query(Expression<Func<Pensum, Pensum>> expression);
        IQueryable<Pensum> Filter(Expression<Func<Pensum, bool>> expression);
        Pensum Update(Pensum itemToUpdate, bool courseUpdate, bool gradeUpdate);
        Pensum Delete(long id);
        void SaveChanges();
        IEnumerable<Pensum> GetAllPesums();
    }
}