using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mhotivo.Implement.Repositories
{
    public class AcademicYearDetailRepository : IAcademicYearDetailRepository
    {
        private readonly MhotivoContext _context;

        public AcademicYearDetailRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public AcademicYearDetail First(Expression<Func<AcademicYearDetail, AcademicYearDetail>> query)
        {
            IQueryable<AcademicYearDetail> academicYearDetail = _context.AcademicYearDetails.Select(query);
            return academicYearDetail.Count() != 0 ? academicYearDetail.First() : null;
        }

        public AcademicYearDetail FindByCourse(long id)
        {
            IQueryable<AcademicYearDetail> academicYearDetail =
                _context.AcademicYearDetails.Where(x => x.Course.Id == id && !false);
            return academicYearDetail.Count() != 0 ? academicYearDetail.First() : null;
        }

        public AcademicYearDetail GetById(long id)
        {
            IQueryable<AcademicYearDetail> academicYearDetail =
                _context.AcademicYearDetails.Where(x => x.Id == id && !false);
            return academicYearDetail.Count() != 0 ? academicYearDetail.First() : null;
        }

        public AcademicYearDetail FindByAcademicYear(long id)
        {
            IQueryable<AcademicYearDetail> academicYearDetail =
                _context.AcademicYearDetails.Where(x => x.AcademicYear.Id == id && !false);
            return academicYearDetail.Count() != 0 ? academicYearDetail.First() : null;
        }

        public AcademicYearDetail Create(AcademicYearDetail itemToCreate)
        {
            AcademicYearDetail academicYearDetail = _context.AcademicYearDetails.Add(itemToCreate);
            _context.SaveChanges();
            return academicYearDetail;
        }

        public IQueryable<AcademicYearDetail> Query(Expression<Func<AcademicYearDetail, AcademicYearDetail>> expression)
        {
            IQueryable<AcademicYearDetail> myAcademicYearDetail = _context.AcademicYearDetails.Select(expression);
            return myAcademicYearDetail;
        }

        public IQueryable<AcademicYearDetail> Filter(Expression<Func<AcademicYearDetail, bool>> expression)
        {
            IQueryable<AcademicYearDetail> myAcademicYearDetail = _context.AcademicYearDetails.Where(expression);
            return myAcademicYearDetail;
        }

        public AcademicYearDetail Update(AcademicYearDetail itemToUpdate)
        {
            _context.SaveChanges();
            return itemToUpdate;
        }

        public AcademicYearDetail Delete(long id)
        {
            AcademicYearDetail itemToDelete = GetById(id);
            _context.SaveChanges();
            return itemToDelete;
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

        public AcademicYearDetail GetAcademicYearDetailEditModelById(long id)
        {
            AcademicYearDetail academicYearDetail = GetById(id);
            return new AcademicYearDetail
            {
                Id = academicYearDetail.Id,
                Course = academicYearDetail.Course,
                AcademicYear = academicYearDetail.AcademicYear,
                Room = academicYearDetail.Room,
                Schedule = academicYearDetail.Schedule,
                Teacher = academicYearDetail.Teacher
            };
        }

        public AcademicYearDetail GetAcademicYearDetailDisplayModelById(long id)
        {
            AcademicYearDetail academicYearDetail = GetById(id);
            return new AcademicYearDetail
            {
                Id = academicYearDetail.Id,
                Course = academicYearDetail.Course,
                AcademicYear = academicYearDetail.AcademicYear,
                Room = academicYearDetail.Room,
                Schedule = academicYearDetail.Schedule,
                Teacher = academicYearDetail.Teacher
            };
        }

        public AcademicYearDetail UpdateAcademicYearDetailFromHomeworkEditModel(
            AcademicYearDetail academicYearDetailEditModel,
            AcademicYearDetail homework)
        {
            homework.Id = homework.Id;
            homework.Course = homework.Course;
            homework.AcademicYear = homework.AcademicYear;
            homework.Room = homework.Room;
            homework.Schedule = homework.Schedule;
            homework.Teacher = homework.Teacher;

            return Update(homework);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public AcademicYearDetail GenerateAcademicYearDetailFromRegisterModel(
            AcademicYearDetail academicYearDetailRegisterModel)
        {
            return new AcademicYearDetail
            {
                Id = academicYearDetailRegisterModel.Id,
                Course = academicYearDetailRegisterModel.Course,
                AcademicYear = academicYearDetailRegisterModel.AcademicYear,
                Room = academicYearDetailRegisterModel.Room,
                Schedule = academicYearDetailRegisterModel.Schedule,
                Teacher = academicYearDetailRegisterModel.Teacher
            };
        }
    }
}