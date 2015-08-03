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
    public class ParentRepository : IParentRepository
    {
        private readonly MhotivoContext _context;

        public ParentRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public MhotivoContext GeContext()
        {
            return _context;
        }
        
        public Parent First(Expression<Func<Parent, Parent>> query)
        {
            var parent = _context.Parents.Select(query).Include(x => x.User);
            return parent.Count() != 0 ? parent.First() : null;
        }

        public Parent GetById(long id)
        {
            var parent = _context.Parents.Where(x => x.Id == id && !x.Disable).Include(x => x.User);
            return parent.Count() != 0 ? parent.First() : null;
        }

        public Parent GetByIdNumber(string idNumber)
        {
            var parent = _context.Parents.Where(x => x.IdNumber == idNumber && !x.Disable).Include(x => x.User);
            return parent.Count() != 0 ? parent.First() : null;
        }

        public Parent Create(Parent itemToCreate)
        {
            itemToCreate.Disable = false;
            _context.Users.Attach(itemToCreate.User);
            var parent = _context.Parents.Add(itemToCreate);
            _context.SaveChanges();
            return parent;
        }

        public IQueryable<Parent> Query(Expression<Func<Parent, Parent>> expression)
        {
            var myParents = _context.Parents.Select(expression).Include(x => x.User);
            return myParents;
        }

        public IQueryable<Parent> Filter(Expression<Func<Parent, bool>> expression)
        {
            var myParents = _context.Parents.Where(expression).Include(x => x.User);
            return myParents;
        }

        public Parent Update(Parent itemToUpdate)
        {
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Parent Delete(long id)
        {
            var itemToDelete = GetById(id);
            itemToDelete.Disable = true;
            _context.SaveChanges();
            return itemToDelete;
        }
        //TODO: AUTOMAPPER.
        public IEnumerable<Parent> GetAllParents()
        {
            return Query(x => x).Where(x => !x.Disable).ToList().Select(x => new Parent
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
                Photo = x.Photo,
                User = x.User
            });
        }

        public Parent GetParentDisplayModelById(long id)
        {
            var parent = GetById(id);
            return new Parent
            {
                Id = parent.Id,
                IdNumber = parent.IdNumber,
                UrlPicture = parent.UrlPicture,
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                FullName = parent.FullName,
                BirthDate = parent.BirthDate,
                Nationality = parent.Nationality,
                Address = parent.Address,
                City = parent.City,
                State = parent.State,
                Country = parent.Country,
                Gender = parent.Gender,
                Contacts = parent.Contacts,
                Photo = parent.Photo
            };
        }

        public Parent UpdateParentFromParentEditModel(Parent parentEditModel, Parent parent)
        {
            parent.FirstName = parentEditModel.FirstName;
            parent.LastName = parentEditModel.LastName;
            parent.FullName = (parentEditModel.FirstName + " " + parentEditModel.LastName).Trim();
            parent.Country = parentEditModel.Country;
            parent.IdNumber = parentEditModel.IdNumber;
            parent.BirthDate = parentEditModel.BirthDate;
            parent.Gender = parentEditModel.Gender;
            parent.Nationality = parentEditModel.Nationality;
            parent.State = parentEditModel.State;
            parent.City = parentEditModel.City;
            parent.Address = parentEditModel.Address;
            parent.Photo = parentEditModel.Photo;
            return Update(parent);
        }

        public Parent GenerateParentFromRegisterModel(Parent parentRegisterModel)
        {
            return new Parent
            {
                FirstName = parentRegisterModel.FirstName,
                LastName = parentRegisterModel.LastName,
                FullName = (parentRegisterModel.FirstName + " " + parentRegisterModel.LastName).Trim(),
                IdNumber = parentRegisterModel.IdNumber,
                BirthDate = parentRegisterModel.BirthDate,
                Gender = parentRegisterModel.Gender,
                Nationality = parentRegisterModel.Nationality,
                State = parentRegisterModel.State,
                Country = parentRegisterModel.Country,
                City = parentRegisterModel.City,
                Address = parentRegisterModel.Address,
                Photo = parentRegisterModel.Photo,
                User = parentRegisterModel.User
            };
        }

        public Parent GetParentEditModelById(long id)
        {
            var parent = GetById(id);
            return new Parent
            {
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                FullName = (parent.FirstName + " " + parent.LastName).Trim(),
                IdNumber = parent.IdNumber,
                BirthDate = parent.BirthDate,
                Gender = parent.Gender,
                Nationality = parent.Nationality,
                Country = parent.Country,
                State = parent.State,
                City = parent.City,
                Address = parent.Address,
                Id = parent.Id,
                Photo = parent.Photo,
                User = parent.User
            };
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public bool ExistIdNumber(string idNumber)
        {
            var parentWithIdNumber = _context.Parents.Where(x => x.IdNumber.Equals(idNumber));
            if (parentWithIdNumber.Any())
                return true;
            return false;
        }

        internal void Detach(Parent parent)
        {
            _context.Entry(parent).State = EntityState.Detached;
        }
    }
}
