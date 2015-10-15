using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly MhotivoContext _context;

        public TeacherRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Teacher First(Expression<Func<Teacher, Teacher>> query)
        {
            var teachers = _context.Teachers.Select(query);
            return teachers.Count() != 0 ? teachers.First() : null;
        }

        public Teacher FirstOrDefault(Expression<Func<Teacher, bool>> query)
        {
            var teacher = _context.Teachers.FirstOrDefault(query);
            return teacher;
        }

        public Teacher GetById(long id)
        {
            var teachers = _context.Teachers.Where(x => x.Id == id);
            return teachers.Count() != 0 ? teachers.First() : null;
        }

        public Teacher Create(Teacher itemToCreate)
        {
            _context.Users.Attach(itemToCreate.MyUser);
            var teacher = _context.Teachers.Add(itemToCreate);
            _context.SaveChanges();
            return teacher;
        }

        public IQueryable<Teacher> Query(Expression<Func<Teacher, Teacher>> expression)
        {
            return _context.Teachers.Select(expression);
        }

        public IQueryable<Teacher> Filter(Expression<Func<Teacher, bool>> expression)
        {
            return _context.Teachers.Where(expression);
        }

        public Teacher Update(Teacher itemToUpdate)
        {
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Teacher Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Teachers.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }
        //Is all that truly necessary? Lots of repeated code.
        public IEnumerable<Teacher> GetAllTeachers()
        {
            var test = _context.Teachers.FirstOrDefault(x => x.FirstName == "Hans");
            return Query(x => x).Include(x => x.MyUser).ToList().Select(x => new Teacher
                {
                    Id = x.Id,
                    IdNumber = x.IdNumber,
                    FullName = x.FullName,
                    BirthDate = x.BirthDate,
                    Nationality = x.Nationality,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    Country = x.Country,
                    MyGender = x.MyGender,
                    Contacts = x.Contacts,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Biography = x.Biography,
                    Photo = x.Photo,
                    MyUser = x.MyUser
                });
        }

        public Teacher GetTeacherDisplayModelById(long id)
        {
            var teacher = GetById(id);
            return new Teacher
            {
                Id = teacher.Id,
                IdNumber = teacher.IdNumber,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                FullName = teacher.FullName,
                BirthDate = teacher.BirthDate,
                Nationality = teacher.Nationality,
                Address = teacher.Address,
                City = teacher.City,
                State = teacher.State,
                Country = teacher.Country,
                MyGender = teacher.MyGender,
                Contacts = teacher.Contacts,
                StartDate = teacher.StartDate,
                EndDate = teacher.EndDate,
                Biography = teacher.Biography
            };
        }

        public Teacher UpdateTeacherFromTeacherEditModel(Teacher teacherEditModel, Teacher teacher)
        {
            teacher.FirstName = teacherEditModel.FirstName;
            teacher.LastName = teacherEditModel.LastName;
            teacher.FullName = (teacherEditModel.FirstName + " " + teacherEditModel.LastName).Trim();
            teacher.Country = teacherEditModel.Country;
            teacher.IdNumber = teacherEditModel.IdNumber;
            teacher.BirthDate =teacherEditModel.BirthDate;
            teacher.MyGender = teacherEditModel.MyGender;
            teacher.Nationality = teacherEditModel.Nationality;
            teacher.State = teacherEditModel.State;
            teacher.City = teacherEditModel.City;
            teacher.Address = teacherEditModel.Address;
            teacher.Biography = teacherEditModel.Biography;
            teacher.StartDate = teacherEditModel.StartDate;
            teacher.EndDate = teacherEditModel.EndDate;
            teacher.Photo = teacherEditModel.Photo;
            return Update(teacher);
        }

        public Teacher GenerateTeacherFromRegisterModel(Teacher teacherRegisterModel)
        {
            return new Teacher
            {
                FirstName = teacherRegisterModel.FirstName,
                LastName = teacherRegisterModel.LastName,
                FullName = (teacherRegisterModel.FirstName + " " + teacherRegisterModel.LastName).Trim(),
                IdNumber = teacherRegisterModel.IdNumber,
                BirthDate = teacherRegisterModel.BirthDate,
                MyGender = teacherRegisterModel.MyGender,
                Nationality = teacherRegisterModel.Nationality,
                State = teacherRegisterModel.State,
                Country = teacherRegisterModel.Country,
                City = teacherRegisterModel.City,
                Address = teacherRegisterModel.Address,
                Biography = teacherRegisterModel.Biography,
                StartDate = teacherRegisterModel.StartDate,
                EndDate = teacherRegisterModel.EndDate
            };
        }

        public Teacher GetTeacherEditModelById(long id)
        {
            var teacher = GetById(id);
            return new Teacher
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                FullName = (teacher.FirstName + " " + teacher.LastName).Trim(),
                IdNumber = teacher.IdNumber,
                BirthDate = teacher.BirthDate,
                MyGender = teacher.MyGender,
                Nationality = teacher.Nationality,
                Country = teacher.Country,
                State = teacher.State,
                City = teacher.City,
                Address = teacher.Address,
                Id = teacher.Id,
                StartDate = teacher.StartDate,
                EndDate = teacher.EndDate,
                Biography = teacher.Biography,
                Photo = teacher.Photo
            };
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public bool ExistIdNumber(string idNumber)
        {
            var teacherWithIdNumber = _context.Teachers.Where(x => x.IdNumber.Equals(idNumber));
            return teacherWithIdNumber.Any();
        }

        public bool ExistEmail(string email)
        {
            var teacher = _context.Users.Where(x => x.Email.Equals(email));
            return teacher.Any();
        }
    }
}