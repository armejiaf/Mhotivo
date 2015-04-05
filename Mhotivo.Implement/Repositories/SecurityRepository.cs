using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        
        private static string _userNameIdentifier;
        private static string _userRoleIdentifier;
        private static string _userEmailIdentifier;
        private static string _userIdIdentifier;

        public SecurityRepository(MhotivoContext ctx, IUserRepository userRepository, IRoleRepository roleRepository, IPeopleRepository peopleRepository)
        {
            _context = ctx;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _peopleRepository = peopleRepository;

            _userNameIdentifier = "loggedUserName";
            _userEmailIdentifier = "loggedUserEmail";
            _userRoleIdentifier = "loggedUserRole";
            _userIdIdentifier = "loggedUserId";
        }

        public ICollection<Role> GetUserLoggedRoles()
        {
            if (!IsAuthenticated())
                return new List<Role>();

            var idUser = int.Parse(HttpContext.Current.User.Identity.Name);
            
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

        public ICollection<Group> GetUserLoggedGroups()
        {
            if (!IsAuthenticated())
                return new List<Group>();

            var idUser = int.Parse(HttpContext.Current.User.Identity.Name);

            var userTemp = _userRepository.GetById(idUser);

            if (userTemp == null)
                return new Collection<Group>();

            if (userTemp.Groups == null)
                return new Collection<Group>();


            return userTemp.Groups;
        }

        public User GetUserLogged()
        {
            if (!IsAuthenticated())
                return null;

            var idUser = int.Parse(HttpContext.Current.User.Identity.Name);

            return _userRepository.GetById(idUser);
        }

        public ICollection<People> GetUserLoggedPeoples()
        {
            if (!IsAuthenticated())
                return new List<People>();

            var idUser = int.Parse(HttpContext.Current.User.Identity.Name);

            var peopleTemp = _peopleRepository.GetAllPeopleByUserId(idUser).ToList();
            return peopleTemp;
        }

        public string GetUserLoggedName()
        {
            if (!IsAuthenticated())
                return "";

            return HttpContext.Current.Session[_userNameIdentifier].ToString();
        }

        public string GetUserLoggedEmail()
        {
            if (!IsAuthenticated())
                return "";

            return HttpContext.Current.Session[_userEmailIdentifier].ToString();
        }

        private bool IsAuthenticated()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return false;

            var idUser = int.Parse(HttpContext.Current.User.Identity.Name);

            var val = HttpContext.Current.Session[_userIdIdentifier];

            if (val != null) return true;

            var myUser = _userRepository.GetById(idUser);
            HttpContext.Current.Session[_userIdIdentifier] = myUser.Id;
            HttpContext.Current.Session[_userNameIdentifier] = myUser.DisplayName;
            HttpContext.Current.Session[_userEmailIdentifier] = myUser.Email;
            HttpContext.Current.Session[_userRoleIdentifier] = _userRepository.GetUserRoles(idUser).First().Name;
            return true;
        }

    }
}
