using System.Data.Entity;
using Mhotivo.Data.Entities;


namespace Mhotivo.Implement.Context
{
    public class MhotivoContext : DbContext
    {
        public MhotivoContext() : base("DefaultConnection")
        {
        }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<AcademicYearDetail> AcademicYearDetails { get; set; }
        public DbSet<AppointmentDiary> AppointmentDiary { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Benefactor> Benefactors { get; set; }
        public DbSet<ClassActivityGrading> ClassActivityGradings { get; set; }
        public DbSet<ClassActivity> ClassActivities { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Meister> Meisters { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Pensum> Pensums { get; set; }
        public DbSet<People> Peoples { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRol> UserRoles { get; set; }
        public DbSet<AppointmentParticipants> AppointmentParticipant { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Notification>().
            //  HasMany(c => c.Users).
            //  WithMany(p => p.Notifications).
            //  Map(
            //   m =>
            //   {
            //       m.MapLeftKey("NotificationId"); //Campo asociado con Notifications
            //       m.MapRightKey("UserId"); //Campo asociado con Users
            //       //m.ToTable("UserNotifications");
            //   });
        }
    }
}
