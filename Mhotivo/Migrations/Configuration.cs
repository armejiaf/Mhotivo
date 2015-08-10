using System.Collections.Generic;
using Mhotivo.Data.Entities;
using System.Data.Entity.Migrations;
using System.Linq;
using Mhotivo.Implement.Context;

namespace Mhotivo.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MhotivoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MhotivoContext context)
        {
            context.Roles.AddOrUpdate(new Role { Description = "Admin", Name = "Admin" });
            context.Roles.AddOrUpdate(new Role { Description = "Padre", Name = "Padre" });
            context.Roles.AddOrUpdate(new Role { Description = "Maestro", Name = "Maestro" });
            context.SaveChanges();
            var admin = new User
            {
                DisplayName = "Administrador",
                Email = "admin@mhotivo.edu",
                Password = "password",
                Status = true,
                Roles = new List<Role> {context.Roles.First()}
            };
            admin.EncryptPassword();
            var genericTeacher = new User
            {
                DisplayName = "Maestro Generico",
                Email = "teacher@mhotivo.edu",
                Password = "password",
                Status = true,
                Roles = new List<Role> {context.Roles.Find(3)}
            };
            genericTeacher.EncryptPassword();
            context.Users.AddOrUpdate(admin);
            context.Users.AddOrUpdate(genericTeacher);
            context.SaveChanges();
            var user = context.Users.Find(2);
            var maestroDefault = context.Meisters.FirstOrDefault(x => x.FirstName == "Maestro Generico");
            if (maestroDefault == null)
            {
                context.Meisters.AddOrUpdate(new Teacher { Id = 1, IdNumber = "0000000000000", FirstName = "Maestro", LastName = "Generico", FullName = "Maestro Generico", Disable = false, Gender = true, User = user });
                context.SaveChanges();
            }
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 1, Description = "General" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 2, Description = "Area" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 3, Description = "Grado" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 4, Description = "Personal" });
            context.SaveChanges();
        }
    }
}
