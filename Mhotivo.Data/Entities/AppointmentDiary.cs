using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class AppointmentDiary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime DateTimeScheduled { get; set; }
        public int StatusEnum { get; set; }
        public int AppointmentLength { get; set; }
        public User Creator { get; set; }
        public bool IsApproved { get; set; }
    }
}