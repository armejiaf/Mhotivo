using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface ITeacherRepository 
    {
        Teacher GetById(long id);
        Teacher Create(Teacher itemToCreate);
        IQueryable<Teacher> Query(Expression<Func<Teacher, Teacher>> expression);
        IQueryable<Teacher> Filter(Expression<Func<Teacher, bool>> expression);
        Teacher Update(Teacher itemToUpdate);
        Teacher Delete(long id);
        Teacher Delete(Teacher itemToDelete);
        IEnumerable<Teacher> GetAllTeachers();
    }
}