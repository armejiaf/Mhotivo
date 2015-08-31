using System.Collections.Generic;
using System.Web.UI;
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
            context.Roles.AddOrUpdate(new Role { Description = "Administrador", Name = "Administrador" });
            context.Roles.AddOrUpdate(new Role { Description = "Padre", Name = "Padre" });
            context.Roles.AddOrUpdate(new Role { Description = "Maestro", Name = "Maestro" });
            context.SaveChanges();
            var admin = new User
            {
                DisplayName = "Administrador",
                Email = "admin@mhotivo.edu",
                Password = "password",
                IsActive = true,
                Roles = new List<Role> {context.Roles.First()}
            };
            admin.EncryptPassword();
            context.Users.AddOrUpdate(admin);
            context.SaveChanges();
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 1, Description = "General" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 2, Description = "Area" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 3, Description = "Grado" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 4, Description = "Personal" });
            context.SaveChanges();
            DebuggingSeeder(context);
            context.SaveChanges();
        }

        private void DebuggingSeeder(MhotivoContext context)
        {
            var genericTeacher = new User
            {
                DisplayName = "Maestro Generico",
                Email = "teacher@mhotivo.edu",
                Password = "password",
                IsActive = true,
                Roles = new List<Role> { context.Roles.Find(3) }
            };
            genericTeacher.EncryptPassword();
            var genericParent = new User
            {
                DisplayName = "Padre Generico",
                Email = "padre@mhotivo.edu",
                Password = "password",
                IsActive = true,
                Roles = new List<Role> { context.Roles.Find(2) }
            };
            genericParent.EncryptPassword();
            context.Users.AddOrUpdate(genericTeacher);
            context.Users.AddOrUpdate(genericParent);
            context.SaveChanges();
            var maestroDefault = context.Meisters.FirstOrDefault(x => x.FullName == "Maestro Generico");
            if (maestroDefault == null)
            {
                context.Meisters.AddOrUpdate(new Teacher { IdNumber = "0000000000000", FirstName = "Maestro", LastName = "Generico", FullName = "Maestro Generico", Disable = false, Gender = true, MyUser = genericTeacher });
            }
            var padreDefault = context.Parents.FirstOrDefault(x => x.FullName == "Padre Generico");
            if (padreDefault == null)
            {
                context.Parents.AddOrUpdate(new Parent { IdNumber = "1234567890", FirstName = "Padre", LastName = "Generico", FullName = "Padre Generico", Disable = false, MyUser = genericParent});
            }
            context.SaveChanges();
        }
    }
}
