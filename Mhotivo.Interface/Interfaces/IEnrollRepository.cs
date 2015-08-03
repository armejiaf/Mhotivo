using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IEnrollRepository
    {
        Enroll First(Expression<Func<Enroll, Enroll>> query);
        Enroll GetById(long id);
        Enroll Create(Enroll itemToCreate);
        IQueryable<Enroll> Query(Expression<Func<Enroll, Enroll>> expression);
        IQueryable<Enroll> Filter(Expression<Func<Enroll, bool>> expression);
        Enroll Update(Enroll itemToUpdate, bool academicYear, bool student);
        Enroll Delete(long id);
        void SaveChanges();
        IEnumerable<Enroll> GetAllsEnrolls(); 
    }
}