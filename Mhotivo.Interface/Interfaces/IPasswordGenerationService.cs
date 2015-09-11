using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mhotivo.Interface.Interfaces
{
    public interface IPasswordGenerationService
    {
        string GenerateTemporaryPassword();
        void AddPasswordsToTable(List<string> passwordsList);
    }
}
