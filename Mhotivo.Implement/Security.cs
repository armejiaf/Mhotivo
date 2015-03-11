using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement
{
    public class Security
    {
        private static ISecurityRepository _securityRepository;

        public static void SetSecurityRepository(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        public static ICollection<Role> GetLoggedUserRoles()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return new List<Role>();

            var val = HttpContext.Current.Session["loggedUserId"];
            if (val != null)
                if ((int)val == 0)
                    return new Collection<Role>();

            var id = int.Parse(HttpContext.Current.User.Identity.Name);


            return _securityRepository.GetUserLoggedRoles(id);
        }

        public static ICollection<Group> GetLoggedUserGroups()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return new List<Group>();

            var val = HttpContext.Current.Session["loggedUserId"];
            if (val != null)
                if ((int)val == 0)
                    return new Collection<Group>();

            var id = int.Parse(HttpContext.Current.User.Identity.Name);


            return _securityRepository.GetUserLoggedGroups(id);
        }

        public static ICollection<People> GetLoggedUserPeoples()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return new List<People>();

            var val = HttpContext.Current.Session["loggedUserId"];
            if (val != null)
                if ((int)val == 0)
                    return new Collection<People>();

            var id = int.Parse(HttpContext.Current.User.Identity.Name);

            return _securityRepository.GetUserLoggedPeoples(id);
        }

        public static string GetUserLoggedName()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return "";

            var val = HttpContext.Current.Session["loggedUserId"];
            if (val != null)
                if ((int)val == 0)
                    return "";

            var id = int.Parse(HttpContext.Current.User.Identity.Name);


            return _securityRepository.GetUserLoggedName(id);
        }

        public static string GetUserLoggedEmail()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return "";

            var val = HttpContext.Current.Session["loggedUserId"];
            if (val != null)
                if ((int)val == 0)
                    return "";

            var id = int.Parse(HttpContext.Current.User.Identity.Name);


            return _securityRepository.GetUserLoggedEmail(id);
        }
    }
}