using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mhotivo.Data.Entities
{
    public class Parent : People
    {

        public string JustARandomColumn { get; set; }

        public virtual User UserId { get; set; }

    }
}