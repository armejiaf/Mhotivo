using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicYearDetailsRepository : IDisposable
    {
        AcademicYearDetail First(Expression<Func<AcademicYearDetail, AcademicYearDetail>> query);
        AcademicYearDetail GetById(int id);
        AcademicYearDetail Create(AcademicYearDetail academicYearToCreate);
        IQueryable<AcademicYearDetail> Query(Expression<Func<AcademicYearDetail, AcademicYearDetail>> expression);
        IQueryable<AcademicYearDetail> Filter(Expression<Func<AcademicYearDetail, bool>> expression);
        IEnumerable<AcademicYearDetail> GetAllAcademicYearsDetails(int id);
        AcademicYearDetail Update(AcademicYearDetail itemToUpdate);
        AcademicYearDetail Delete(int id);
        void SaveChanges();
        IEnumerable<AcademicYear> GetAllAcademicYear();
    }
}
