using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class AppointmentParticipants
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual ICollection<AppointmentDiary> AppointmentDiary { get; set; }
        public long FKUserGroup { get; set; } //Do you mean to use a Group entity?
        public string Type { get; set; }//type Group or Users
    }
}
