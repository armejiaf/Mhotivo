﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class NotificationRepositoryRepository:INotificationRepository
    {
        private readonly MhotivoContext _context;

        public NotificationRepositoryRepository(MhotivoContext ctx)
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
            var notification = _context.Notifications.Add(itemToCreate);
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
                    n => n.Created.Year==currentAcademicYear && n.NotificationType.NotificationTypeId == 1 && n.Approved);

            return generalNotifications;
        }

        public IQueryable<Notification> GetGradeNotifications(int currentAcademicYear, long id)
        {
            var gradeNotifications = _context.Notifications.Where(
                x => x.Created.Year == currentAcademicYear &&
                    x.NotificationType.NotificationTypeId == 3 &&
                      x.Users.FirstOrDefault(u => u.Id == id) != null && x.Approved);

            return gradeNotifications;
        }

        public IQueryable<Notification> GetPersonalNotifications(int currentAcademicYear, long id)
        {

            var personalNotifications = _context.Notifications.Where(
                x => x.Created.Year == currentAcademicYear &&
                    x.NotificationType.NotificationTypeId == 4 &&
                   x.Users.FirstOrDefault(u => u.Id == id) != null && x.Approved);

            return personalNotifications;
        }

        public IQueryable<Notification> GetAreaNotifications(int currentAcademicYear, long id)
        {

            var areaNotifications = _context.Notifications.Where(
                x => x.Created.Year == currentAcademicYear &&
                    x.NotificationType.NotificationTypeId == 2 &&
                       x.Users.FirstOrDefault(u => u.Id == id) != null && x.Approved);

            return areaNotifications;
        }


    }
}
