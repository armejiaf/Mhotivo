using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mhotivo.Data.Entities;

namespace Mhotivo.Models
{
    public class ClassActivityGrading /*TODO: separate entity from model */
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public virtual ClassActivity ClassActivity { get; set; }
        public virtual Student Student { get; set; }
        public double Score { get; set; }
        public double Percentage { get; set; }
        public string Comments { get; set; }
    }
}