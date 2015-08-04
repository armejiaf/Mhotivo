using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Interface;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Data;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;

namespace Mhotivo.Implement.Repositories
{
    public class AppoimentParticipant:IAppointmentParticipants
    {
        
        private readonly MhotivoContext _context;

        public AppoimentParticipant(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public AppointmentParticipants First(Expression<Func<AppointmentParticipants, bool>> query)
        {
            var appointmentParticipant = _context.AppointmentParticipant.First(query);
            return appointmentParticipant;
        }

        public AppointmentParticipants GetById(long id)
        {
            var appointmentParticipants = _context.AppointmentParticipant.Where(x => x.Id == id);
            return appointmentParticipants.Count() != 0 ? appointmentParticipants.First() : null;
        }

        public AppointmentParticipants Create(AppointmentParticipants itemToCreate)
        {
            var appointmentParticipants = _context.AppointmentParticipant.Add(itemToCreate);
            return appointmentParticipants;
        }

        public IQueryable<AppointmentParticipants> Query(Expression<Func<AppointmentParticipants, AppointmentParticipants>> expression)
        {
            return _context.AppointmentParticipant.Select(expression);
        }

        public IQueryable<AppointmentParticipants> Where(Expression<Func<AppointmentParticipants, bool>> expression)
        {
            return _context.AppointmentParticipant.Where(expression);
        }

        public IQueryable<AppointmentParticipants> Filter(Expression<Func<AppointmentParticipants, bool>> expression)
        {
            return _context.AppointmentParticipant.Where(expression);
        }

        public AppointmentParticipants Update(AppointmentParticipants itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            return itemToUpdate;
        }

        public void Delete(AppointmentParticipants itemToDelete)
        {
            _context.AppointmentParticipant.Remove(itemToDelete);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
