using System.Collections.Generic;

namespace Mhotivo.Data.Entities
{
    public class Teacher : PeopleWithUser
    {
        public Teacher()
        {
            MyCourses = new HashSet<AcademicCourse>();
            MySections = new HashSet<AcademicGrade>();
        }

        public virtual ICollection<AcademicCourse> MyCourses { get; set; }
        public virtual ICollection<AcademicGrade> MySections { get; set; }
    }
}