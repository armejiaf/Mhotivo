using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class Role
    {
        public Role()
        {
            Privileges = new HashSet<Privilege>();
            Users = new HashSet<User>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public virtual ICollection<Privilege> Privileges { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}