using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface INotificationCommentRepository
    {
        NotificationComments GetById(long id);

        void Delete(NotificationComments itemToDelete);

        NotificationComments Create(NotificationComments itemToCreate);

        NotificationComments Update(NotificationComments itemToUpdate);

    }
}
