using System;
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

        public EducationLevel First(Expression<Func<EducationLevel, EducationLevel>> query)
        {
            var areas = _context.EducationLevels.Select(query);
            return areas.Count() != 0 ? areas.First() : null;
        }

        public EducationLevel GetById(long id)
        {
            var areas = _context.EducationLevels.Where(x => x.Id == id);
            return areas.Count() != 0 ? areas.First() : null;
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
            SaveChanges();
            return itemToUpdate;
        }
        public EducationLevel UpdateAreaFromAreaEditModel(EducationLevel areaEditModel, EducationLevel area)
        {
            area.Name = areaEditModel.Name;
            return Update(area);
        }
        public EducationLevel Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.EducationLevels.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public System.Collections.Generic.IEnumerable<EducationLevel> GetAllAreas()
        {
            return Query(x => x).ToList().Select(x => new EducationLevel
            {
                Name = x.Name,
                Id = x.Id
            });
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

