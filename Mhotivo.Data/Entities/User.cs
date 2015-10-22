using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Mhotivo.Data.Entities
{
    //public enum Roles
    //{
    //    Invalid = -1,
    //    Padre = 1,
    //    Maestro = 2,
    //    Director = 3,
    //    Administrador = 4
    //}

    public class Role
    {
        public Role()
        {
            Privileges = new HashSet<Privilege>();
            Users = new HashSet<User>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public virtual ICollection<Privilege> Privileges { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }

    public class Privilege
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrivilegeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }

    public class User
    {
        public User()
        {
            Notifications = new HashSet<Notification>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Salt { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        
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
        public virtual Role Role { get; set; }
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