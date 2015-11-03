using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicYearCourseRepository
    {
        AcademicYearCourse GetById(long id);
        AcademicYearCourse Create(AcademicYearCourse academicYearToCreate);
        IQueryable<AcademicYearCourse> Query(Expression<Func<AcademicYearCourse, AcademicYearCourse>> expression);
        IQueryable<AcademicYearCourse> Filter(Expression<Func<AcademicYearCourse, bool>> expression);
        AcademicYearCourse Update(AcademicYearCourse itemToUpdate);
        AcademicYearCourse Delete(long id);
        AcademicYearCourse Delete(AcademicYearCourse itemToDelete);
        IEnumerable<AcademicYearCourse> GetAllAcademicYearDetails();
    }
}
