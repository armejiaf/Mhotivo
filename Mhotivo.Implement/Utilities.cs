using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mhotivo.Data.Entities;

namespace Mhotivo.Implement
{
    public class Utilities
    {
        public static Gender DefineGender(string gender)
        {
            if (gender == null)
                gender = "";
            return (gender.ToLower().StartsWith("m") ? Gender.Masculino : Gender.Femenino);
        }
    }
}
