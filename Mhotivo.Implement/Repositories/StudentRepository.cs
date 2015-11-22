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
    public class StudentRepository : IStudentRepository
    {
        private readonly MhotivoContext _context;

        public StudentRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Student GetById(long id)
        {
            return _context.Students.FirstOrDefault(x => x.Id == id);
        }

        public Student Create(Student itemToCreate)
        {
            var student = _context.Students.Add(itemToCreate);
            _context.SaveChanges();
            return student;
        }

        public IQueryable<Student> Query(Expression<Func<Student, Student>> expression)
        {
            return _context.Students.Select(expression);
        }

        public IQueryable<Student> Filter(Expression<Func<Student, bool>> expression)
        {
            return _context.Students.Where(expression);
        }

        public Student Update(Student itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Student Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Students.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Student Delete(Student itemToDelete)
        {
            _context.Students.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return Query(x => x).ToList();
        }
    }
}
