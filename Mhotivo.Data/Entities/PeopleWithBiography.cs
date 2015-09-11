using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mhotivo.Data.Entities
{
    public class PeopleWithBiography : People
    {
        public string Biography { get; set; }
        public string StartDate { get; set; }
    }
}
