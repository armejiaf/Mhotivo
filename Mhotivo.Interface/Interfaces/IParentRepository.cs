using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IParentRepository
    {
        Parent First(Expression<Func<Parent, Parent>> query);
        Parent GetById(long id);
        Parent GetByIdNumber(string idNumber);
        Parent Create(Parent itemToCreate);
        IQueryable<Parent> Query(Expression<Func<Parent, Parent>> expression);
        IQueryable<Parent> Filter(Expression<Func<Parent, bool>> expression);
        Parent Update(Parent itemToUpdate);
        Parent Delete(long id);
        IEnumerable<Parent> GetAllParents();
        Parent GenerateParentFromRegisterModel(Parent parentRegisterModel);
        Parent GetParentEditModelById(long id);
        Parent GetParentDisplayModelById(long id);
        Parent UpdateParentFromParentEditModel(Parent parentEditModel, Parent parent);
        void SaveChanges();
        bool ExistIdNumber(string idNumber);
    }
}