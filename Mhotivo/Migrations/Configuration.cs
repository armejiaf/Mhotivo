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
            context.Roles.AddOrUpdate(new Role { Id = 1, Description = "Admin", Name = "Admin" });
            context.Roles.AddOrUpdate(new Role { Id = 2, Description = "Padres", Name = "Padre" });
            context.Roles.AddOrUpdate(new Role { Id = 3, Description = "Maestro", Name = "Maestro" });
            context.SaveChanges();
            context.Users.AddOrUpdate(new User { Id = 1, DisplayName = "Administrador", Email = "admin@mhotivo.edu", Password = "password", Status = true });
            context.Users.AddOrUpdate(new User { Id = 2, DisplayName = "Maestro Generico", Email = "teacher@mhotivo.edu", Password = "password", Status = true });
            context.SaveChanges();
            var rol1 = context.Roles.First();
            var rol2 = context.Roles.Find(3);
            var user1 = context.Users.First();
            var user2 = context.Users.Find(2);
            context.UserRoles.AddOrUpdate(new UserRol { Id = 1, Role = rol1, User = user1 });
            context.UserRoles.AddOrUpdate(new UserRol { Id = 2, Role = rol2, User = user2 });
            context.SaveChanges();
            var maestroDefault = context.Meisters.FirstOrDefault(x => x.FirstName == "Maestro Generico");
            if (maestroDefault == null)
            {
                context.Meisters.AddOrUpdate(new Teacher { Id = 1, IdNumber = "0000000000000", FirstName = "Maestro", LastName = "Generico", FullName = "Maestro Generico", Disable = false, Gender = true, User = user2 });
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
