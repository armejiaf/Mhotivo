using System.Collections.Generic;

namespace Mhotivo.Data.Entities
{
    public class AcademicGrade
    {
        public AcademicGrade()
        {
            CoursesDetails = new HashSet<AcademicCourse>();
            Students = new HashSet<Student>();
        }

        public long Id { get; set; }
        public virtual AcademicYear AcademicYear { get; set; }
        public virtual Grade Grade { get; set; }
        public string Section { get; set; }
        public virtual Pensum ActivePensum { get; set; }
        public virtual Teacher SectionTeacher { get; set; }
        public virtual ICollection<AcademicCourse> CoursesDetails { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
