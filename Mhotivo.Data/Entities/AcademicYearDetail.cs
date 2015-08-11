using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class AcademicYearDetail : IEnumerable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual AcademicYear AcademicYear { get; set; }
        public virtual Course Course { get; set; }
        public virtual Teacher Teacher { get; set; }
        public DateTime? TeacherStartDate { get; set; }
        public DateTime? TeacherEndDate { get; set; }
        public DateTime? Schedule { get; set; }
        public string Room { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}