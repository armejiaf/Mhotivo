using System;
using System.Collections.Generic;
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
    public class EnrollRepository : IEnrollRepository
    {
        private readonly MhotivoContext _context;

        public EnrollRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public MhotivoContext GeContext()
        {
            return _context;
        }

        public Enroll First(Expression<Func<Enroll, Enroll>> query)
        {
            var enroll = _context.Enrolls.Select(query);
            return enroll.Count() != 0 ? enroll.First() : null;
        }

        public Enroll GetById(long id)
        {
            var enroll = _context.Enrolls.Where(x => x.Id == id);
            return enroll.Count() != 0 ? enroll.First() : null;
        }

        public Enroll Create(Enroll itemToCreate)
        {
            var enroll = _context.Enrolls.Add(itemToCreate);
            _context.SaveChanges();
            return enroll;
        }

        public IQueryable<Enroll> Query(Expression<Func<Enroll, Enroll>> expression)
        {
            return _context.Enrolls.Select(expression);
        }

        public IQueryable<Enroll> Filter(Expression<Func<Enroll, bool>> expression)
        {
            var myEnrolls = _context.Enrolls.Where(expression);
            return myEnrolls.Count() != 0 ? myEnrolls : null;
        }

        public Enroll Update(Enroll itemToUpdate, bool academicYear, bool student)
        {
            if (academicYear)
            {
                _context.Entry(itemToUpdate.AcademicYear).State = EntityState.Modified;
            }
            if (student)
            {
                _context.Entry(itemToUpdate.Student).State = EntityState.Modified;
            }
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Enroll Update(Enroll itemToUpdate)
        {
            var enroll = GetById(itemToUpdate.Id);
            bool academicYear = false;
            bool student = false;

            if (enroll.AcademicYear != itemToUpdate.AcademicYear)
            {
                enroll.AcademicYear = itemToUpdate.AcademicYear;
                academicYear = true;
            }
            if (enroll.Student != itemToUpdate.Student)
            {
                enroll.Student = itemToUpdate.Student;
                student = true;
            }

            return Update(enroll, academicYear, student);

        }

        public Enroll Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Enrolls.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Enroll> GetAllsEnrolls()
        {
           
            return Query(x => x).ToList().Select(x => new Enroll
            {
                Id = x.Id,
               AcademicYear = x.AcademicYear,
               Student = x.Student

            });
        
        }

        public void Detach(Enroll enroll)
        {
            _context.Entry(enroll).State = EntityState.Detached;
        }
    }
}
