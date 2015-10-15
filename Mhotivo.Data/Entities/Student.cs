namespace Mhotivo.Data.Entities
{
    public class Student : PeopleWithBiography
    {
        public string BloodType { get; set; }
        public string AccountNumber { get; set; }
        public virtual Parent Tutor1 { get; set; }
        public virtual Parent Tutor2 { get; set; }
    }
}