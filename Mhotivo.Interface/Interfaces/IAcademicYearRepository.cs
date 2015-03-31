using Mhotivo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAcademicYearRepository 
    {
        AcademicYear First(Expression<Func<AcademicYear, AcademicYear>> query);

        AcademicYear GetById(long id);

        AcademicYear Create(AcademicYear academicYearToCreate);


        //AcademicYear Create(AcademicYear itemToCreate);


        IQueryable<AcademicYear> Query(Expression<Func<AcademicYear, AcademicYear>> expression);

        IQueryable<AcademicYear> Filter(Expression<Func<AcademicYear, bool>> expression);

        AcademicYear Update(AcademicYear itemToUpdate);


        AcademicYear Update(AcademicYear displayAcademicYear, AcademicYear academicYear);


        AcademicYear Delete(long id);

        AcademicYear GetCurrentAcademicYear();

        void SaveChanges();
        void CreateDefaultPensum(AcademicYear academicYear);
        IEnumerable<Pensum> GetDefaultPensum(int grade);
        IEnumerable<AcademicYear> GetAllAcademicYears();
        bool ExistAcademicYear(int year, int grade, string section);
        AcademicYear GetByFields(int year, int grade, string section);
    }
}