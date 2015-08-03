using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mhotivo.Models
{
    public class AddGroup
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        public string Users { get; set; }
    }
}