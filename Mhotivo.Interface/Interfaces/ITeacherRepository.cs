using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface ITeacherRepository 
    {
        Teacher First(Expression<Func<Teacher, Teacher>> query);
        Teacher FirstOrDefault(Expression<Func<Teacher, bool>> query);
        Teacher GetById(long id);
        Teacher Create(Teacher itemToCreate);
        IQueryable<Teacher> Query(Expression<Func<Teacher, Teacher>> expression);
        IQueryable<Teacher> Filter(Expression<Func<Teacher, bool>> expression);
        Teacher Update(Teacher itemToUpdate);
        Teacher Delete(long id);
        IEnumerable<Teacher> GetAllTeachers();
        Teacher GenerateTeacherFromRegisterModel(Teacher teacherRegisterModel);
        Teacher GetTeacherEditModelById(long id);
        Teacher GetTeacherDisplayModelById(long id);
        Teacher UpdateTeacherFromTeacherEditModel(Teacher teacherEditModel, Teacher teacher);
        void SaveChanges();
        bool ExistIdNumber(string idNumber);
        bool ExistEmail(string email);
    }
}