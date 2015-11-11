using System.Data.Entity;
using Mhotivo.Data.Entities;

namespace Mhotivo.Implement.Context
{
    public class MhotivoContext : DbContext
    {
        public MhotivoContext() : base("MhotivoContext") {}
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<AcademicYearGrade> AcademicYearGrades { get; set; }
        public DbSet<AcademicYearCourse> AcademicYearCourses { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Teacher> Teachers{ get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationComment> NotificationComments { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Pensum> Pensums { get; set; }
        public DbSet<People> Peoples { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
