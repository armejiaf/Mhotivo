using System.Collections.Generic;
using Mhotivo.Data.Entities;

namespace Mhotivo.Interface.Interfaces
{
    public interface ISecurityRepository
    {
        ICollection<Role> GetUserLoggedRoles();
        ICollection<Group> GetUserLoggedGroups();
        ICollection<People> GetUserLoggedPeoples();
        User GetUserLogged();
        string GetUserLoggedName();
        string GetUserLoggedEmail();
    }
}
