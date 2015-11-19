using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class AcademicCourse
    {
        public AcademicCourse()
        {
            Homeworks = new HashSet<Homework>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual AcademicGrade AcademicGrade { get; set; }
        public virtual Course Course { get; set; }
        public virtual Teacher Teacher { get; set; }
        public TimeSpan Schedule { get; set; }
        public virtual ICollection<Homework> Homeworks { get; set; }
    }
}