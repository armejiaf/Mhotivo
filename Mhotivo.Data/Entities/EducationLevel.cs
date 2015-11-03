using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class EducationLevel
    {
        public EducationLevel()
        {
            Grades = new HashSet<Grade>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual User Director { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
