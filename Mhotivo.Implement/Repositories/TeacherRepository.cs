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
    public class TeacherRepository : ITeacherRepository
    {
        private readonly MhotivoContext _context;

        public TeacherRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Teacher GetById(long id)
        {
            return _context.Teachers.FirstOrDefault(x => x.Id == id);
        }

        public Teacher Create(Teacher itemToCreate)
        {
            var teacher = _context.Teachers.Add(itemToCreate);
            _context.SaveChanges();
            return teacher;
        }

        public IQueryable<Teacher> Query(Expression<Func<Teacher, Teacher>> expression)
        {
            return _context.Teachers.Select(expression);
        }

        public IQueryable<Teacher> Filter(Expression<Func<Teacher, bool>> expression)
        {
            return _context.Teachers.Where(expression);
        }

        public Teacher Update(Teacher itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Teacher Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Teachers.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Teacher Delete(Teacher itemToDelete)
        {
            _context.Teachers.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Teacher> GetAllTeachers()
        {
            return Query(x => x).ToList();
        }
    }
}