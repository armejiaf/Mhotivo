using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data.Entities;
using Mhotivo.Logic.ViewMessage;
using Mhotivo.Models;
using Mhotivo.Implement.Context;

namespace Mhotivo.Controllers
{
    public class EventController : Controller
    {
        private readonly IAppointmentDiaryRepository _appointmentDiaryRepository;
        private readonly ISessionManagementRepository _sessionManagement;
        private readonly IUserRepository _userRepository;
        private readonly ViewMessageLogic _viewMessageLogic; //remove if unused
        public MhotivoContext Db = new MhotivoContext(); //dependencies. Get this out of here.

        public EventController(IAppointmentDiaryRepository appointmentDiaryRepository,
            ISessionManagementRepository sessionManagement, IUserRepository userRepository)
        {
            _appointmentDiaryRepository = appointmentDiaryRepository;
            _sessionManagement = sessionManagement;
            _userRepository = userRepository;
            _viewMessageLogic = new ViewMessageLogic(this);
        }

        //
        // GET: /Event/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Agenda()
        {
            string userEmail = _sessionManagement.GetUserLoggedEmail();
            var appoinments = _appointmentDiaryRepository.Where(x => String.Compare(x.Creator.Email, userEmail, StringComparison.Ordinal) == 0 && x.IsApproved);
            return View("Agenda", appoinments); //how is this even compiling!?
        }

        [HttpGet]
        public ActionResult Principal()
        {
            var role = _sessionManagement.GetUserLoggedRole();
            if (String.Compare(role, "Principal", StringComparison.Ordinal)!=0)
            {
                return RedirectToAction("Index");
            }
            var appoinments = _appointmentDiaryRepository.Query(x => x);
            return View("PrincipalIndex", appoinments); //how is this even compiling!?
        }

        
       // [HttpPost]
        public ActionResult Approve(int id)
        {
            var role = _sessionManagement.GetUserLoggedRole();
            if (String.Compare(role, "Principal", StringComparison.Ordinal) != 0)
            {
                return RedirectToAction("Index");
            }
            AppointmentDiary rec = _appointmentDiaryRepository.First(s => s.Id == id);
            if (rec != null)
            {
                rec.IsApproved = !rec.IsApproved;
                _appointmentDiaryRepository.SaveChanges();
            }
            return RedirectToAction("Principal");
        }

        public void UpdateEvent(int id, string newEventStart, string newEventEnd)
        {
            UpdateDiaryEvent(id,"", newEventStart, newEventEnd,"",new User());
        }


        public bool SaveEvent(int id, string title, string newEventDate, string newEventTime, string newEventDuration)
        {
            string userEmail = _sessionManagement.GetUserLoggedEmail();
            User creator = _userRepository.First(x => x.Email == userEmail);
            if (id == 0)
            {
                return CreateNewEvent(title, newEventDate, newEventTime, newEventDuration, creator);
            }
            UpdateDiaryEvent(id, title, newEventDate, newEventTime, newEventDuration,creator);
            return true;
        }

        public JsonResult GetDiarySummary(double start, double end)
        {
            List<DiaryEventModel> apptListForDate = LoadAppointmentSummaryInDateRange(start, end);
            var eventList = from e in apptListForDate
                select new
                       {
                           id = e.Id,
                           title = e.Title,
                           date = e.Date,
                           duration = e.Duration,
                           time = e.Time,                           
                           start = e.StartDateString,
                           end = e.EndDateString,
                           someKey = e.SomeImportantKeyId,
                           allDay = false
                       };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiaryEvents(double start, double end)
        {
            List<DiaryEventModel> apptListForDate = LoadAllAppointmentsInDateRange(start, end);
            var eventList = from e in apptListForDate
                select new
                       {
                           id = e.Id,
                           title = e.Title,
                           fecha =e.Date.ToShortDateString(),
                           duration=e.Duration,
                           time=e.Time,
                           start = e.StartDateString,
                           end = e.EndDateString,
                           color = e.StatusColor,
                           className = e.ClassName,
                           someKey = e.SomeImportantKeyId,
                           allDay = false
                       };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        public List<DiaryEventModel> LoadAllAppointmentsInDateRange(double start, double end)
        {
            DateTime fromDate = ConvertFromUnixTimestamp(start);
            DateTime toDate = ConvertFromUnixTimestamp(end);
            using (_appointmentDiaryRepository)
            {
                var loggedEmail = _sessionManagement.GetUserLoggedEmail();
                IQueryable<AppointmentDiary> rslt =
                    _appointmentDiaryRepository.Where(
                        s =>
                            String.Compare(s.Creator.Email, loggedEmail, StringComparison.Ordinal)==0 && s.DateTimeScheduled >= fromDate &&
                            DbFunctions.AddMinutes(s.DateTimeScheduled, s.AppointmentLength) <= toDate);
                var result = new List<DiaryEventModel>();
                foreach (AppointmentDiary item in rslt)
                {
                    var rec = new DiaryEventModel
                    {
                        Id = item.Id,
                        //"s" is a preset format that outputs as: "2009-02-27T12:12:22"
                        StartDateString = item.DateTimeScheduled.ToString("s"),
                        //field AppointmentLength is in minutes
                        EndDateString = item.DateTimeScheduled.AddMinutes(item.AppointmentLength).ToString("s"),
                        Title = item.Title,
                        Duration = item.AppointmentLength,
                        Date =
                            new DateTime(item.DateTimeScheduled.Year, item.DateTimeScheduled.Month,
                                item.DateTimeScheduled.Day),
                        Time = item.DateTimeScheduled.ToString("HH:MM"),
                        StatusString = Enums.GetName((AppointmentStatus) item.StatusEnum)
                    };
                    rec.StatusColor = Enums.GetEnumDescription<AppointmentStatus>(rec.StatusString);
                    var colorCode = rec.StatusColor.Substring(0, rec.StatusColor.IndexOf(":", StringComparison.Ordinal));
                    rec.ClassName = rec.StatusColor.Substring(rec.StatusColor.IndexOf(":", StringComparison.Ordinal) + 1, 
                        rec.StatusColor.Length - colorCode.Length - 1);
                    rec.StatusColor = colorCode;
                    result.Add(rec);
                }
                return result;
            }
        }


        public List<DiaryEventModel> LoadAppointmentSummaryInDateRange(double start, double end)
        {
            //Test
            //^SUCH SPECIFIC, SO DETAILS, WOW
            DateTime fromDate = ConvertFromUnixTimestamp(start);
            DateTime toDate = ConvertFromUnixTimestamp(end);
            using (_appointmentDiaryRepository)
            {
                var loggedEmail = _sessionManagement.GetUserLoggedEmail();
                IQueryable<AppointmentDiary> rslt =
                    _appointmentDiaryRepository.Where(
                        s =>
                            String.Compare(s.Creator.Email, loggedEmail, StringComparison.Ordinal)==0 &&
                            s.DateTimeScheduled >= fromDate &&
                            DbFunctions.AddMinutes(s.DateTimeScheduled, s.AppointmentLength) <= toDate);

                var result = new List<DiaryEventModel>();
                int i = 0;
                foreach (AppointmentDiary item in rslt)
                {
                    var rec = new DiaryEventModel {Id = i};
                    string stringDate = string.Format("{0:yyyy-MM-dd}", item.DateTimeScheduled);
                    rec.StartDateString = stringDate + "T00:00:00"; //ISO 8601 format
                    rec.EndDateString = stringDate + "T23:59:59";
                    rec.Title = item.Title;
                    rec.Duration = item.AppointmentLength;
                    rec.Date = new DateTime(item.DateTimeScheduled.Year, item.DateTimeScheduled.Month, item.DateTimeScheduled.Day);
                    rec.Time = item.DateTimeScheduled.ToString("HH:MM"); if (item.IsApproved)
                    {
                        result.Add(rec);
                    }
                    i++;
                }
                return result;
            }
        }

        public void UpdateDiaryEvent(int id, string title, string newEventStart, string newEventEnd, string newEventDuration, User creator)
        {
            // EventStart comes ISO 8601 format, eg:  "2000-01-10T10:00:00Z" - need to convert to DateTime
            using (_appointmentDiaryRepository)
            {
                AppointmentDiary rec = _appointmentDiaryRepository.First(s => s.Id == id);
                if (rec != null)
                {
                    rec.Title = title;
                    rec.DateTimeScheduled = DateTime.ParseExact(newEventStart + " " + newEventEnd, "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture);
                    rec.AppointmentLength = Int32.Parse(newEventDuration);
                    rec.Creator = creator;           
                    _appointmentDiaryRepository.SaveChanges();
                }
            }
        }

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public bool CreateNewEvent(string title, string newEventDate, string newEventTime, string newEventDuration,
            User creator)
        {
            try
            {
                var rec = new AppointmentDiary
                {
                    Title = title,
                    DateTimeScheduled = DateTime.ParseExact(newEventDate + " " + newEventTime, "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture),
                    AppointmentLength = Int32.Parse(newEventDuration),
                    Creator = creator
                };
                _appointmentDiaryRepository.Create(rec);
                _appointmentDiaryRepository.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public JsonResult GetGroupsAndEmails(string filter)
        {
            List<string> groups = Db.Groups.Where(x => x.Name.Contains(filter)).Select(x => x.Name).ToList();
            List<string> mails =
                Db.Users.Where(x => x.DisplayName.Contains(filter) || x.Email.Contains(filter))
                    .Select(x => x.Email)
                    .ToList();
            groups = groups.Union(mails).ToList();
            return Json(groups, JsonRequestBehavior.AllowGet);
        }
    }
}