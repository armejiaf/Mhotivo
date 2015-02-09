using System;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface INotification:IDisposable
    {


        Notification First(Expression<Func<Notification, bool>> query);
        Notification GetById(long id);
        Notification Create(Notification itemToCreate);
        IQueryable<Notification> Query(Expression<Func<Notification, Notification>> expression);
        IQueryable<Notification> Where(Expression<Func<Notification, bool>> expression);
        IQueryable<Notification> Filter(Expression<Func<Notification, bool>> expression);
        Notification Update(Notification itemToUpdate);
        void Delete(Notification itemToDelete);
        void SaveChanges();
    }
}
