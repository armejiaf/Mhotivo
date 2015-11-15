namespace Mhotivo.Data.Entities
{
    public class PeopleWithUser : People
    {
        public virtual User User { get; set; }
    }
}
