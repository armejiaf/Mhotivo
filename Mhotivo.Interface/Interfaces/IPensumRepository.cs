using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IPensumRepository
    {
        Pensum GetById(long id);
        Pensum Create(Pensum itemToCreate);
        IQueryable<Pensum> Query(Expression<Func<Pensum, Pensum>> expression);
        IQueryable<Pensum> Filter(Expression<Func<Pensum, bool>> expression);
        Pensum Update(Pensum itemToUpdate);
        Pensum Delete(long id);
        Pensum Delete(Pensum itemToDelete);
        IEnumerable<Pensum> GetAllPesums();
    }
}