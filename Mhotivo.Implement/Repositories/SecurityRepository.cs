using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly MhotivoContext _context;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPeopleRepository _peopleRepository;

        public SecurityRepository(MhotivoContext ctx, IUserRepository userRepository, IRoleRepository roleRepository, IPeopleRepository peopleRepository)
        {
            _context = ctx;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _peopleRepository = peopleRepository;
        }

        public ICollection<Role> GetUserLoggedRoles(int idUser)
        {
            var lstRole = new Collection<Role>();
            var userTemp = _userRepository.GetById(idUser);
            
            if(userTemp == null)
                return lstRole;

            var userroles =
                _context.UserRoles.Where(x => x.User != null && x.Role != null && x.User.Id == idUser)
                    .Select(x => x.Role)
                    .ToList();

            return userroles;
        }

        public ICollection<Group> GetUserLoggedGroups(int idUser)
        {
            var userTemp = _userRepository.GetById(idUser);

            if (userTemp == null)
                return new Collection<Group>();

            if (userTemp.Groups == null)
                return new Collection<Group>();


            return userTemp.Groups;
        }



        public ICollection<People> GetUserLoggedPeoples(int idUser)
        {
            var peopleTemp = _peopleRepository.GetAllPeople().Where(x => x.User.Id == idUser).ToList();

            return peopleTemp;
        }

        public string GetUserLoggedName(int idUser)
        {
            var userTemp = _userRepository.GetById(idUser);

            if (userTemp == null)
                return "";

            return userTemp.DisplayName;
        }

        public string GetUserLoggedEmail(int idUser)
        {
            var userTemp = _userRepository.GetById(idUser);

            if (userTemp == null)
                return "";

            return userTemp.Email;
        }
    }
}
