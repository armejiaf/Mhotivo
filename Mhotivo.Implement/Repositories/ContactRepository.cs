using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class ContactInformationRepository : IContactInformationRepository
    {
        private readonly MhotivoContext _context;

        public ContactInformationRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public ContactInformation GetById(long id)
        {
            return _context.ContactInformations.FirstOrDefault(x => x.Id == id);
        }

        public ContactInformation Create(ContactInformation itemToCreate)
        {
            var contactInformation = _context.ContactInformations.Add(itemToCreate);
            _context.Entry(contactInformation.People).State = EntityState.Modified;
            _context.SaveChanges();
            return contactInformation;
        }

        public IQueryable<ContactInformation> Query(Expression<Func<ContactInformation, ContactInformation>> expression)
        {
            return _context.ContactInformations.Select(expression);
        }

        public IQueryable<ContactInformation> Filter(Expression<Func<ContactInformation, bool>> expression)
        {
            return _context.ContactInformations.Where(expression);
        }

        public ContactInformation Update(ContactInformation itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public ContactInformation Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.ContactInformations.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public ContactInformation Delete(ContactInformation itemToDelete)
        {
            _context.ContactInformations.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }
    }
}