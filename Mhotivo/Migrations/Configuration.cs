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

            //context.Roles.AddOrUpdate(new Role {Id = 1, Description = "Admin", Name = "Admin"});
            //context.Roles.AddOrUpdate(new Role {Id = 2, Description = "Principal", Name = "Principal"});
            //context.SaveChanges();
            //context.Users.AddOrUpdate(new User
            //{
            //    Id = 1,
            //    DisplayName = "Alex Fernandez",
            //    Email = "olorenzo@outlook.com",
            //    Password = "123",
            //    Status = true
            //});
            //context.Users.AddOrUpdate(new User
            //{
            //    Id = 2,
            //    DisplayName = "Franklin Castellanos",
            //    Email = "castellarfrank@hotmail.com",
            //    Password = "siniestro",
            //    Status = true
            //});
            //context.Users.AddOrUpdate(new User
            //{
            //    Id = 3,
            //    DisplayName = "La directora",
            //    Email = "holis@holis.com",
            //    Password = "holis",
            //    Status = true
            //});


            //context.NotificationTypes.AddOrUpdate(new NotificationType
            //{
            //    NotificationTypeId = 1,
            //    TypeDescription = "General"
            //});
            //context.NotificationTypes.AddOrUpdate(new NotificationType
            //{
            //    NotificationTypeId = 2,
            //    TypeDescription = "Area"
            //});
            //context.NotificationTypes.AddOrUpdate(new NotificationType
            //{
            //    NotificationTypeId = 3,
            //    TypeDescription = "Grado"
            //});
            //context.NotificationTypes.AddOrUpdate(new NotificationType
            //{
            //    NotificationTypeId = 4,
            //    TypeDescription = "Personal"
            //});

            //context.SaveChanges();

            context.Roles.AddOrUpdate(new Role { Id = 1, Description = "Admin", Name = "Admin" });
            context.Roles.AddOrUpdate(new Role { Id = 2, Description = "Principal", Name = "Principal" });
            context.Roles.AddOrUpdate(new Role { Id = 3, Description = "Padres", Name = "Padres" });
            context.SaveChanges();
            
            context.Users.AddOrUpdate(new User { Id = 1, DisplayName = "Alex Fernandez", Email = "olorenzo@outlook.com", Password = "123", Status = true });
            context.Users.AddOrUpdate(new User { Id = 2, DisplayName = "Franklin Castellanos", Email = "castellarfrank@hotmail.com", Password = "siniestro", Status = true });
            context.Users.AddOrUpdate(new User { Id = 3, DisplayName = "La directora", Email = "holis@holis.com", Password = "holis", Status = true });
            context.SaveChanges();

            var rol1 = context.Roles.First();
            var rol2 = context.Roles.Find(2);

            var user1 = context.Users.First();
            var user2 = context.Users.Find(2);
            var user3 = context.Users.Find(3);

            context.UserRoles.AddOrUpdate(new UserRol { Id = 1, Role = rol2, User = user1 });
            context.UserRoles.AddOrUpdate(new UserRol { Id = 1, Role = rol2, User = user2 });
            context.UserRoles.AddOrUpdate(new UserRol { Id = 1, Role = rol1, User = user3 });
            context.SaveChanges();

            context.Areas.AddOrUpdate(new Area{Id = 1, Name = "Sociales"});
            context.SaveChanges();
            context.Grades.AddOrUpdate(new Grade{Id = 1, EducationLevel = "Primaria", Name = "Primero"});
            context.SaveChanges();

            var area = context.Areas.First();
            context.Courses.AddOrUpdate(new Course { Id = 1, Area = area, Name = "Estudios Sociales" });
            context.SaveChanges();

            var grade1 = context.Grades.First();
            context.AcademicYears.AddOrUpdate(new AcademicYear { Id = 1, Year = DateTime.Now, Approved = true, Grade = grade1, IsActive = true, Section = "A"});
            context.SaveChanges();


        }
    }
}
