using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Mhotivo.Data.Entities
{
    public enum Roles
    {
        Invalid = -1,
        Padre = 1,
        Maestro = 2,
        Director = 3,
        Administrador = 4
    }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Salt { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Parent> Parents { get; set; }
        public virtual Roles Role { get; set; }

        public bool CheckPassword(string password)
        {
            if (String.IsNullOrEmpty(Salt))
                return false;
            var hashtool = SHA512.Create();
            var hashBytes = hashtool.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashString = BitConverter.ToString(hashBytes).Replace("-", "");
            var prePassword = hashtool.ComputeHash(Encoding.UTF8.GetBytes(hashString + Salt));
            var hashedPassword = BitConverter.ToString(prePassword).Replace("-", "");
            return Password.Equals(hashedPassword);
        }

        public void EncryptPassword()
        {
            var hashtool = SHA512.Create();
            if (String.IsNullOrEmpty(Salt))
            {
                var stringSalt = hashtool.ComputeHash(Encoding.UTF8.GetBytes(Email + DisplayName));
                var hashedSalt = BitConverter.ToString(stringSalt).Replace("-", "");
                Salt = hashedSalt;
            }
            var hashBytes = hashtool.ComputeHash(Encoding.UTF8.GetBytes(Password));
            var hashString = BitConverter.ToString(hashBytes).Replace("-", "");
            var prePassword = hashtool.ComputeHash(Encoding.UTF8.GetBytes(hashString + Salt));
            Password = BitConverter.ToString(prePassword).Replace("-", "");
        }
    }
}