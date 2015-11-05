using System.Collections.Generic;

namespace Mhotivo.Data.Entities
{
    public class AcademicYearGrade
    {
        public AcademicYearGrade()
        {
            CoursesDetails = new HashSet<AcademicYearCourse>();
            Students = new HashSet<Student>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public virtual AcademicYear AcademicYear { get; set; }
        public virtual Grade Grade { get; set; }
        public string Section { get; set; }
        public virtual Pensum ActivePensum { get; set; }
        public virtual ICollection<AcademicYearCourse> CoursesDetails { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
