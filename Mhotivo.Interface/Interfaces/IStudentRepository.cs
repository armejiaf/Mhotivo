using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IStudentRepository
    {
        Student First(Expression<Func<Student, Student>> query);
        Student GetById(long id);
        Student GetByIdNumber(string idNumber);
        Student Create(Student itemToCreate);
        IQueryable<Student> Query(Expression<Func<Student, Student>> expression);
        IQueryable<Student> Filter(Expression<Func<Student, bool>> expression);
        Student Update(Student itemToUpdate);
        Student Delete(long id);
        Student GenerateStudentFromRegisterModel(Student studentRegisterModel);
        Student GetStudentEditModelById(long id);
        Student GetStudentDisplayModelById(long id);
        Student UpdateStudentFromStudentEditModel(Student studentEditModel, Student student);
        void SaveChanges();
        IEnumerable<Student> GetAllStudents();
    }
}