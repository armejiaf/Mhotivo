namespace Mhotivo.Models
{
    public class AcademicGradeDisplayModel
    {
        public long Id { get; set; }
        public string Grade { get; set; }
        public string Section { get; set; }
        public string ActivePensum { get; set; }
        public string SectionTeacher { get; set; }
    }

    public class AcademicGradeRegisterModel
    {
        public long AcademicYear { get; set; }
        public long Grade { get; set; }
        public string Section { get; set; }
        public long ActivePensum { get; set; }
    }

    public class AcademicGradeEditModel
    {
        public long Id { get; set; }
        public long Grade { get; set; }
        public string Section { get; set; }
        public long ActivePensum { get; set; }
        public long SectionTeacher { get; set; }
    }
}