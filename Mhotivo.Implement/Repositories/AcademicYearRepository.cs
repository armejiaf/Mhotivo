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
    public class AcademicYearRepository : IAcademicYearRepository
    {
        private readonly MhotivoContext _context;

        public AcademicYearRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public AcademicYear First(Expression<Func<AcademicYear, AcademicYear>> query)
        {
            //var ayears = _context.AcademicYears.Select(query);
            return null;
            //return ayears.Count() != 0 ? 
            //    ayears.Include(x => x.Course)
            //          .Include(x=> x.Grade)
            //          .Include(x=> x.Teacher)
            //          .First() : null;
        }

        public AcademicYear GetById(long id)
        {
            //var ayears = _context.AcademicYears.Where(x => x.Id == id);
            return null;
            //return ayears.Count() != 0 ?
            //    ayears.Include(x => x.Course)
            //          .Include(x => x.Grade)
            //          .Include(x => x.Teacher)
            //          .First() : null;
        }

        public AcademicYear Create(AcademicYear itemToCreate)
        {
            //var ayear = _context.AcademicYears.Add(itemToCreate);
            //_context.Entry(ayear.Course).State = EntityState.Modified;
            //_context.Entry(ayear.Grade).State = EntityState.Modified;
            //_context.Entry(ayear.Teacher).State = EntityState.Modified;
            //_context.SaveChanges();
            //return ayear;
            return null;
        }

        public IQueryable<AcademicYear> Query(Expression<Func<AcademicYear, AcademicYear>> expression)
        {
            //var ayears = _context.AcademicYears.Select(expression);
            //return ayears.Count() != 0 ?
            //    ayears.Include(x => x.Course)
            //          .Include(x => x.Grade)
            //          .Include(x => x.Teacher) : ayears;
            return null;
        }

        public IQueryable<AcademicYear> Filter(Expression<Func<AcademicYear, bool>> expression)
        {
            //var ayears = _context.AcademicYears.Where(expression);
            //return ayears.Count() != 0 ?
            //    ayears.Include(x => x.Course)
            //          .Include(x => x.Grade)
            //          .Include(x => x.Teacher) : ayears;
            return null;
        }

        public AcademicYear Update(AcademicYear itemToUpdate, bool updateCourse = true, bool updateGrade = true, 
            bool updateTeacher = true)
        {
            //if (updateCourse)
            //    _context.Entry(itemToUpdate.Course).State = EntityState.Modified;

            //if (updateGrade)
            //    _context.Entry(itemToUpdate.Grade).State = EntityState.Modified;

            //if (updateTeacher)
            //    _context.Entry(itemToUpdate.Teacher).State = EntityState.Modified;

            //_context.SaveChanges();
            //return itemToUpdate;   
            return null;
        }

        public AcademicYear Update(AcademicYear itemToUpdate)
        {
            var updateCourse = false;
            var updateGrade = false;
            var updateTeacher = false;

            //var ayear = GetById(itemToUpdate.Id);
            //ayear.Approved = itemToUpdate.Approved;
            //ayear.IsActive = itemToUpdate.IsActive;
            //ayear.Room = itemToUpdate.Room;
            //ayear.Schedule = itemToUpdate.Schedule;
            //ayear.Section = itemToUpdate.Section;
            //ayear.StudentsLimit = itemToUpdate.StudentsLimit;
            //ayear.TeacherEndDate = itemToUpdate.TeacherEndDate;
            //ayear.TeacherStartDate = itemToUpdate.TeacherStartDate;
            //ayear.Year = itemToUpdate.Year;
            //ayear.StudentsCount = itemToUpdate.StudentsCount;

            //if (ayear.Course.Id != itemToUpdate.Course.Id)
            //{
            //    ayear.Course = itemToUpdate.Course;
            //    updateCourse = true;
            //}

            //if (ayear.Grade.Id != itemToUpdate.Grade.Id)
            //{
            //    ayear.Course = itemToUpdate.Course;
            //    updateGrade = true;
            //}

            //if (ayear.Teacher.Id != itemToUpdate.Teacher.Id)
            //{
            //    ayear.Teacher = itemToUpdate.Teacher;
            //    updateTeacher = true;
            //}

            //return Update(ayear, updateCourse, updateGrade, updateTeacher);  
            return null;
        }

        public AcademicYear Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.AcademicYears.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public int GetCurrentAcademicYear()
        {
            var currentYear = DateTime.Now.Year;

            return currentYear;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Detach(AcademicYear academicYear)
        {
            _context.Entry(academicYear).State = EntityState.Detached;
        }
    }
}