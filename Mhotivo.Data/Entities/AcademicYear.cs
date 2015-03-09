using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class AcademicYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Grade Grade { get; set; }

        public DateTime Year { get; set; }
        public string Section { get; set; }

        public bool Approved { get; set; }

        public bool IsActive { get; set; }
    }
}