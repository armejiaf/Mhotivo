using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class NotificationCommentRepository : INotificationCommentRepository
    {
        private readonly MhotivoContext _context;

        public NotificationCommentRepository(MhotivoContext context)
        {
            _context = context;
        }

        public NotificationComment GetById(long id)
        {
            return _context.NotificationComments.FirstOrDefault(x => x.Id == id);
        }

        public NotificationComment Delete(NotificationComment itemToDelete)
        {
            _context.NotificationComments.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public NotificationComment Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.NotificationComments.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public NotificationComment Create(NotificationComment itemToCreate)
        {
            var comment = _context.NotificationComments.Add(itemToCreate);
            _context.SaveChanges();
            return comment;
        }

        public NotificationComment Update(NotificationComment itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public IQueryable<NotificationComment> Query(Expression<Func<NotificationComment, NotificationComment>> expression)
        {
            return _context.NotificationComments.Select(expression);
        }

        public IQueryable<NotificationComment> Where(Expression<Func<NotificationComment, bool>> expression)
        {
            return _context.NotificationComments.Where(expression);
        }
    }
}
