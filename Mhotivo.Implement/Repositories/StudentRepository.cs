using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly MhotivoContext _context;

        public StudentRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public MhotivoContext GeContext()
        {
            return _context;
        }
        public static StudentRepository Instance
        {
            get { return new StudentRepository(new MhotivoContext()); }
        }

        public Student First(Expression<Func<Student, Student>> query)
        {
            var student = _context.Students.Select(query);
            return student.Count() != 0 ? student.Include(x => x.Tutor1).First() : null;
        }

        public Student GetById(long id)
        {
            var student = _context.Students.Where(x => x.Id == id);
            return student.Count() != 0 ? student.Include(x => x.Tutor1).First() : null;
        }

        public Student GetByIdNumber(string idNumber)
        {
            var student = _context.Students.Where(x => x.IdNumber == idNumber);
            return student.Count() != 0 ? student.Include(x => x.Tutor1).First() : null;
        }

        public Student Create(Student itemToCreate)
        {
            var student = _context.Students.Add(itemToCreate);
            _context.SaveChanges();
            return student;
        }

        public IQueryable<Student> Query(Expression<Func<Student, Student>> expression)
        {
            var myStudents = _context.Students.Select(expression);
            return myStudents.Count() != 0 ? myStudents.Include(x => x.Tutor1) : myStudents;
        }

        public IQueryable<Student> Filter(Expression<Func<Student, bool>> expression)
        {
            var myStudents = _context.Students.Where(expression);
            return myStudents.Count() != 0 ? myStudents.Include(x => x.Tutor1) : myStudents;
        }

        public Student Update(Student itemToUpdate)
        {
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Student Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Students.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return Query(x => x).Where(x => !x.Disable).ToList().Select(x => new Student
            {
                Id = x.Id,
                IdNumber = x.IdNumber,
                UrlPicture = x.UrlPicture,
                FullName = x.FullName,
                BirthDate = x.BirthDate,
                Nationality = x.Nationality,
                Address = x.Address,
                City = x.City,
                State = x.State,
                Country = x.Country,
                Gender = x.Gender,
                StartDate = x.StartDate,
                BloodType = x.BloodType,
                AccountNumber = x.AccountNumber,
                Biography = x.Biography,
                Tutor1 = x.Tutor1,
                Tutor2 = x.Tutor2,
                Disable = x.Disable,
                User = x.User,
                Photo = x.Photo
            });
        }

        public Student GetStudentDisplayModelById(long id)
        {
            var student = GetById(id);
            return new Student
            {
                Id = student.Id,
                IdNumber = student.IdNumber,
                UrlPicture = student.UrlPicture,
                FirstName = student.FirstName,
                LastName = student.LastName,
                FullName = student.FullName,
                BirthDate = student.BirthDate,
                Nationality = student.Nationality,
                Address = student.Address,
                City = student.City,
                State = student.State,
                Country = student.Country,
                Gender = student.Gender,
                Contacts = student.Contacts,
                StartDate = student.StartDate,
                BloodType = student.BloodType,
                AccountNumber = student.AccountNumber,
                Biography = student.Biography,
                Tutor1 = student.Tutor1,
                Tutor2 = student.Tutor2,
                User = student.User,
                Photo = student.Photo
            };
        }

        public Student UpdateStudentFromStudentEditModel(Student studentEditModel, Student student)
        {
            student.FirstName = studentEditModel.FirstName;
            student.LastName = studentEditModel.LastName;
            student.FullName = (studentEditModel.FirstName + " " + studentEditModel.LastName).Trim();
            student.Country = studentEditModel.Country;
            student.IdNumber = studentEditModel.IdNumber;
            student.BirthDate = studentEditModel.BirthDate;
            student.Gender = studentEditModel.Gender;
            student.Nationality = studentEditModel.Nationality;
            student.State = studentEditModel.State;
            student.City = studentEditModel.City;
            student.Address = studentEditModel.Address;
            student.Biography = studentEditModel.Biography;
            student.StartDate = studentEditModel.StartDate;
            student.BloodType = studentEditModel.BloodType;
            student.AccountNumber = studentEditModel.AccountNumber;
            student.Tutor1 = studentEditModel.Tutor1;
            student.Tutor2 = studentEditModel.Tutor2;
            student.User = studentEditModel.User;
            student.Photo = studentEditModel.Photo;
            return Update(student);
        }

        public Student GenerateStudentFromRegisterModel(Student studentRegisterModel)
        {
   
            return new Student
            {
                FirstName = studentRegisterModel.FirstName,
                LastName = studentRegisterModel.LastName,
                FullName = (studentRegisterModel.FirstName + " " + studentRegisterModel.LastName).Trim(),
                IdNumber = studentRegisterModel.IdNumber,
                BirthDate = studentRegisterModel.BirthDate,
                Gender = studentRegisterModel.Gender,
                Nationality = studentRegisterModel.Nationality,
                State = studentRegisterModel.State,
                Country = studentRegisterModel.Country,
                City = studentRegisterModel.City,
                Address = studentRegisterModel.Address,
                Biography = studentRegisterModel.Biography,
                StartDate = studentRegisterModel.StartDate,
                BloodType = studentRegisterModel.BloodType,
                AccountNumber = studentRegisterModel.AccountNumber,
                Tutor1 = studentRegisterModel.Tutor1,
                Tutor2 = studentRegisterModel.Tutor2,
                User = studentRegisterModel.User
            };
        }

        public Student GetStudentEditModelById(long id)
        {
            var student = GetById(id);

            return new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                FullName = (student.FirstName + " " + student.LastName).Trim(),
                IdNumber = student.IdNumber,
                BirthDate = student.BirthDate,
                Gender = student.Gender,
                Nationality = student.Nationality,
                Country = student.Country,
                State = student.State,
                City = student.City,
                Address = student.Address,
                Id = student.Id,
                StartDate = student.StartDate,
                Biography = student.Biography,
                AccountNumber = student.AccountNumber,
                BloodType = student.BloodType,
                Tutor1 = student.Tutor1,
                Tutor2 = student.Tutor2,
                User = student.User
            };
        }
    }
}
