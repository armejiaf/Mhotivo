using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface INotificationCommentRepository
    {
        NotificationComment GetById(long id);
        NotificationComment Delete(NotificationComment itemToDelete);
        NotificationComment Delete(long id);
        NotificationComment Create(NotificationComment itemToCreate);
        NotificationComment Update(NotificationComment itemToUpdate);
        IQueryable<NotificationComment> Query(Expression<Func<NotificationComment, NotificationComment>> expression);
        IQueryable<NotificationComment> Where(Expression<Func<NotificationComment, bool>> expression);
    }
}
