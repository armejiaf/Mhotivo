using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class MeisterRepository : IMeisterRepository
    {
        private readonly MhotivoContext _context;

        public MeisterRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Meister First(Expression<Func<Meister, Meister>> query)
        {
            var meisters = _context.Meisters.Select(query);
            return meisters.Count() != 0 ? meisters.First() : null;
        }

        public Meister GetById(long id)
        {
            var meisters = _context.Meisters.Where(x => x.Id == id);
            return meisters.Count() != 0 ? meisters.First() : null;
        }

        public Meister Create(Meister itemToCreate)
        {
            var meister = _context.Meisters.Add(itemToCreate);
            _context.SaveChanges();
            return meister;
        }

        public IQueryable<Meister> Query(Expression<Func<Meister, Meister>> expression)
        {
            return _context.Meisters.Select(expression);
        }

        public IQueryable<Meister> Filter(Expression<Func<Meister, bool>> expression)
        {
            return _context.Meisters.Where(expression);

        }

        public Meister Update(Meister itemToUpdate)
        {
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Meister Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Meisters.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Meister> GetAllMeisters()
        {
            return Query(x => x).ToList().Select(x => new Meister
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
                    //Sexo = Utilities.GenderToString(x.Gender),
                    Gender = x.Gender,
                    Contacts = x.Contacts,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Biography = x.Biography,
                    Photo = x.Photo
                });
        }

        public Meister GetMeisterDisplayModelById(long id)
        {
            var meister = GetById(id);
            return new Meister
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
                //Sexo = Utilities.GenderToString(meister.Gender),
                Gender = meister.Gender,
                Contacts = meister.Contacts,
                StartDate = meister.StartDate,
                EndDate = meister.EndDate,
                Biography = meister.Biography
            };
        }

        public Meister UpdateMeisterFromMeisterEditModel(Meister meisterEditModel, Meister meister)
        {
            meister.FirstName = meisterEditModel.FirstName;
            meister.LastName = meisterEditModel.LastName;
            meister.FullName = (meisterEditModel.FirstName + " " + meisterEditModel.LastName).Trim();
            meister.Country = meisterEditModel.Country;
            meister.IdNumber = meisterEditModel.IdNumber;
            meister.BirthDate =meisterEditModel.BirthDate;
            //meister.Gender = Utilities.IsMasculino(meisterEditModel.Gender);
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

        public Meister GenerateMeisterFromRegisterModel(Meister meisterRegisterModel)
        {
            return new Meister
            {
                FirstName = meisterRegisterModel.FirstName,
                LastName = meisterRegisterModel.LastName,
                FullName = (meisterRegisterModel.FirstName + " " + meisterRegisterModel.LastName).Trim(),
                IdNumber = meisterRegisterModel.IdNumber,
                BirthDate = meisterRegisterModel.BirthDate,
                //Gender = Utilities.IsMasculino(meisterRegisterModel.Gender),
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

        public Meister GetMeisterEditModelById(long id)
        {
            var meister = GetById(id);
            return new Meister
            {
                FirstName = meister.FirstName,
                LastName = meister.LastName,
                FullName = (meister.FirstName + " " + meister.LastName).Trim(),
                IdNumber = meister.IdNumber,
                BirthDate = meister.BirthDate,
                //Gender = Utilities.GenderToString(meister.Gender),
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

    }
}