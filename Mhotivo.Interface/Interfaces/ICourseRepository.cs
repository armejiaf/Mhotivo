using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface ICourseRepository :  IDisposable
    {
        Course Create(Course itemToCreate);
        Course Delete(long id);
        IEnumerable<Course> GetAllCourse();
        IEnumerable<EducationLevel> GetAllAreas();
        IQueryable<Course> Filter(Expression<Func<Course, bool>> expression);
        Course GetById(long id);
        Course GetCourseEditModelById(long id);
        Course GenerateCourseFromRegisterModel(Course courseRegisterModel);
        IQueryable<TResult> Query<TResult>(Expression<Func<Course, TResult>> expression);
        IQueryable<TResult> QueryAreaResults<TResult>(Expression<Func<EducationLevel, TResult>> expression);
        Course Update(Course itemToUpdate);
        Course UpdateCourseFromCourseEditModel(Course courseEditModel, Course course);
    }
}