using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Repositories
{
    public class PreloadedPasswordsGenerationService : IPasswordGenerationService
    {
        private readonly MhotivoContext _context;
        private List<PreloadedPassword> _passwords;
        public PreloadedPasswordsGenerationService(MhotivoContext ctx)
        {
            _context = ctx;
            UpdateList();
        }

        public string GenerateTemporaryPassword()
        {
            PreloadedPassword pass;
            if (_context.PreloadedPasswords.Count() != 0)
            {
                var rnd = new Random();
                do
                {
                    var index = rnd.Next(0, _passwords.Count);
                    pass = _passwords.ElementAt(index);
                } while (pass == null);
            }
            else
            {
                pass = new PreloadedPassword{Password = "123456"};
            }
            return pass.Password;
        }

        public void AddPasswordsToTable(List<string> passwordsList)
        {
            foreach (var password in passwordsList)
            {
                _context.PreloadedPasswords.AddOrUpdate(new PreloadedPassword{ Password = password });
            }
            _context.SaveChanges();
            UpdateList();
        }

        private void UpdateList()
        {
            _passwords = _context.PreloadedPasswords.ToList();
        }
    }
}
