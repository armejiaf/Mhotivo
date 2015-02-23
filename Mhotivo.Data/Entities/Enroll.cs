using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class Enroll 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual AcademicYear AcademicYear { get; set; }
        public virtual Student Student { get; set; }
        public IEnumerator<object> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}