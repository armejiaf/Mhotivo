using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IBenefactorRepository
    {
        Benefactor First(Expression<Func<Benefactor, Benefactor>> query);
        Benefactor GetById(long id);
        Benefactor Create(Benefactor itemToCreate);
        IQueryable<Benefactor> Query(Expression<Func<Benefactor, Benefactor>> expression);
        IQueryable<Benefactor> Filter(Expression<Func<Benefactor, bool>> expression);
        Benefactor Update(Benefactor itemToUpdate);
        Benefactor Delete(long id);
        IEnumerable<Benefactor> GettAllBenefactors();
        Benefactor GenerateBenefactorFromRegisterModel(Benefactor benefactorRegisterModel);
        Benefactor GetBenefactorEditModelById(long id);
        Benefactor GetBenefactorDisplayModelById(long id);
        Benefactor UpdateBenefactorFromBenefactorEditModel(Benefactor editModel, Benefactor benefactorModel);
        void SaveChanges();
    }
}