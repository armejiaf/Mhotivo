using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class Pensum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual Course Course { get; set; }
        public virtual Grade Grade { get; set; }
    }
}