using System;
using System.Data.Entity;
using System.Linq;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class NotificationTypeRepository:INotificationTypeRepository
    {
        private readonly MhotivoContext _context;

        public NotificationTypeRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Data.Entities.NotificationType First(System.Linq.Expressions.Expression<Func<Data.Entities.NotificationType, bool>> query)
        {
            return _context.NotificationTypes.First(query);
        }

        public Data.Entities.NotificationType GetById(long id)
        {
            return _context.NotificationTypes.FirstOrDefault(x => x.NotificationTypeId == id);
        }

        public Data.Entities.NotificationType Create(Data.Entities.NotificationType itemToCreate)
        {
            return _context.NotificationTypes.Add(itemToCreate);
        }

        public IQueryable<Data.Entities.NotificationType> Query(System.Linq.Expressions.Expression<Func<Data.Entities.NotificationType, Data.Entities.NotificationType>> expression)
        {
            return _context.NotificationTypes.Select(expression);
        }

        public IQueryable<Data.Entities.NotificationType> Filter(System.Linq.Expressions.Expression<Func<Data.Entities.NotificationType, bool>> expression)
        {
            return _context.NotificationTypes.Where(expression);
        }

        public Data.Entities.NotificationType Update(Data.Entities.NotificationType itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            return itemToUpdate;
        }

        public void Delete(Data.Entities.NotificationType itemToDelete)
        {
            _context.NotificationTypes.Remove(itemToDelete);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
