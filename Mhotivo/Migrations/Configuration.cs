using System.Collections.ObjectModel;
using Mhotivo.Models;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Implement.Repositories;
using Mhotivo.Interface.Interfaces;

namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MhotivoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Mhotivo.Implement.Context.MhotivoContext";
        }

        protected override void Seed(MhotivoContext context)
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

            context.Roles.AddOrUpdate(new Role {Id = 1, Description = "Admin", Name = "Admin"});
            context.Roles.AddOrUpdate(new Role {Id = 2, Description = "Principal", Name = "Principal"});
            context.SaveChanges();
            context.Users.AddOrUpdate(new User
            {
                Id = 1,
                DisplayName = "Alex Fernandez",
                Email = "olorenzo@outlook.com",
                Password = "123",
                Status = true
            });
            context.Users.AddOrUpdate(new User
            {
                Id = 2,
                DisplayName = "Franklin Castellanos",
                Email = "castellarfrank@hotmail.com",
                Password = "siniestro",
                Status = true
            });
            context.Users.AddOrUpdate(new User
            {
                Id = 3,
                DisplayName = "La directora",
                Email = "holis@holis.com",
                Password = "holis",
                Status = true
            });


            context.NotificationTypes.AddOrUpdate(new NotificationType
            {
                NotificationTypeId = 1,
                TypeDescription = "General"
            });
            context.NotificationTypes.AddOrUpdate(new NotificationType
            {
                NotificationTypeId = 2,
                TypeDescription = "Area"
            });
            context.NotificationTypes.AddOrUpdate(new NotificationType
            {
                NotificationTypeId = 3,
                TypeDescription = "Grado"
            });
            context.NotificationTypes.AddOrUpdate(new NotificationType
            {
                NotificationTypeId = 4,
                TypeDescription = "Personal"
            });

            context.SaveChanges();
        }
    }
}
