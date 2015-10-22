using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class NotificationRepository:INotificationRepository
    {
        private readonly MhotivoContext _context;

        public NotificationRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Notification First(Expression<Func<Notification, bool>> query)
        {
            var notification = _context.Notifications.First(query);
            return notification;
        }

        public Notification GetById(long id)
        {
            var notification = _context.Notifications.Where(x => x.Id == id);
            return notification.Count() != 0 ? notification.FirstOrDefault() : null;
        }

        public Notification Create(Notification itemToCreate)
        {
            foreach (var user in itemToCreate.Users)
            {
                _context.Users.Attach(user);
            }
            var notification = _context.Notifications.Add(itemToCreate);
            _context.SaveChanges();
            return notification;
        }

        public IQueryable<Notification> Query(Expression<Func<Notification, Notification>> expression)
        {
            return _context.Notifications.Select(expression);
        }

        public IQueryable<Notification> Where(Expression<Func<Notification, bool>> expression)
        {
            return _context.Notifications.Where(expression);
        }

        public IQueryable<Notification> Filter(Expression<Func<Notification, bool>> expression)
        {
            return _context.Notifications.Where(expression);
        }

        public Notification Update(Notification itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            return itemToUpdate;
        }

        public void Delete(Notification itemToDelete)
        {
            _context.Notifications.Remove(itemToDelete);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IQueryable<Notification> GetGeneralNotifications(int currentAcademicYear)
        {   
            var generalNotifications = _context.Notifications.Where(
                    n => n.Created.Year==currentAcademicYear && n.NotificationType.Id == 1 && n.Approved);
            return generalNotifications;
        }

        public IEnumerable<Notification> GetGradeNotifications(int currentAcademicYear, long id)
        {

            var students = _context.Students.Where(x => x.Tutor1.Id == id || x.Tutor2.Id == id).ToList();
            var enrolls = new List<Enroll>();
            foreach (var student in students)
            {
                enrolls.AddRange(_context.Enrolls.Where(x => x.Student.Id == student.Id));
            }
            var years = new List<string>();
            foreach (var enroll in enrolls)
            {
                if(enroll.AcademicYear.IsActive)
                    years.Add(enroll.AcademicYear.Section);
               
            }
            
            var gradeNotifications = _context.Notifications.Where(
                x => x.Created.Year == currentAcademicYear &&
                    x.NotificationType.Id == 3 && 
                    years.Contains(x.Section) &&
                        x.Approved ).ToList();
            return gradeNotifications;
        }

        public IEnumerable<Notification> GetPersonalNotifications(int currentAcademicYear, long id)
        {

            var personalNotifications = _context.Notifications.Where(
                x => x.Created.Year == currentAcademicYear &&
                     x.NotificationType.Id == 4 &&
                     x.Approved
                     && (x.TargetStudent.Tutor1.Id == id || x.TargetStudent.Tutor2.Id == id)
                );

            return personalNotifications;
        }

        public IEnumerable<Notification> GetAllNotifications()
        {
            return Query(x => x).ToList().Select(x => new Notification
            {
                Id = x.Id,
                Approved = x.Approved,
                Created = x.Created,
                GradeIdifNotificationTypePersonal = x.GradeIdifNotificationTypePersonal,
                IdGradeAreaUserGeneralSelected = x.IdGradeAreaUserGeneralSelected,
                Message = x.Message,
                NotificationComments = x.NotificationComments,
                NotificationCreator = x.NotificationCreator,
                NotificationName = x.NotificationName,
                NotificationType = x.NotificationType
            });
        }

        public IQueryable<Notification> GetAreaNotifications(int currentAcademicYear, long id)
        {
            var areaNotifications = _context.Notifications.Where(
                x => x.Created.Year == currentAcademicYear &&
                    x.NotificationType.Id == 2 && x.Approved);

            return areaNotifications;
        }
    }
}
