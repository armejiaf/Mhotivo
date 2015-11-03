using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class Pensum
    {
        public Pensum()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}