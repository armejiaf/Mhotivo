using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicYearGradeRepository
    {
        AcademicYearGrade GetById(long id);
        AcademicYearGrade Create(AcademicYearGrade itemToCreate);
        IQueryable<AcademicYearGrade> Query(Expression<Func<AcademicYearGrade, AcademicYearGrade>> expression);
        IQueryable<AcademicYearGrade> Filter(Expression<Func<AcademicYearGrade, bool>> expression);
        AcademicYearGrade Update(AcademicYearGrade itemToUpdate);
        AcademicYearGrade Delete(long id);
        AcademicYearGrade Delete(AcademicYearGrade grade);
        IEnumerable<AcademicYearGrade> GetAllGrades();
    }
}
