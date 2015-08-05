using System;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IContactInformationRepository 
    {
        ContactInformation First(Expression<Func<ContactInformation, ContactInformation>> query);
        ContactInformation GetById(long id);
        ContactInformation Create(ContactInformation itemToCreate);
        IQueryable<TResult> Query<TResult>(Expression<Func<ContactInformation, TResult>> expression);
        IQueryable<ContactInformation> Filter(Expression<Func<ContactInformation, bool>> expression);
        ContactInformation Update(ContactInformation itemToUpdate);
        ContactInformation Delete(long Id);
        void SaveChanges();
    }
}