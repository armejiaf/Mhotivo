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
        ICollection<Role> GetUserLoggedRoles(int idUser);

        ICollection<Group> GetUserLoggedGroups(int idUser);
        ICollection<People> GetUserLoggedPeoples(int idUser);

        string GetUserLoggedName(int idUser);

        string GetUserLoggedEmail(int idUser);
    }
}
