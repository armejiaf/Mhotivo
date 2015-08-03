using Mhotivo.Data.Entities;

namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Mhotivo.Implement.Context.MhotivoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Mhotivo.Implement.Context.MhotivoContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Roles.AddOrUpdate(new Role { Id = 1, Description = "Admin", Name = "Admin" });
            context.Roles.AddOrUpdate(new Role { Id = 2, Description = "Padres", Name = "Padres" });
            context.Roles.AddOrUpdate(new Role { Id = 3, Description = "Maestro", Name = "Maestro" });
            context.SaveChanges();

            context.Users.AddOrUpdate(new User { Id = 1, DisplayName = "La directora", Email = "holis@holis.com", Password = "holis", Status = true });
            context.Users.AddOrUpdate(new User { Id = 2, DisplayName = "Default", Email = "default@holis.com", Password = "123456", Status = true });
            context.SaveChanges();

            var rol1 = context.Roles.First();
            var rol2 = context.Roles.Find(3);

            var user1 = context.Users.First();
            var user2 = context.Users.Find(2);

            context.UserRoles.AddOrUpdate(new UserRol { Id = 1, Role = rol1, User = user1 });
            context.UserRoles.AddOrUpdate(new UserRol { Id = 2, Role = rol2, User = user2 });
            context.SaveChanges();

            var maestroDefault = context.Meisters.FirstOrDefault(x => x.FirstName == "Default");
            if (maestroDefault == null)
            {
                context.Meisters.AddOrUpdate(new Teacher { Id = 1, IdNumber = "0000000000000", FirstName = "Default", LastName = "Default", FullName = "Default Default", Disable = false, Gender = true, User = user2 });
                context.SaveChanges();
            }

            context.NotificationTypes.AddOrUpdate(new NotificationType { NotificationTypeId = 1, TypeDescription = "General" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { NotificationTypeId = 2, TypeDescription = "Area" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { NotificationTypeId = 3, TypeDescription = "Grado" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { NotificationTypeId = 4, TypeDescription = "Personal" });
            context.SaveChanges();
        }
    }
}
