using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mhotivo.Interface.Interfaces
{
    public interface ISessionManagementRepository
    {
        bool LogIn(string userName, string password, bool remember = false);

        void LogOut(bool redirect = false);

        string GetUserLoggedName();

        string GetUserLoggedEmail();

        string GetUserLoggedRole();

    }
}
