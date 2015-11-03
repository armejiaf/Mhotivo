using System;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IContactInformationRepository 
    {
        ContactInformation GetById(long id);
        ContactInformation Create(ContactInformation itemToCreate);
        IQueryable<ContactInformation> Query(Expression<Func<ContactInformation, ContactInformation>> expression);
        IQueryable<ContactInformation> Filter(Expression<Func<ContactInformation, bool>> expression);
        ContactInformation Update(ContactInformation itemToUpdate);
        ContactInformation Delete(long id);
        ContactInformation Delete(ContactInformation itemToDelete);
    }
}