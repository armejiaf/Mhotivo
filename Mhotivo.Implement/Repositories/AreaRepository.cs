using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly MhotivoContext _context;

        public AreaRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }



        public Area First(Expression<Func<Area, Area>> query)
        {
            var areas = _context.Areas.Select(query);
            return areas.Count() != 0 ? areas.First() : null;
        }

        public Area GetById(long id)
        {
            var areas = _context.Areas.Where(x => x.Id == id);
            return areas.Count() != 0 ? areas.First() : null;
        }

        public Area Create(Area itemToCreate)
        {
            var role = _context.Areas.Add(itemToCreate);
            _context.SaveChanges();
            return role;
        }

        public IQueryable<Area> Query(Expression<Func<Area, Area>> expression)
        {
            return _context.Areas.Select(expression);
        }

        public IQueryable<Area> Filter(Expression<Func<Area, bool>> expression)
        {
            return _context.Areas.Where(expression);
        }

        public Area Update(Area itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            SaveChanges();
            return itemToUpdate;
        }
        public Area UpdateAreaFromAreaEditModel(Area areaEditModel, Area area)
        {
            area.Name = areaEditModel.Name;
            return Update(area);
        }
        public Area Delete(long id)
        {
            //_context.Areas.Remove(itemToDelete);
            var itemToDelete = GetById(id);
            _context.Areas.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public System.Collections.Generic.IEnumerable<Area> GetAllAreas()
        {
            return Query(x => x).ToList().Select(x => new Area
            {
                Name = x.Name,
                //Email = x.Email,
                //Role = x.Role.Name,
                //Status = x.Status ? "Activo" : "Inactivo",
                //Role = x.Role,
                //Status = x.Status,
                Id = x.Id
            });
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

