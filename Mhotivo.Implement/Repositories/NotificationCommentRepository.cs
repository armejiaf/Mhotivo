using System.Data.Entity;
using System.Linq;
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

        public NotificationComments GetById(long id)
        {
            var comment = _context.NotificationComments.Where(x => x.Id == id);
            return comment.Count() != 0 ? comment.FirstOrDefault() : null;
        }

        public void Delete(NotificationComments itemToDelete)
        {
            _context.NotificationComments.Remove(itemToDelete);
            _context.SaveChanges();
        }

        public NotificationComments Create(NotificationComments itemToCreate)
        {
            var comment = _context.NotificationComments.Add(itemToCreate);
            _context.SaveChanges();
            return comment;
        }

        public NotificationComments Update(NotificationComments itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            return itemToUpdate;
        }
    }
}
