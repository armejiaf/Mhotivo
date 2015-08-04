using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class Benefactor : People
    {
        
        public int Capacity { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}