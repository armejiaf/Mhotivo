using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IMeisterRepository 
    {
        Meister First(Expression<Func<Meister, Meister>> query);
        Meister GetById(long id);
        Meister Create(Meister itemToCreate);
        IQueryable<Meister> Query(Expression<Func<Meister, Meister>> expression);
        IQueryable<Meister> Filter(Expression<Func<Meister, bool>> expression);
        Meister Update(Meister itemToUpdate);
        Meister Delete(long id);
        IEnumerable<Meister> GetAllMeisters();
        Meister GenerateMeisterFromRegisterModel(Meister meisterRegisterModel);
        Meister GetMeisterEditModelById(long id);
        Meister GetMeisterDisplayModelById(long id);
        Meister UpdateMeisterFromMeisterEditModel(Meister meisterEditModel, Meister meister);
        void SaveChanges();
    }
}