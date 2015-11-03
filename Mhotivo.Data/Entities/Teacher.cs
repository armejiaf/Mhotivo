using System.Collections.Generic;

namespace Mhotivo.Data.Entities
{
    public class Teacher : PeopleWithUser
    {
        public Teacher()
        {
            MyCourses = new HashSet<AcademicYearCourse>();
            MyPreviousCourses = new HashSet<AcademicYearCourse>();
        }

        public virtual ICollection<AcademicYearCourse> MyCourses { get; set; }
        public virtual ICollection<AcademicYearCourse> MyPreviousCourses { get; set; }
    }
}