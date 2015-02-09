using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class NotificationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NotificationTypeId { get; set; }

        public string TypeDescription { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

    }
}
