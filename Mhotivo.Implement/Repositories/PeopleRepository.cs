using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using System.Data.Entity;

namespace Mhotivo.Implement.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly MhotivoContext _context;

        public PeopleRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }
        
        public People First(Expression<Func<People, People>> query)
        {
            var peoples = _context.Peoples.Select(query);
            return peoples.Count() != 0 ? peoples.First() : null;
        }

        public People GetById(long id)
        {
            var peoples = _context.Peoples.Where(x => x.Id == id);
            return peoples.Count() != 0 ? peoples.First() : null;
        }

        public People Create(People itemToCreate)
        {
            var people = _context.Peoples.Add(itemToCreate);
            _context.SaveChanges();
                return people;
        }

        public IQueryable<People> Query(Expression<Func<People, People>> expression)
        {
            return _context.Peoples.Select(expression);
        }

        public IQueryable<People> Filter(Expression<Func<People, bool>> expression)
        {
            return _context.Peoples.Where(expression);
        }
            
        public People Update(People itemToUpdate)
        {
            _context.SaveChanges();
            return itemToUpdate;   
        }
        
        public void Detach(People people)
        {
            _context.Entry(people).State = EntityState.Detached;
        }

        public People Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Peoples.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public People GetPeopleDisplayModelById(long id)
        {
            var people = GetById(id);
            return new People
            {
                Id = people.Id,
                IdNumber = people.IdNumber,
                UrlPicture = people.UrlPicture,
                FullName = people.FullName,
                BirthDate = people.BirthDate,
                Nationality = people.Nationality,
                Address = people.Address,
                City = people.City,
                State = people.State,
                Gender = people.Gender
            };
        }

        //TODO: Possibly implement automapper. Too many of these.
        public People UpdatePeopleFromPeopleEditModel(People peopleEditModel, People people) 
        {
            people.FirstName = peopleEditModel.FirstName;
            people.LastName = peopleEditModel.LastName;
            people.FullName = (peopleEditModel.FirstName + " " + peopleEditModel.LastName).Trim();
            people.Country = peopleEditModel.Country;
            people.IdNumber = peopleEditModel.IdNumber;
            people.BirthDate = peopleEditModel.BirthDate;
            people.Gender = peopleEditModel.Gender;
            people.Nationality = peopleEditModel.Nationality;
            people.State = peopleEditModel.State;
            people.City = peopleEditModel.City;
            people.Address = peopleEditModel.Address;
            return Update(people);
        }

        public People GeneratePeopleFromRegisterModel(People peopleRegisterModel)
        {
            return new People
            {
                FirstName = peopleRegisterModel.FirstName,
                LastName = peopleRegisterModel.LastName,
                FullName = (peopleRegisterModel.FirstName + " " + peopleRegisterModel.LastName).Trim(),
                IdNumber = peopleRegisterModel.IdNumber,
                BirthDate = peopleRegisterModel.BirthDate,
                Gender = peopleRegisterModel.Gender,
                Nationality = peopleRegisterModel.Nationality,
                State = peopleRegisterModel.State,
                Country = peopleRegisterModel.Country,
                City = peopleRegisterModel.City,
                Address = peopleRegisterModel.Address
            };
        }

        public People GetPeopleEditModelById(long id)
        {
            var people = GetById(id);
            return new People
            {
                FirstName = people.FirstName,
                LastName = people.LastName,
                FullName = (people.FirstName + " " + people.LastName).Trim(),
                IdNumber = people.IdNumber,
                BirthDate = people.BirthDate,
                Gender = people.Gender, 
                Nationality = people.Nationality,
                Country = people.Country,
                State = people.State,
                City = people.City,
                Address = people.Address,
                Id = people.Id
            };
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<People> GetAllPeople()
        {
            return Query(x => x).ToList();
        }

        public IEnumerable<People> GetAllPeopleByUserId(long userId)
        {
            return Filter(x => x.MyUser.Id == userId).ToList();
        }
    }
}