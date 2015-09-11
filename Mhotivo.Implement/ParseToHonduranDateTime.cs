using System;
using System.Globalization;

namespace Mhotivo.App_Data
{
    public class ParseToHonduranDateTime
    {
        public static DateTime Parse(string dateToParse)
        {
            DateTime toReturn;
            DateTime.TryParseExact(dateToParse, "dd-MM-yyyy", null,
                DateTimeStyles.None, out toReturn);
            return toReturn;

        }
    }
}