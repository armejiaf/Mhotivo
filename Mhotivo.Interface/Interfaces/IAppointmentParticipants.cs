using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IAppointmentParticipants
    {
        AppointmentParticipants First(Expression<Func<AppointmentParticipants, bool>> query);
        AppointmentParticipants GetById(long id);
        AppointmentParticipants Create(AppointmentParticipants itemToCreate);
        IQueryable<AppointmentParticipants> Query(Expression<Func<AppointmentParticipants, AppointmentParticipants>> expression);
        IQueryable<AppointmentParticipants> Where(Expression<Func<AppointmentParticipants, bool>> expression);
        IQueryable<AppointmentParticipants> Filter(Expression<Func<AppointmentParticipants, bool>> expression);
        AppointmentParticipants Update(AppointmentParticipants itemToUpdate);
        void Delete(AppointmentParticipants itemToDelete);
        void SaveChanges();
    }
}
