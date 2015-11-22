using System.ComponentModel;

namespace Mhotivo.Data.Entities
{
    public enum Parentage
    {
        [Description("Madre")]
        Mother = 1,
        [Description("Padre")]
        Father = 2,
        [Description("Abuelo (a)")]
        Grandfather = 3,
        [Description("Tio (a)")]
        Uncle = 4,
        [Description("Hermano (a)")]
        Brother = 5,
        [Description("Otro")]
        Other = 6
    }

    public class Tutor : PeopleWithUser
    {
        public Parentage Parentage { get; set; }
    }
}