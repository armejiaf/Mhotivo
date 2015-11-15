using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicCourseRepository
    {
        AcademicCourse GetById(long id);
        AcademicCourse Create(AcademicCourse itemToCreate);
        IQueryable<AcademicCourse> Query(Expression<Func<AcademicCourse, AcademicCourse>> expression);
        IQueryable<AcademicCourse> Filter(Expression<Func<AcademicCourse, bool>> expression);
        AcademicCourse Update(AcademicCourse itemToUpdate);
        AcademicCourse Delete(long id);
        AcademicCourse Delete(AcademicCourse itemToDelete);
        IEnumerable<AcademicCourse> GetAllAcademicYearDetails();
    }
}
