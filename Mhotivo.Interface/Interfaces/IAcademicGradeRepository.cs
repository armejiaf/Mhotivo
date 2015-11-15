using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicGradeRepository
    {
        AcademicGrade GetById(long id);
        AcademicGrade Create(AcademicGrade itemToCreate);
        IQueryable<AcademicGrade> Query(Expression<Func<AcademicGrade, AcademicGrade>> expression);
        IQueryable<AcademicGrade> Filter(Expression<Func<AcademicGrade, bool>> expression);
        AcademicGrade Update(AcademicGrade itemToUpdate);
        AcademicGrade Delete(long id);
        AcademicGrade Delete(AcademicGrade grade);
        IEnumerable<AcademicGrade> GetAllGrades();
    }
}
