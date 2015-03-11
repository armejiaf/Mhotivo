
﻿using System;
using System.Collections.Generic;

﻿using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

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

        //public AcademicYear First(Expression<Func<AcademicYear, AcademicYear>> query)
        //{
        //    var academicYear = _context.AcademicYears.Select(query);
        //    return academicYear.Count() != 0 ? academicYear.Include(x => x.Grade).First() : null;

        
        public AcademicYear First(Expression<Func<AcademicYear, AcademicYear>> query)
        {
            IQueryable<AcademicYear> academicYear = _context.AcademicYears.Select(query);
            return academicYear.Count() != 0 ? academicYear.First() : null;

        }

        public AcademicYear GetById(long id)
        {

            var academicYear = _context.AcademicYears.Where(x => x.Id == id);
            return academicYear.Count() != 0 ? academicYear.Include(x => x.Grade).First() : null;


            //var academicYear = _context.AcademicYears.Where(x => x.Id == id && x.Approved);
            //return academicYear.Count() != 0 ? academicYear.Include(x => x.Grade).First() : null;


        }

        public AcademicYear Create(AcademicYear itemToCreate)
        {

            var academicYear = _context.AcademicYears.Add(itemToCreate);
            _context.Entry(academicYear.Grade).State = EntityState.Modified;

           // AcademicYear academicYear = _context.AcademicYears.Add(itemToCreate);

            _context.SaveChanges();
            CreateDefaultPensum(itemToCreate);
            return academicYear;
        }

        public IQueryable<AcademicYear> Query(Expression<Func<AcademicYear, AcademicYear>> expression)
        {

           // var academicYear = _context.AcademicYears.Select(expression);
            //return academicYear.Count() != 0 ? academicYear.Include(x => x.Grade) : academicYear;

            IQueryable<AcademicYear> myAcademicYear = _context.AcademicYears.Select(expression);
            return myAcademicYear;

        }

        public IQueryable<AcademicYear> Filter(Expression<Func<AcademicYear, bool>> expression)
        {

            var academicYear = _context.AcademicYears.Where(expression);
            return academicYear.Count() != 0 ? academicYear.Include(x => x.Grade) : academicYear;
        }

        public AcademicYear Update(AcademicYear itemToUpdate, bool updateCourse = true, bool updateGrade = true,
            bool updateTeacher = true)
        {
            if (updateGrade)
                _context.Entry(itemToUpdate.Grade).State = EntityState.Modified;

            _context.SaveChanges();

            return itemToUpdate;

            return itemToUpdate;   
//=======
//            IQueryable<AcademicYear> myAcademicYear = _context.AcademicYears.Where(expression);
//            return myAcademicYear;
//>>>>>>> homew2

        }

        public AcademicYear Update(AcademicYear displayAcademicYearModel, AcademicYear academicYear)
        {

            const bool updateCourse = false;
            var updateGrade = false;
            const bool updateTeacher = false;


//            var updateCourse = false;
//            var updateGrade = false;
//            var updateTeacher = false;


//            var ayear = GetById(itemToUpdate.Id);
//            ayear.Approved = itemToUpdate.Approved;
//            ayear.IsActive = itemToUpdate.IsActive;
//            ayear.Section = itemToUpdate.Section;
//            ayear.Year = itemToUpdate.Year;

//            if (ayear.Grade.Id != itemToUpdate.Grade.Id)
//            {
//                ayear.Grade = itemToUpdate.Grade;
//                updateGrade = true;
//            }

//            return Update(ayear, updateCourse, updateGrade, updateTeacher);  

            academicYear.Id = displayAcademicYearModel.Id;
            academicYear.Grade = displayAcademicYearModel.Grade;
            academicYear.Section = displayAcademicYearModel.Section;
            academicYear.IsActive = displayAcademicYearModel.IsActive;
            academicYear.Approved = displayAcademicYearModel.Approved;


            //return Update(academicYear, updateCourse, updateGrade, updateTeacher);

            return Update(academicYear);

        }

        public AcademicYear Delete(long id)
        {
            AcademicYear itemToDelete = GetById(id);

            _context.SaveChanges();
            return itemToDelete;
        }

        public AcademicYear GetCurrentAcademicYear()
        {
            var currentYear = DateTime.Now.Year;
          var currentAcademicYeary = _context.AcademicYears.FirstOrDefault(ay => ay.Year.Year.Equals(currentYear));
            return currentAcademicYeary ?? new AcademicYear();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void CreateDefaultPensum(AcademicYear academicYear)
        {
            var pensums = GetDefaultPensum(academicYear.Grade.Id);
            var teacher = _context.Meisters.First(x => x.FirstName.Equals("Default"));
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

        public IEnumerable<Pensum> GetDefaultPensum(int grade)
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

        public bool ExistAcademicYear(int year, int grade, string section)
        {
            var years = GetAllAcademicYears().Where(x => Equals(x.Year.Year, year) && Equals(x.Grade.Id, grade) && Equals(x.Section, section) && x.Approved);
            return years.Any();
        }

        public AcademicYear GetByFields(int year, int grade, string section)
        {
            var academicYears = GetAllAcademicYears().Where(x => Equals(x.Year.Year, year) && Equals(x.Grade.Id, grade) && Equals(x.Section, section) && x.Approved).ToArray();
            return academicYears.Any() ? academicYears.First() : null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public AcademicYear Update(AcademicYear itemToUpdate)
        {
            _context.SaveChanges();
            return itemToUpdate;
        }

        public void Detach(AcademicYear academicYear)
        {
            _context.Entry(academicYear).State = EntityState.Detached;
        }
    }
}