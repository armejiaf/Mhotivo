using Mhotivo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicYearDetailRepository
    {
        AcademicYearDetail First(Expression<Func<AcademicYearDetail, AcademicYearDetail>> query);
       
        AcademicYearDetail FindByCourse(long id);

        AcademicYearDetail GetById(long id);

        AcademicYearDetail FindByAcademicYear(long id);

        AcademicYearDetail Create(AcademicYearDetail itemToCreate);

        IQueryable<AcademicYearDetail> Query(Expression<Func<AcademicYearDetail, AcademicYearDetail>> expression);

        IQueryable<AcademicYearDetail> Filter(Expression<Func<AcademicYearDetail, bool>> expression);

        AcademicYearDetail Update(AcademicYearDetail itemToUpdate);

        AcademicYearDetail Delete(long id);

        IEnumerable<AcademicYearDetail> GetAllAcademicYearDetails();

        AcademicYearDetail GetAcademicYearDetailEditModelById(long id);

        AcademicYearDetail GetAcademicYearDetailDisplayModelById(long id);

        AcademicYearDetail UpdateAcademicYearDetailFromHomeworkEditModel(AcademicYearDetail academicYearDetailEditModel, AcademicYearDetail homework);

        void SaveChanges();

        AcademicYearDetail GenerateAcademicYearDetailFromRegisterModel(AcademicYearDetail academicYearDetailRegisterModel);
    }
}