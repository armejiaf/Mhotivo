using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IPeopleRepository : IDisposable
    {
        People First(Expression<Func<People, People>> query);
        People GetById(long id);
        People Create(People itemToCreate);
        IQueryable<People> Query(Expression<Func<People, People>> expression);
        IQueryable<People> Filter(Expression<Func<People, bool>> expression);
        People Update(People itemToUpdate);
        People Delete(long id);
        People GeneratePeopleFromRegisterModel(People peopleRegisterModel);
        People GetPeopleEditModelById(long id);
        People GetPeopleDisplayModelById(long id);
        People UpdatePeopleFromPeopleEditModel(People peopleEditModel, People people);
        void SaveChanges();
        IEnumerable<People> GetAllPeople();
        People GetPeopleUser(long userId);
    }
}