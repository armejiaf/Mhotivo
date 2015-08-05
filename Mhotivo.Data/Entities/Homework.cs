using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class Homework
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime DeliverDate { get; set; }
        public float Points { get; set; }
        public virtual AcademicYearDetail AcademicYearDetail { get; set; }
    }
}