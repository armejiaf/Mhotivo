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
        public virtual Tutor Tutor1 { get; set; }
        public virtual Tutor Tutor2 { get; set; }
        public virtual AcademicGrade MyGrade { get; set; }
        public virtual ICollection<Grade> MyCompletedGrades { get; set; }
    }
}