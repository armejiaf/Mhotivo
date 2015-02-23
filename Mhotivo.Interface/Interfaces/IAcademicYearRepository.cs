using System;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicYearRepository : IDisposable
    {
        AcademicYear First(Expression<Func<AcademicYear, AcademicYear>> query);
        AcademicYear GetById(long id);
        AcademicYear Create(AcademicYear itemToCreate);
        IQueryable<AcademicYear> Query(Expression<Func<AcademicYear, AcademicYear>> expression);
        IQueryable<AcademicYear> Filter(Expression<Func<AcademicYear, bool>> expression);
        AcademicYear Update(AcademicYear itemToUpdate, bool updateCourse, bool updateGrade, bool updateTeacher);
        AcademicYear Delete(long id);
        int GetCurrentAcademicYear();
        void SaveChanges();
    }
}