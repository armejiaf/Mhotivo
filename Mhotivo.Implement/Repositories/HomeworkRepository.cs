using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mhotivo.Implement.Repositories
{
    public class HomeworkRepository : IHomeworkRepository
    {
        private readonly MhotivoContext _context;

        public HomeworkRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public Homework First(Expression<Func<Homework, Homework>> query)
        {
            IQueryable<Homework> homeworks = _context.Homeworks.Select(query);
            return homeworks.Count() != 0 ? homeworks.First() : null;
        }

        public Homework GetById(long id)
        {
            IQueryable<Homework> homeworks = _context.Homeworks.Where(x => x.Id == id && !false);
            return homeworks.Count() != 0 ? homeworks.First() : null;
        }

        public Homework Create(Homework itemToCreate)
        {
            Homework homework = _context.Homeworks.Add(itemToCreate);
            _context.SaveChanges();
            return homework;
        }

        public IQueryable<Homework> Query(Expression<Func<Homework, Homework>> expression)
        {
            IQueryable<Homework> myHomeworks = _context.Homeworks.Select(expression);
            return myHomeworks;
        }

        public IQueryable<Homework> Filter(Expression<Func<Homework, bool>> expression)
        {
            IQueryable<Homework> myHomeworks = _context.Homeworks.Where(expression);
            return myHomeworks;
        }

        public Homework Update(Homework itemToUpdate)
        {
            _context.SaveChanges();
            return itemToUpdate;
        }

        public Homework Delete(long id)
        {
            Homework itemToDelete = GetById(id);

            _context.Homeworks.Remove(itemToDelete);
            _context.SaveChanges();
            
            return itemToDelete;
        }

        public IEnumerable<Homework> GetAllHomeworks()
        {
            return Query(x => x).Where(x => !false).ToList().Select(x => new Homework
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DeliverDate = x.DeliverDate,
                Points = x.Points,
                AcademicYearDetail = x.AcademicYearDetail
            });
        }

        public Homework GenerateHomeworkFromRegisterModel(Homework homeworkRegisterModel)
        {
            return new Homework
            {
                Id = homeworkRegisterModel.Id,
                Title = homeworkRegisterModel.Title,
                Description = homeworkRegisterModel.Description,
                DeliverDate = homeworkRegisterModel.DeliverDate,
                Points = homeworkRegisterModel.Points,
                AcademicYearDetail = homeworkRegisterModel.AcademicYearDetail
            };
        }

        public Homework GetHomeworkEditModelById(long id)
        {
            Homework homework = GetById(id);
            return new Homework
            {
                Id = homework.Id,
                Title = homework.Title,
                Description = homework.Description,
                DeliverDate = homework.DeliverDate,
                Points = homework.Points,
                AcademicYearDetail = homework.AcademicYearDetail
            };
        }

        public Homework GetHomeworkDisplayModelById(long id)
        {
            Homework homework = GetById(id);
            return new Homework
            {
                Id = homework.Id,
                Title = homework.Title,
                Description = homework.Description,
                DeliverDate = homework.DeliverDate,
                Points = homework.Points,
                AcademicYearDetail = homework.AcademicYearDetail
            };
        }

        public Homework UpdateHomeworkFromHomeworkEditModel(Homework displayHomeworkModel, Homework homework)
        {
            homework.Id = displayHomeworkModel.Id;
            homework.Title = displayHomeworkModel.Title;
            homework.Description = displayHomeworkModel.Description;
            homework.DeliverDate = displayHomeworkModel.DeliverDate;
            homework.Points = displayHomeworkModel.Points;
            homework.AcademicYearDetail = displayHomeworkModel.AcademicYearDetail;

            return Update(homework);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}