using System.Collections.Generic;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface INotificationHandlerService
    {
        void SendAllPending();
        void SendNotification(Notification notification);
        List<Notification> GetAllOfTypeAndYear(NotificationType type, long yearId);
        List<Notification> GetAllActiveOfType(NotificationType type);
    }
}