using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using System.Data.Entity;

namespace Mhotivo.Implement.Repositories
{
    public class PensumRepository : IPensumRepository
    {
        private readonly MhotivoContext _context;

        public PensumRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Pensum GetById(long id)
        {
            return _context.Pensums.FirstOrDefault(x => x.Id == id);
        }

        public Pensum Delete(Pensum itemToDelete)
        {
            _context.Pensums.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Pensum> GetAllPesums()
        {
            return Query(x => x).ToList();
        }

        public Pensum Create(Pensum itemToCreate)
        {
            var pensum = _context.Pensums.Add(itemToCreate);
            _context.SaveChanges();
            return pensum;
        }

        public IQueryable<Pensum> Query(Expression<Func<Pensum, Pensum>> expression)
        {
            return _context.Pensums.Select(expression);
        }

        public IQueryable<Pensum> Filter(Expression<Func<Pensum, bool>> expression)
        {
            return _context.Pensums.Where(expression);
        }

        public Pensum Update(Pensum itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;   
        }

        public Pensum Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Pensums.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }
    }
}