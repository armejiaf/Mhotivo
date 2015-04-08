using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class AcademicYearDetailsRepository : IAcademicYearDetailsRepository
    {
        private readonly MhotivoContext _context;

        public AcademicYearDetailsRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public AcademicYearDetail FindByCourse(long id)
        {
            IQueryable<AcademicYearDetail> academicYearDetail =
                _context.AcademicYearDetails.Where(x => x.Course.Id == id && !false);
            return academicYearDetail.Count() != 0 ? academicYearDetail.First() : null;
        }

        public IEnumerable<AcademicYear> GetAllAcademicYear(long mesiterId)
        {
            IQueryable<AcademicYearDetail> academicYearDetail = _context.AcademicYearDetails.Include(x => x.AcademicYear).Include(c => c.Teacher).Where(x => x.Teacher.Id == mesiterId); ;

            return academicYearDetail.ToList().Select(x => new AcademicYear
            {
                Id = x.AcademicYear.Id,
                Approved = x.AcademicYear.Approved,
                Grade = x.AcademicYear.Grade,
                IsActive = x.AcademicYear.IsActive,
                Section = x.AcademicYear.Section,
                Year = x.AcademicYear.Year
            });
        }
        public IEnumerable<AcademicYearDetail> GetAllAcademicYearDetails()
        {
            return Query(x => x).Where(x => !false).ToList().Select(x => new AcademicYearDetail
            {
                Id = x.Id,
                Course = x.Course,
                AcademicYear = x.AcademicYear,
                Room = x.Room,
                Schedule = x.Schedule,
                Teacher = x.Teacher
            });
        }
        public AcademicYearDetail First(Expression<Func<AcademicYearDetail, AcademicYearDetail>> query)
        {
            var academicYearDetails = _context.AcademicYearDetails.Select(query);
            return academicYearDetails.Count() != 0 ? academicYearDetails.Include(x => x.AcademicYear)
                                                                         .Include(x =>x.Course)
                                                                         .Include(x => x.Teacher).First() : null;
        }

        public AcademicYearDetail GetById(int id)
        {
            var academicYearDetails = _context.AcademicYearDetails.Where(x => x.Id == id);
            return academicYearDetails.Count() != 0 ? academicYearDetails.Include(x => x.AcademicYear)
                                                                         .Include(x => x.Course)
                                                                         .Include(x => x.Teacher).First() : null;
        }

        public AcademicYearDetail Create(AcademicYearDetail academicYearToCreate)
        {
            var academicYearDetails = _context.AcademicYearDetails.Add(academicYearToCreate);
            _context.Entry(academicYearToCreate.Course).State = EntityState.Modified;
            _context.Entry(academicYearToCreate.Teacher).State = EntityState.Modified;
            _context.SaveChanges();
            return academicYearDetails;
        }

        public IQueryable<AcademicYearDetail> Query(Expression<Func<AcademicYearDetail, AcademicYearDetail>> expression)
        {
            var academicYearDetails = _context.AcademicYearDetails.Select(expression);
            var result = academicYearDetails.Count() != 0 ? academicYearDetails.Include(x => x.AcademicYear)
                                                                         .Include(x => x.Course)
                                                                         .Include(x => x.Teacher).Where(x => x.AcademicYear.Year != null): academicYearDetails;
            return result;
        }

        public IQueryable<AcademicYearDetail> Filter(Expression<Func<AcademicYearDetail, bool>> expression)
        {
            var academicYearDetails = _context.AcademicYearDetails.Where(expression);
            return academicYearDetails.Count() != 0 ? academicYearDetails.Include(x => x.AcademicYear)
                                                                         .Include(x => x.Course)
                                                                         .Include(x => x.Teacher) : academicYearDetails;
        }

        public AcademicYearDetail Update(AcademicYearDetail itemToUpdate, bool updateAcademicYear = true, bool updateCourse = true,
            bool updateTeacher = true)
        {
            if (updateAcademicYear)
                _context.Entry(itemToUpdate.AcademicYear).State = EntityState.Modified;

            if (updateCourse)
                _context.Entry(itemToUpdate.Course).State = EntityState.Modified;

            if (updateTeacher)
                _context.Entry(itemToUpdate.Teacher).State = EntityState.Modified;

            _context.SaveChanges();
            return itemToUpdate;
        }

        public AcademicYearDetail Update(AcademicYearDetail itemToUpdate)
        {
            var updateCourse = false;
            var updateAcademicYear = false;
            var updateTeacher = false;

            var academicYearDetail = GetById(itemToUpdate.Id);
            academicYearDetail.TeacherStartDate = itemToUpdate.TeacherStartDate;
            academicYearDetail.TeacherEndDate = itemToUpdate.TeacherEndDate;
            academicYearDetail.Schedule = itemToUpdate.Schedule;
            academicYearDetail.Room = itemToUpdate.Room;

            if (academicYearDetail.AcademicYear.Id != itemToUpdate.AcademicYear.Id)
            {
                academicYearDetail.AcademicYear = itemToUpdate.AcademicYear;
                updateAcademicYear = true;
            }

            if (academicYearDetail.Course.Id != itemToUpdate.Course.Id)
            {
                academicYearDetail.Course = itemToUpdate.Course;
                updateCourse = true;
            }

            if (academicYearDetail.Teacher.Id != itemToUpdate.Teacher.Id)
            {
                academicYearDetail.Teacher = itemToUpdate.Teacher;
                updateTeacher = true;
            }

            return Update(academicYearDetail, updateAcademicYear, updateCourse, updateTeacher);
        }
        public IEnumerable<AcademicYearDetail> GetAllAcademicYearsDetails(int academicYearId)
        {
            var query = Query(x => x).ToList().Select(x => new AcademicYearDetail
            {
                Id = x.Id,
                TeacherStartDate = x.TeacherStartDate,
                TeacherEndDate = x.TeacherEndDate,
                Schedule = x.Schedule,
                Room = x.Room,
                AcademicYear = x.AcademicYear,
                Course = x.Course,
                Teacher = x.Teacher
            }).Where(x => x.AcademicYear.Id == academicYearId);
            return query;
        }

        public AcademicYearDetail Delete(int id)
        {
            var itemToDelete = GetById(id);
            _context.AcademicYearDetails.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
