using System.Collections.Generic;

namespace Mhotivo.Data.Entities
{
    public class Student : People
    {
        public Student()
        {
            MyCompletedGrades = new HashSet<Grade>();
        }
        public string BloodType { get; set; }
        public string AccountNumber { get; set; }
        public virtual Parent Tutor1 { get; set; }
        public virtual Parent Tutor2 { get; set; }
        public virtual AcademicYearGrade MyGrade { get; set; }
        public virtual ICollection<Grade> MyCompletedGrades { get; set; }
    }
}