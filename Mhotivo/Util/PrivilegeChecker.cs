using System;
using System.Collections.Generic;
using System.Linq;
using Mhotivo.Data.Entities;

namespace Mhotivo.Util
{
    public static class PrivilegeChecker
    {
        public static bool HasAnyPrivilege(this Role role, IEnumerable<string> requireAtLeatOnePrivileges)
        {
            return requireAtLeatOnePrivileges.Any(privilegeName => role.Privileges.Any(x => x.Name == privilegeName));
        }

        public static bool HasAllPrivileges(this Role role, IEnumerable<string> requireAtLeatOnePrivileges)
        {
            throw new NotImplementedException();
            return requireAtLeatOnePrivileges.Any(privilegeName => role.Privileges.Any(x => x.Name == privilegeName));
        }

    }
}