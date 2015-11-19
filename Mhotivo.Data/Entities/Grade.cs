using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class Grade
    {
        public Grade()
        {
            Pensums = new HashSet<Pensum>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }
        public virtual ICollection<Pensum> Pensums { get; set; } 
    }
}