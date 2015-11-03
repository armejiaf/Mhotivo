using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicYearRepository
    {
        AcademicYear GetById(long id);
        AcademicYear Create(AcademicYear academicYearGradeToCreate);
        IQueryable<AcademicYear> Query(Expression<Func<AcademicYear, AcademicYear>> expression);
        IQueryable<AcademicYear> Filter(Expression<Func<AcademicYear, bool>> expression);
        AcademicYear Update(AcademicYear itemToUpdate);
        AcademicYear Delete(long id);
        AcademicYear Delete(AcademicYear itemToDelete);
        IEnumerable<AcademicYear> GetAllAcademicYears();
        AcademicYear GetCurrentAcademicYear();
    }
}