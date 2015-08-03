using System.Collections.Generic;

namespace Mhotivo.Data.Entities
{
    public class Benefactor : People
    {
        public int Capacity { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}