using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
