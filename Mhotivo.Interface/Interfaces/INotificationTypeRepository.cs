using System;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface INotificationTypeRepository
    {
        NotificationType First(Expression<Func<NotificationType, bool>> query);
        NotificationType GetById(long id);
        NotificationType Create(NotificationType itemToCreate);
        IQueryable<NotificationType> Query(Expression<Func<NotificationType, NotificationType>> expression);
        IQueryable<NotificationType> Filter(Expression<Func<NotificationType, bool>> expression);
        NotificationType Update(NotificationType itemToUpdate);
        void Delete(NotificationType itemToDelete);
        void SaveChanges();
    }
}
