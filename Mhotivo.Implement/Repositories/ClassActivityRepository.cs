using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class ClassActivityRepository : IClassActivityRepository
    {
        private readonly MhotivoContext _context;

        public ClassActivityRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public ClassActivity First(Expression<Func<ClassActivity, ClassActivity>> query)
        {
            var classactivities = _context.ClassActivities.Select(query);
            return classactivities.First();
        }

        public ClassActivity GetById(long id)
        {
            var classactivities = _context.ClassActivities.Where(x => x.Id == id);
            return classactivities.First();
        }

        public ClassActivity Create(ClassActivity itemToCreate)
        {
            var classactivity = _context.ClassActivities.Add(itemToCreate);
            _context.Entry(classactivity.AcademicYear).State = EntityState.Modified;
            return classactivity;
        }

        public IQueryable<ClassActivity> Query(Expression<Func<ClassActivity, ClassActivity>> expression)
        {
            var myClassActivities = _context.ClassActivities.Select(expression);
            return myClassActivities.Count() != 0 ? myClassActivities.Include(x => x.AcademicYear) : myClassActivities;
        }

        public IQueryable<ClassActivity> Filter(Expression<Func<ClassActivity, bool>> expression)
        {
            var myClassActivities = _context.ClassActivities.Where(expression);
            return myClassActivities.Count() != 0 ? myClassActivities.Include(x => x.AcademicYear) : myClassActivities;
        }

        public ClassActivity Update(ClassActivity itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            return itemToUpdate;   
        }

        public ClassActivity Delete(ClassActivity itemToDelete)
        {
            _context.ClassActivities.Remove(itemToDelete);
            return itemToDelete;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public String ActiveClassActivityDisplaytext(bool active)
        {
            return active ? "Activo" : "Inactivo";
        }
    }
}