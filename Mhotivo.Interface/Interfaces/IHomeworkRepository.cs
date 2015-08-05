using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IHomeworkRepository
    {
        Homework First(Expression<Func<Homework, Homework>> query);
        Homework GetById(long id);
        Homework Create(Homework itemToCreate);
        IQueryable<Homework> Query(Expression<Func<Homework, Homework>> expression);
        IQueryable<Homework> Filter(Expression<Func<Homework, bool>> expression);
        Homework Update(Homework itemToUpdate);
        Homework Delete(long id);
        IEnumerable<Homework> GetAllHomeworks();
        Homework GetHomeworkEditModelById(long id);
        Homework GetHomeworkDisplayModelById(long id);
        Homework UpdateHomeworkFromHomeworkEditModel(Homework homeworkEditModel, Homework homework);
        void SaveChanges();
        Homework GenerateHomeworkFromRegisterModel(Homework homeworkRegisterModel);
    }
}
