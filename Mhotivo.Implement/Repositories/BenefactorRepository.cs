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
    public class BenefactorRepository : IBenefactorRepository
    {
        private readonly MhotivoContext _context;

        public BenefactorRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Benefactor First(Expression<Func<Benefactor, Benefactor>> query)
        {
            var benefactor = _context.Benefactors.Select(query);
            return benefactor.Count() != 0 ? benefactor.First() : null;
        }

        public Benefactor GetById(long id)
        {
            var benefactor = _context.Benefactors.Where(x => x.Id == id);
            return benefactor.Count() != 0 ? benefactor.First() : null;
        }

        public Benefactor Create(Benefactor itemToCreate)
        {
            var benefactor = _context.Benefactors.Add(itemToCreate);
            _context.SaveChanges();
            return benefactor;
        }

        public IQueryable<Benefactor> Query(Expression<Func<Benefactor, Benefactor>> expression)
        {
            var myBenefactors = _context.Benefactors.Select(expression);
            return myBenefactors;
        }

        public IQueryable<Benefactor> Filter(Expression<Func<Benefactor, bool>> expression)
        {
            var myBenefactors = _context.Benefactors.Where(expression);
            return myBenefactors;
        }

        public Benefactor Update(Benefactor itemToUpdate)
        {
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Benefactor Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Benefactors.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Benefactor> GettAllBenefactors()
        {
            return Query(x => x).ToList().Select(x => new Benefactor
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
                //Gender = Utilities.GenderToString(x.Gender),
                Gender = x.Gender,
                Contacts = x.Contacts,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Capacity = x.Capacity
            });
        }

        public Benefactor GetBenefactorDisplayModelById(long id)
        {
            var benefactor = GetById(id);
            return new Benefactor
            {
                Id = benefactor.Id,
                IdNumber = benefactor.IdNumber,
                UrlPicture = benefactor.UrlPicture,
                FirstName = benefactor.FirstName,
                LastName = benefactor.LastName,
                FullName = benefactor.FullName,
                BirthDate = benefactor.BirthDate,
                Nationality = benefactor.Nationality,
                Address = benefactor.Address,
                City = benefactor.City,
                State = benefactor.State,
                Country = benefactor.Country,
                //Gender = Utilities.GenderToString(benefactor.Gender),
                Gender = benefactor.Gender,
                Contacts = benefactor.Contacts,
                Capacity = benefactor.Capacity,
                //StudentsCount = benefactor.Students.Count,
                Students = benefactor.Students
            };
        }

        public Benefactor UpdateBenefactorFromBenefactorEditModel(Benefactor editModel, Benefactor benefactorModel)
        {
            benefactorModel.FirstName = editModel.FirstName;
            benefactorModel.LastName = editModel.LastName;
            benefactorModel.FullName = (editModel.FirstName + " " + editModel.LastName).Trim();
            benefactorModel.Country = editModel.Country;
            benefactorModel.IdNumber = editModel.IdNumber;
            benefactorModel.BirthDate = editModel.BirthDate;
            //benefactorModel.Gender = Utilities.IsMasculino(editModel.Gender);
            benefactorModel.Gender = editModel.Gender;
            benefactorModel.Nationality = editModel.Nationality;
            benefactorModel.State = editModel.State;
            benefactorModel.City = editModel.City;
            benefactorModel.Address = editModel.Address;
            benefactorModel.Capacity = editModel.Capacity;
            return Update(benefactorModel);
        }

        public Benefactor GenerateBenefactorFromRegisterModel(Benefactor benefactorRegisterModel)
        {
            return new Benefactor
            {
                FirstName = benefactorRegisterModel.FirstName,
                LastName = benefactorRegisterModel.LastName,
                FullName = (benefactorRegisterModel.FirstName + " " + benefactorRegisterModel.LastName).Trim(),
                IdNumber = benefactorRegisterModel.IdNumber,
                BirthDate = benefactorRegisterModel.BirthDate,
                //Gender = Utilities.IsMasculino(benefactorRegisterModel.Gender),
                Gender = benefactorRegisterModel.Gender,
                Nationality = benefactorRegisterModel.Nationality,
                State = benefactorRegisterModel.State,
                Country = benefactorRegisterModel.Country,
                City = benefactorRegisterModel.City,
                Address = benefactorRegisterModel.Address,
                //Capacity = int.Parse(benefactorRegisterModel.Capacity)
                Capacity = benefactorRegisterModel.Capacity
            };
        }

        public Benefactor GetBenefactorEditModelById(long id)
        {
            var benefactor = GetById(id);
            return new Benefactor
            {
                FirstName = benefactor.FirstName,
                LastName = benefactor.LastName,
                FullName = (benefactor.FirstName + " " + benefactor.LastName).Trim(),
                IdNumber = benefactor.IdNumber,
                BirthDate = benefactor.BirthDate,
                //Gender = Utilities.GenderToString(benefactor.Gender),
                Gender = benefactor.Gender,
                Nationality = benefactor.Nationality,
                Country = benefactor.Country,
                State = benefactor.State,
                City = benefactor.City,
                Address = benefactor.Address,
                Id = benefactor.Id,
                //StudentsCount = benefactor.Students.Count
                Students = benefactor.Students,
                Capacity = benefactor.Capacity
            };
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        internal void Detach(Benefactor benefactor)
        {
            _context.Entry(benefactor).State = EntityState.Detached;
        }
    }
}
