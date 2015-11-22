using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    class PeopleWithUserRepository : IPeopleWithUserRepository
    {
        private readonly MhotivoContext _context;

        public PeopleWithUserRepository(MhotivoContext ctx)
        {
            _context = ctx;
        }

        public PeopleWithUser GetById(long id)
        {
            return _context.PeopleWithUsers.FirstOrDefault(x => x.Id == id);
        }

        public PeopleWithUser Create(PeopleWithUser itemToCreate)
        {
            var peopleWithUser = _context.PeopleWithUsers.Add(itemToCreate);
            _context.SaveChanges();
            return peopleWithUser;
        }

        public IQueryable<PeopleWithUser> Query(Expression<Func<PeopleWithUser, PeopleWithUser>> expression)
        {
            return _context.PeopleWithUsers.Select(expression);
        }

        public IQueryable<PeopleWithUser> Filter(Expression<Func<PeopleWithUser, bool>> expression)
        {
            return _context.PeopleWithUsers.Where(expression);
        }

        public People Update(PeopleWithUser itemToUpdate)
        {
            _context.Entry(itemToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return itemToUpdate;
        }

        public People Delete(long id)
        {
            var itemToDelete = GetById(id);
            _context.PeopleWithUsers.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public People Delete(PeopleWithUser itemToDelete)
        {
            _context.PeopleWithUsers.Remove(itemToDelete);
            _context.SaveChanges();
            return itemToDelete;
        }

        public IEnumerable<PeopleWithUser> GetAllPeopleWithUsers()
        {
            return Query(x => x).ToList();
        }
    }
}