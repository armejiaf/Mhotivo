using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class EducationLevelRepository : IEducationLevelRepository
    {
        private readonly MhotivoContext _context;

        public EducationLevelRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public EducationLevel GetById(long id)
        {
            return _context.EducationLevels.FirstOrDefault(x => x.Id == id);
        }

        public EducationLevel Create(EducationLevel itemToCreate)
        {
            var role = _context.EducationLevels.Add(itemToCreate);
            _context.SaveChanges();
            return role;
        }

        public IQueryable<EducationLevel> Query(Expression<Func<EducationLevel, EducationLevel>> expression)
        {
            return _context.EducationLevels.Select(expression);
        }

        public IQueryable<EducationLevel> Filter(Expression<Func<EducationLevel, bool>> expression)
        {
            return _context.EducationLevels.Where(expression);
        }

        public EducationLevel Update(EducationLevel itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public EducationLevel Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.EducationLevels.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public EducationLevel Delete(EducationLevel itemToDelete)
        {
            _context.EducationLevels.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<EducationLevel> GetAllAreas()
        {
            return Query(x => x).ToList();
        }
    }
}

