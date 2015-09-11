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
            var meisters = _context.Teachers.Select(query);
            return meisters.Count() != 0 ? meisters.First() : null;
        }

        public Teacher FirstOrDefault(Expression<Func<Teacher, bool>> query)
        {
            var teacher = _context.Teachers.FirstOrDefault(query);
            return teacher;
        }

        public Teacher GetById(long id)
        {
            var meisters = _context.Teachers.Where(x => x.Id == id);
            return meisters.Count() != 0 ? meisters.First() : null;
        }

        public Teacher Create(Teacher itemToCreate)
        {
            _context.Users.Attach(itemToCreate.MyUser);
            var meister = _context.Teachers.Add(itemToCreate);
            _context.SaveChanges();
            return meister;
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
                    UrlPicture = x.UrlPicture,
                    FullName = x.FullName,
                    BirthDate = x.BirthDate,
                    Nationality = x.Nationality,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    Country = x.Country,
                    Gender = x.Gender,
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
            var meister = GetById(id);
            return new Teacher
            {
                Id = meister.Id,
                IdNumber = meister.IdNumber,
                UrlPicture = meister.UrlPicture,
                FirstName = meister.FirstName,
                LastName = meister.LastName,
                FullName = meister.FullName,
                BirthDate = meister.BirthDate,
                Nationality = meister.Nationality,
                Address = meister.Address,
                City = meister.City,
                State = meister.State,
                Country = meister.Country,
                Gender = meister.Gender,
                Contacts = meister.Contacts,
                StartDate = meister.StartDate,
                EndDate = meister.EndDate,
                Biography = meister.Biography
            };
        }

        public Teacher UpdateTeacherFromMeisterEditModel(Teacher meisterEditModel, Teacher meister)
        {
            meister.FirstName = meisterEditModel.FirstName;
            meister.LastName = meisterEditModel.LastName;
            meister.FullName = (meisterEditModel.FirstName + " " + meisterEditModel.LastName).Trim();
            meister.Country = meisterEditModel.Country;
            meister.IdNumber = meisterEditModel.IdNumber;
            meister.BirthDate =meisterEditModel.BirthDate;
            meister.Gender = meisterEditModel.Gender;
            meister.Nationality = meisterEditModel.Nationality;
            meister.State = meisterEditModel.State;
            meister.City = meisterEditModel.City;
            meister.Address = meisterEditModel.Address;
            meister.Biography = meisterEditModel.Biography;
            meister.StartDate = meisterEditModel.StartDate;
            meister.EndDate = meisterEditModel.EndDate;
            meister.Photo = meisterEditModel.Photo;
            return Update(meister);
        }

        public Teacher GenerateTeacherFromRegisterModel(Teacher meisterRegisterModel)
        {
            return new Teacher
            {
                FirstName = meisterRegisterModel.FirstName,
                LastName = meisterRegisterModel.LastName,
                FullName = (meisterRegisterModel.FirstName + " " + meisterRegisterModel.LastName).Trim(),
                IdNumber = meisterRegisterModel.IdNumber,
                BirthDate = meisterRegisterModel.BirthDate,
                Gender = meisterRegisterModel.Gender,
                Nationality = meisterRegisterModel.Nationality,
                State = meisterRegisterModel.State,
                Country = meisterRegisterModel.Country,
                City = meisterRegisterModel.City,
                Address = meisterRegisterModel.Address,
                Biography = meisterRegisterModel.Biography,
                StartDate = meisterRegisterModel.StartDate,
                EndDate = meisterRegisterModel.EndDate
            };
        }

        public Teacher GetTeacherEditModelById(long id)
        {
            var meister = GetById(id);
            return new Teacher
            {
                FirstName = meister.FirstName,
                LastName = meister.LastName,
                FullName = (meister.FirstName + " " + meister.LastName).Trim(),
                IdNumber = meister.IdNumber,
                BirthDate = meister.BirthDate,
                Gender = meister.Gender,
                Nationality = meister.Nationality,
                Country = meister.Country,
                State = meister.State,
                City = meister.City,
                Address = meister.Address,
                Id = meister.Id,
                StartDate = meister.StartDate,
                EndDate = meister.EndDate,
                Biography = meister.Biography,
                Photo = meister.Photo
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