namespace Mhotivo.Implement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicCourses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Schedule = c.Time(nullable: false, precision: 7),
                        Teacher_Id = c.Long(),
                        AcademicGrade_Id = c.Long(),
                        Course_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Teacher_Id)
                .ForeignKey("dbo.AcademicGrades", t => t.AcademicGrade_Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .Index(t => t.Teacher_Id)
                .Index(t => t.AcademicGrade_Id)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.AcademicGrades",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Section = c.String(),
                        AcademicYear_Id = c.Long(),
                        SectionTeacher_Id = c.Long(),
                        ActivePensum_Id = c.Long(),
                        Grade_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYears", t => t.AcademicYear_Id)
                .ForeignKey("dbo.People", t => t.SectionTeacher_Id)
                .ForeignKey("dbo.Pensums", t => t.ActivePensum_Id)
                .ForeignKey("dbo.Grades", t => t.Grade_Id)
                .Index(t => t.AcademicYear_Id)
                .Index(t => t.SectionTeacher_Id)
                .Index(t => t.ActivePensum_Id)
                .Index(t => t.Grade_Id);
            
            CreateTable(
                "dbo.AcademicYears",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pensums",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Grade_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Grades", t => t.Grade_Id)
                .Index(t => t.Grade_Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Pensum_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pensums", t => t.Pensum_Id)
                .Index(t => t.Pensum_Id);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Student_Id = c.Long(),
                        EducationLevel_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Student_Id)
                .ForeignKey("dbo.EducationLevels", t => t.EducationLevel_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.EducationLevel_Id);
            
            CreateTable(
                "dbo.EducationLevels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Director_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Director_Id)
                .Index(t => t.Director_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        DefaultPassword = c.String(),
                        IsUsingDefaultPassword = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Salt = c.String(),
                        Role_Id = c.Long(),
                        UserOwner_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .ForeignKey("dbo.People", t => t.UserOwner_Id)
                .Index(t => t.Role_Id)
                .Index(t => t.UserOwner_Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Message = c.String(),
                        NotificationType = c.Int(nullable: false),
                        DestinationId = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        Sent = c.Boolean(nullable: false),
                        SendEmail = c.Boolean(nullable: false),
                        AcademicYear_Id = c.Long(),
                        NotificationCreator_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYears", t => t.AcademicYear_Id)
                .ForeignKey("dbo.Users", t => t.NotificationCreator_Id)
                .Index(t => t.AcademicYear_Id)
                .Index(t => t.NotificationCreator_Id);
            
            CreateTable(
                "dbo.NotificationComments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CommentText = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Commenter_Id = c.Long(),
                        Notification_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Commenter_Id)
                .ForeignKey("dbo.Notifications", t => t.Notification_Id)
                .Index(t => t.Commenter_Id)
                .Index(t => t.Notification_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdNumber = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        FullName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Nationality = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        Address = c.String(),
                        Photo = c.Binary(),
                        MyGender = c.Int(nullable: false),
                        Disable = c.Boolean(nullable: false),
                        BloodType = c.String(),
                        AccountNumber = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        User_Id = c.Long(),
                        MyGrade_Id = c.Long(),
                        Tutor1_Id = c.Long(),
                        Tutor2_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.AcademicGrades", t => t.MyGrade_Id)
                .ForeignKey("dbo.People", t => t.Tutor1_Id)
                .ForeignKey("dbo.People", t => t.Tutor2_Id)
                .Index(t => t.User_Id)
                .Index(t => t.MyGrade_Id)
                .Index(t => t.Tutor1_Id)
                .Index(t => t.Tutor2_Id);
            
            CreateTable(
                "dbo.ContactInformations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.String(),
                        Value = c.String(),
                        People_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.People_Id)
                .Index(t => t.People_Id);
            
            CreateTable(
                "dbo.Homework",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        DeliverDate = c.DateTime(nullable: false),
                        Points = c.Single(nullable: false),
                        AcademicCourse_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicCourses", t => t.AcademicCourse_Id)
                .Index(t => t.AcademicCourse_Id);
            
            CreateTable(
                "dbo.Enrolls",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AcademicGrade_Id = c.Long(),
                        Student_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicGrades", t => t.AcademicGrade_Id)
                .ForeignKey("dbo.People", t => t.Student_Id)
                .Index(t => t.AcademicGrade_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.PrivilegeRoles",
                c => new
                    {
                        Privilege_Id = c.Long(nullable: false),
                        Role_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Privilege_Id, t.Role_Id })
                .ForeignKey("dbo.Privileges", t => t.Privilege_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.Privilege_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Enrolls", "Student_Id", "dbo.People");
            DropForeignKey("dbo.Enrolls", "AcademicGrade_Id", "dbo.AcademicGrades");
            DropForeignKey("dbo.Homework", "AcademicCourse_Id", "dbo.AcademicCourses");
            DropForeignKey("dbo.AcademicCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.AcademicGrades", "Grade_Id", "dbo.Grades");
            DropForeignKey("dbo.AcademicCourses", "AcademicGrade_Id", "dbo.AcademicGrades");
            DropForeignKey("dbo.AcademicGrades", "ActivePensum_Id", "dbo.Pensums");
            DropForeignKey("dbo.Pensums", "Grade_Id", "dbo.Grades");
            DropForeignKey("dbo.Grades", "EducationLevel_Id", "dbo.EducationLevels");
            DropForeignKey("dbo.EducationLevels", "Director_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "UserOwner_Id", "dbo.People");
            DropForeignKey("dbo.People", "Tutor2_Id", "dbo.People");
            DropForeignKey("dbo.People", "Tutor1_Id", "dbo.People");
            DropForeignKey("dbo.People", "MyGrade_Id", "dbo.AcademicGrades");
            DropForeignKey("dbo.Grades", "Student_Id", "dbo.People");
            DropForeignKey("dbo.AcademicGrades", "SectionTeacher_Id", "dbo.People");
            DropForeignKey("dbo.AcademicCourses", "Teacher_Id", "dbo.People");
            DropForeignKey("dbo.People", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ContactInformations", "People_Id", "dbo.People");
            DropForeignKey("dbo.Users", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PrivilegeRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PrivilegeRoles", "Privilege_Id", "dbo.Privileges");
            DropForeignKey("dbo.Notifications", "NotificationCreator_Id", "dbo.Users");
            DropForeignKey("dbo.NotificationComments", "Notification_Id", "dbo.Notifications");
            DropForeignKey("dbo.NotificationComments", "Commenter_Id", "dbo.Users");
            DropForeignKey("dbo.Notifications", "AcademicYear_Id", "dbo.AcademicYears");
            DropForeignKey("dbo.Courses", "Pensum_Id", "dbo.Pensums");
            DropForeignKey("dbo.AcademicGrades", "AcademicYear_Id", "dbo.AcademicYears");
            DropIndex("dbo.PrivilegeRoles", new[] { "Role_Id" });
            DropIndex("dbo.PrivilegeRoles", new[] { "Privilege_Id" });
            DropIndex("dbo.Enrolls", new[] { "Student_Id" });
            DropIndex("dbo.Enrolls", new[] { "AcademicGrade_Id" });
            DropIndex("dbo.Homework", new[] { "AcademicCourse_Id" });
            DropIndex("dbo.ContactInformations", new[] { "People_Id" });
            DropIndex("dbo.People", new[] { "Tutor2_Id" });
            DropIndex("dbo.People", new[] { "Tutor1_Id" });
            DropIndex("dbo.People", new[] { "MyGrade_Id" });
            DropIndex("dbo.People", new[] { "User_Id" });
            DropIndex("dbo.NotificationComments", new[] { "Notification_Id" });
            DropIndex("dbo.NotificationComments", new[] { "Commenter_Id" });
            DropIndex("dbo.Notifications", new[] { "NotificationCreator_Id" });
            DropIndex("dbo.Notifications", new[] { "AcademicYear_Id" });
            DropIndex("dbo.Users", new[] { "UserOwner_Id" });
            DropIndex("dbo.Users", new[] { "Role_Id" });
            DropIndex("dbo.EducationLevels", new[] { "Director_Id" });
            DropIndex("dbo.Grades", new[] { "EducationLevel_Id" });
            DropIndex("dbo.Grades", new[] { "Student_Id" });
            DropIndex("dbo.Courses", new[] { "Pensum_Id" });
            DropIndex("dbo.Pensums", new[] { "Grade_Id" });
            DropIndex("dbo.AcademicGrades", new[] { "Grade_Id" });
            DropIndex("dbo.AcademicGrades", new[] { "ActivePensum_Id" });
            DropIndex("dbo.AcademicGrades", new[] { "SectionTeacher_Id" });
            DropIndex("dbo.AcademicGrades", new[] { "AcademicYear_Id" });
            DropIndex("dbo.AcademicCourses", new[] { "Course_Id" });
            DropIndex("dbo.AcademicCourses", new[] { "AcademicGrade_Id" });
            DropIndex("dbo.AcademicCourses", new[] { "Teacher_Id" });
            DropTable("dbo.PrivilegeRoles");
            DropTable("dbo.Enrolls");
            DropTable("dbo.Homework");
            DropTable("dbo.ContactInformations");
            DropTable("dbo.People");
            DropTable("dbo.Privileges");
            DropTable("dbo.Roles");
            DropTable("dbo.NotificationComments");
            DropTable("dbo.Notifications");
            DropTable("dbo.Users");
            DropTable("dbo.EducationLevels");
            DropTable("dbo.Grades");
            DropTable("dbo.Courses");
            DropTable("dbo.Pensums");
            DropTable("dbo.AcademicYears");
            DropTable("dbo.AcademicGrades");
            DropTable("dbo.AcademicCourses");
        }
    }
}
