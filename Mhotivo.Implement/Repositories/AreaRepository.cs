using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class AreaRepository : IAreaReporsitory
    {
        private readonly MhotivoContext _context;

        public AreaRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Data.Entities.Area First(System.Linq.Expressions.Expression<Func<Data.Entities.Area, bool>> query)
        {
            return _context.Areas.First(query);
        }

        public Data.Entities.Area GetById(long id)
        {
            return _context.Areas.FirstOrDefault(x => x.Id == id);
        }

        public Data.Entities.Area Create(Data.Entities.Area itemToCreate)
        {
            return _context.Areas.Add(itemToCreate);
        }

        public IQueryable<Data.Entities.Area> Query(
            System.Linq.Expressions.Expression<Func<Data.Entities.Area, Data.Entities.Area>> expression)
        {
            return _context.Areas.Select(expression);
        }

        public IQueryable<Data.Entities.Area> Where(
            System.Linq.Expressions.Expression<Func<Data.Entities.Area, bool>> expression)
        {
            return _context.Areas.Where(expression);
        }

        public IQueryable<Data.Entities.Area> Filter(
            System.Linq.Expressions.Expression<Func<Data.Entities.Area, bool>> expression)
        {
            return _context.Areas.Where(expression);
        }

        public Data.Entities.Area Update(Data.Entities.Area itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            return itemToUpdate;
        }

        public void Delete(Data.Entities.Area itemToDelete)
        {
            _context.Areas.Remove(itemToDelete);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
