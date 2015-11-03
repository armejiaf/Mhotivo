using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class AcademicYear
    {
        public AcademicYear()
        {
            Grades = new HashSet<AcademicYearGrade>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual ICollection<AcademicYearGrade> Grades { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
    }
}