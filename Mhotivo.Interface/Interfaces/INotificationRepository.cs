using System;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface INotificationRepository
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
        IQueryable<Notification> GetGeneralNotifications(int currentAcademicYear);
        IQueryable<Notification> GetAreaNotifications(int currentAcademicYear, long id);
        IQueryable<Notification> GetGradeNotifications(int currentAcademicYear, long id);
        IQueryable<Notification> GetPersonalNotifications(int currentAcademicYear, long id);
    }
}
