using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class NotificationRepository:INotificationRepository
    {
        private readonly MhotivoContext _context;

        public NotificationRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Notification GetById(long id)
        {
            return _context.Notifications.FirstOrDefault(x => x.Id == id);
        }

        public Notification Create(Notification itemToCreate)
        {
            var notification = _context.Notifications.Add(itemToCreate);
            _context.SaveChanges();
            return notification;
        }

        public IQueryable<Notification> Query(Expression<Func<Notification, Notification>> expression)
        {
            return _context.Notifications.Select(expression);
        }

        public IQueryable<Notification> Filter(Expression<Func<Notification, bool>> expression)
        {
            return _context.Notifications.Where(expression);
        }

        public Notification Update(Notification itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Notification Delete(Notification itemToDelete)
        {
            _context.Notifications.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public Notification Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.Notifications.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<Notification> GetAllNotifications()
        {
            return Query(x => x).ToList();
        }
    }
}
