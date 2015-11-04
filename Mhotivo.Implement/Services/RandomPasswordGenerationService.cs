using System;
using System.Linq;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Implement.Services
{
    public class RandomPasswordGenerationService : IPasswordGenerationService
    {
        private const string ValidCharacters = "abcdefghijkmnoprqstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ1234567890";

        public string GenerateTemporaryPassword()
        {
            var toReturn = "";
            var rnd = new Random();
            for (var i = 0; i < 10; i++)
            {
                toReturn += ValidCharacters.ElementAt(rnd.Next(ValidCharacters.Length - 1));
            }
            return toReturn;
        }
    }
}
