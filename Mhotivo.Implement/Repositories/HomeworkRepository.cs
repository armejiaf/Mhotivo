using System.Data.Entity;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mhotivo.Implement.Repositories
{
    public class HomeworkRepository : IHomeworkRepository
    {
        private readonly MhotivoContext _context;

        public HomeworkRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Homework GetById(long id)
        {
            return _context.Homeworks.FirstOrDefault(x => x.Id == id);
        }

        public Homework Create(Homework itemToCreate)
        {
            var homework = _context.Homeworks.Add(itemToCreate);
            _context.SaveChanges();
            return homework;
        }

        public IQueryable<Homework> Query(Expression<Func<Homework, Homework>> expression)
        {
            return _context.Homeworks.Select(expression);
        }

        public IQueryable<Homework> Filter(Expression<Func<Homework, bool>> expression)
        {
            return _context.Homeworks.Where(expression);
        }

        public Homework Update(Homework itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Homework Delete(Homework itemToDelete)
        {
            _context.Homeworks.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Homework Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Homeworks.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Homework> GetAllHomeworks()
        {
            return Query(x => x).ToList();
        }
    }
}