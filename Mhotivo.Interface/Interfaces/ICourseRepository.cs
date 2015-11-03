using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface ICourseRepository
    {
        Course Create(Course itemToCreate);
        Course Delete(long id);
        Course Delete(Course itemToDelete);
        IEnumerable<Course> GetAllCourse();
        IQueryable<Course> Filter(Expression<Func<Course, bool>> expression);
        Course GetById(long id);
        IQueryable<Course> Query(Expression<Func<Course, Course>> expression);
        Course Update(Course itemToUpdate);
    }
}