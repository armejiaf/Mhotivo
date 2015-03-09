using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mhotivo.Implement
{
    public class Utilities
    {
        public static string GenderToString(bool masculino)
        {
            return masculino ? "Masculino" : "Femenino";
        }

        public static bool IsMasculino(string sex)
        {
            return sex.Equals(sex.Length == 1 ? "M" : "Hombre");
        }

        public static DateTime ConvertStringToDateTime(string date)
        {
            var strDia = date.Substring(0, 2);
            var strMes = date.Substring(3, 2);
            var strAnio = date.Substring(6, 2);

            var anio = DateTime.Now.Year;
            if (int.Parse(strAnio) < 20)
                anio = int.Parse("20" + strAnio);
            else
                anio = int.Parse("19" + strAnio);

            var newDate = new DateTime(anio, int.Parse(strMes), int.Parse(strDia));
            return newDate;
        }
    }
}
