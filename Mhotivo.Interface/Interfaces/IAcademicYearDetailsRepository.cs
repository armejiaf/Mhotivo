using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicYearDetailsRepository
    {
        AcademicYearDetail First(Expression<Func<AcademicYearDetail, AcademicYearDetail>> query);
        AcademicYearDetail GetById(long id);
        AcademicYearDetail Create(AcademicYearDetail academicYearToCreate);
        IQueryable<AcademicYearDetail> Query(Expression<Func<AcademicYearDetail, AcademicYearDetail>> expression);
        IQueryable<AcademicYearDetail> Filter(Expression<Func<AcademicYearDetail, bool>> expression);
        IEnumerable<AcademicYearDetail> GetAllAcademicYearsDetails(long id);
        AcademicYearDetail Update(AcademicYearDetail itemToUpdate);
        AcademicYearDetail Delete(long id);
        void SaveChanges();
        IEnumerable<AcademicYearDetail> GetAllAcademicYearDetails();
        AcademicYearDetail FindByCourse(long id, long getMeisterId);
        IEnumerable<AcademicYear> GetAllAcademicYear(long mesiterId);
    }
}
