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
    public class AcademicYearRepository : IAcademicYearRepository
    {
        private readonly MhotivoContext _context;

        public AcademicYearRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public MhotivoContext GeContext()
        {
            return _context;
        }

        public AcademicYear First(Expression<Func<AcademicYear, AcademicYear>> query)
        {
            var academicYear = _context.AcademicYears.Select(query);
            return academicYear.Count() != 0 ? academicYear.Include(x => x.Grade).First() : null;
        }

        public AcademicYear GetById(long id)
        {
            var academicYear = _context.AcademicYears.Where(x => x.Id == id);
            return academicYear.Count() != 0 ? academicYear.Include(x => x.Grade).First() : null;
        }

        public AcademicYear Create(AcademicYear itemToCreate)
        {
            var academicYear = _context.AcademicYears.Add(itemToCreate);
            _context.Entry(academicYear.Grade).State = EntityState.Modified;
            _context.SaveChanges();
            //CreateDefaultPensum(itemToCreate);
            return academicYear;
        }

        public IQueryable<AcademicYear> Query(Expression<Func<AcademicYear, AcademicYear>> expression)
        {
            var academicYear = _context.AcademicYears.Select(expression);
            return academicYear.Count() != 0 ? academicYear.Include(x => x.Grade) : academicYear;
        }

        public IQueryable<AcademicYear> Filter(Expression<Func<AcademicYear, bool>> expression)
        {
            var academicYear = _context.AcademicYears.Where(expression);
            return academicYear.Count() != 0 ? academicYear.Include(x => x.Grade) : academicYear;
        }

        //Logic is weird. Lots of unused variables being thrown around these two functions.
        public AcademicYear Update(AcademicYear itemToUpdate, bool updateCourse = true, bool updateGrade = true,
            bool updateTeacher = true)
        {
            if (updateGrade)
                _context.Entry(itemToUpdate.Grade).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public AcademicYear Update(AcademicYear itemToUpdate)
        {
            const bool updateCourse = false;
            var updateGrade = false;
            const bool updateTeacher = false;
            var ayear = GetById(itemToUpdate.Id);
            ayear.Approved = itemToUpdate.Approved;
            ayear.IsActive = itemToUpdate.IsActive;
            ayear.Section = itemToUpdate.Section;
            ayear.Year = itemToUpdate.Year;
            if (ayear.Grade.Id != itemToUpdate.Grade.Id)
            {
                ayear.Grade = itemToUpdate.Grade;
                updateGrade = true;
            }
            return Update(ayear, updateCourse, updateGrade, updateTeacher);
        }

        public AcademicYear Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.AcademicYears.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void CreateDefaultPensum(AcademicYear academicYear)
        {
            var pensums = GetDefaultPensum(academicYear.Grade.Id);
            var teacher = _context.Teachers.First(x => x.FirstName.Equals("Maestro"));
            foreach (var pensum in pensums)
            {
                var academicYearDetails = new AcademicYearDetail
                {
                    TeacherStartDate = DateTime.Now,
                    TeacherEndDate = DateTime.Now,
                    Schedule = DateTime.Now,
                    AcademicYear = academicYear,
                    Course = pensum.Course,
                    Teacher = teacher
                };
                _context.AcademicYearDetails.Add(academicYearDetails);
            }
            _context.SaveChanges();
        }

        public IEnumerable<Pensum> GetDefaultPensum(long grade)
        {
            var pensum = _context.Pensums.Where(x => (x.Grade.Id == grade));
            return pensum.Count() != 0 ? pensum.Include(x => x.Course).Include(x => x.Grade) : null;
        }

        public IEnumerable<AcademicYear> GetAllAcademicYears()
        {
            return Query(x => x).ToList().Select(x => new AcademicYear
            {
                Id = x.Id,
                Approved = x.Approved,
                Grade = x.Grade,
                IsActive = x.IsActive,
                Section = x.Section,
                Year = x.Year
            });
        }

        public bool ExistAcademicYear(int year, long grade, string section)
        {
            var years = GetAllAcademicYears().Where(x => Equals(x.Year.Year, year) && Equals(x.Grade.Id, grade) && Equals(x.Section, section) && x.Approved);
            return years.Any();
        }

        public AcademicYear GetByFields(int year, long grade, string section)
        {
            var academicYears = GetAllAcademicYears().Where(x => Equals(x.Year.Year, year) && Equals(x.Grade.Id, grade) && Equals(x.Section, section) && x.Approved).ToArray();
            return academicYears.Any() ? academicYears.First() : null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Detach(AcademicYear academicYear)
        {
            _context.Entry(academicYear).State = EntityState.Detached;
        }

        public AcademicYear GetCurrentAcademicYear()
        {
            var currentYear = DateTime.Now.Year;
            var currentAcademicYeary = _context.AcademicYears.FirstOrDefault(ay => ay.Year.Year.Equals(currentYear));
            return currentAcademicYeary ?? new AcademicYear();
        }
    }
}