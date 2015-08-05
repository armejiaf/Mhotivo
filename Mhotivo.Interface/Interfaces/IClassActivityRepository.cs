using System;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IClassActivityRepository
    {
        ClassActivity First(Expression<Func<ClassActivity, ClassActivity>> query);
        ClassActivity GetById(long id);
        ClassActivity Create(ClassActivity itemToCreate);
        IQueryable<ClassActivity> Query(Expression<Func<ClassActivity, ClassActivity>> expression);
        IQueryable<ClassActivity> Filter(Expression<Func<ClassActivity, bool>> expression);
        ClassActivity Update(ClassActivity itemToUpdate);
        ClassActivity Delete(ClassActivity itemToDelete);
        void SaveChanges();
    }
}