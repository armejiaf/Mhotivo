using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mhotivo.Controllers
{
    public class DateTimeController : Controller
    {
        public JsonResult GetDaysForMonthAndYear(int month, int year = 2001)
        {
            month = month%12;
            var daysForMonth = new Dictionary<int, int>
            {
                {1, 31},
                {2, 28},
                {3, 31}, 
                {4, 30},
                {5, 31},
                {6, 30},
                {7, 31},
                {8, 31},
                {9, 30},
                {10, 31},
                {11, 30},
                {12, 31}
            };
            var days = new List<int>();
            for (var i = 1; i <= daysForMonth[month]; i++)
            {
                days.Add(i);
            }
            if (year%4 == 0)
            {
                days.Add(days.Last()+1);
            }
            return Json(new SelectList(days), JsonRequestBehavior.AllowGet);
        }
        public static SelectList GetDaysForMonthAndYearStatic(int month, int year = 2001)
        {
            month = month % 12;
            var daysForMonth = new Dictionary<int, int>
            {
                {1, 31},
                {2, 28},
                {3, 31}, 
                {4, 30},
                {5, 31},
                {6, 30},
                {7, 31},
                {8, 31},
                {9, 30},
                {10, 31},
                {11, 30},
                {12, 31}
            };
            var days = new List<int>();
            for (var i = 1; i <= daysForMonth[month]; i++)
            {
                days.Add(i);
            }
            if (year % 4 == 0)
            {
                days.Add(days.Last() + 1);
            }
            return new SelectList(days);
        }
        public static SelectList GetMonths()
        {
            var monthsDictionary = new Dictionary<int, string>
            {
                {1, "Ene"},
                {2, "Feb"},
                {3, "Mar"}, 
                {4, "Abr"},
                {5, "May"},
                {6, "Jun"},
                {7, "Jul"},
                {8, "Ago"},
                {9, "Sept"},
                {10, "Oct"},
                {11, "Nov"},
                {12, "Dic"}
            };
            return new SelectList(monthsDictionary.ToList(), "key", "value");
        }

        public static SelectList GetYears()
        {
            var years = new List<int>();
            for (var i = 1900; i< 2100; i++)
            {
                years.Add(i);
            }
            return new SelectList(years, DateTime.Now.Year);
        }
    }
}