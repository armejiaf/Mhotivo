using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface IGradeRepository
    {
        /// <summary>
        /// Create a new grade.
        /// </summary>
        /// <param name="itemToCreate"> contains the information to create the new grade </param>
        /// <returns />
        Grade Create(Grade itemToCreate);

        /// <summary>
        /// Deletes a degree.
        /// </summary>
        /// <param name="id" />
        /// <returns />
        Grade Delete(long id);

        /// <summary>
        /// Returns the information of all degree
        /// </summary>
        /// <returns />
        IEnumerable<Grade> GetAllGrade();

        /// <summary>
        /// Returns the information of a degree
        /// </summary>
        /// <param name="id" />
        /// <returns />
        Grade GetById(long id);

        Grade GetGradeEditModelById(long id);

        Grade GenerateGradeFromRegisterModel(Grade gradeRegisterModel);

        IQueryable<Grade> Query(Expression<Func<Grade, Grade>> expression);

        Grade Update(Grade itemToUpdate);

        Grade UpdateGradeFromGradeEditModel(Grade gradeEditModel, Grade grade);
    }
}