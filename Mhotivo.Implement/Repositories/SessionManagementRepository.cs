using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class SessionManagementRepository : ISessionManagementRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly IPeopleRepository _peopleRepository;
        private readonly string _userNameIdentifier;
        private readonly string _userRoleIdentifier;
        private readonly string _userEmailIdentifier;
        private readonly string _userIdIdentifier;

        public SessionManagementRepository(IUserRepository userRepository, IPeopleRepository peopleRepository)
        {
            _userRepository = userRepository;
            _peopleRepository = peopleRepository;
            _userNameIdentifier = "loggedUserName";
            _userEmailIdentifier = "loggedUserEmail";
            _userRoleIdentifier = "loggedUserRole";
            _userIdIdentifier = "loggedUserId";
        }

        public bool LogIn(string userEmail, string password, bool remember = false, bool redirect = true)
        {
            User user;
            if (userEmail.Contains("@"))
                user = _userRepository.Filter(x => x.Email.Equals(userEmail)).FirstOrDefault();
            else
            {
                user = _peopleRepository.Filter(x => x.IdNumber.Equals(userEmail)).FirstOrDefault().MyUser;
            }
            if (user == null) return false;
            if (!user.CheckPassword(password)) return false;
            UpdateSessionFromUser(user);
            if (redirect)
            {
               // FormsAuthentication.SetAuthCookie(user.Email, remember);
                FormsAuthentication.RedirectFromLoginPage(user.Id.ToString(CultureInfo.InvariantCulture), remember);
            }
            return true;
        }

        private void UpdateSessionFromUser(User user)
        {
            HttpContext.Current.Session[_userEmailIdentifier] = user.Email;
            HttpContext.Current.Session[_userNameIdentifier] = user.DisplayName;
            HttpContext.Current.Session[_userRoleIdentifier] = _userRepository.GetUserRole(user.Id).ToString("G");
            HttpContext.Current.Session[_userIdIdentifier] = user.Id;
        
        }

        public void LogOut(bool redirect = false)
        {
            HttpContext.Current.Session.Remove(_userEmailIdentifier);
            HttpContext.Current.Session.Remove(_userNameIdentifier);
            HttpContext.Current.Session.Remove(_userRoleIdentifier);
            HttpContext.Current.Session.Remove(_userIdIdentifier);
            FormsAuthentication.SignOut();
            if (redirect) FormsAuthentication.RedirectToLoginPage();
        }

        public string GetUserLoggedName()
        {
            CheckSession();
            var userName = HttpContext.Current.Session[_userNameIdentifier];
            return userName != null ? userName.ToString() : "";
        }

        public string GetUserLoggedEmail()
        {
            CheckSession();
            var userName = HttpContext.Current.Session[_userEmailIdentifier];
            return userName != null ? userName.ToString() : "";
        }

        public string GetUserLoggedRole()
        {
            CheckSession();
            var userRole = HttpContext.Current.Session[_userRoleIdentifier];
            return userRole != null ? userRole.ToString() : "";
        }

        public string GetUserLoggedId()
        {
            CheckSession();
            var userId = HttpContext.Current.Session[_userIdIdentifier];
            return userId != null ? userId.ToString() : "";
        }

        public void CheckSession() //Doesn't implement single responsibility or not named appropriately.
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                FormsAuthentication.RedirectToLoginPage();
            var val = HttpContext.Current.Session[_userIdIdentifier];
            if (val != null)
                if ((long)val > 0) return;
            var id = int.Parse(HttpContext.Current.User.Identity.Name);
            var user = _userRepository.GetById(id);
            UpdateSessionFromUser(user);
        }
    }
}
