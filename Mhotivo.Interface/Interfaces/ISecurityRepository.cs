using System.Collections.Generic;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface ISecurityRepository
    {
        Roles GetUserLoggedRole();
        ICollection<People> GetUserLoggedPeoples();
        User GetUserLogged();
        string GetUserLoggedName();
        string GetUserLoggedEmail();
    }
}
