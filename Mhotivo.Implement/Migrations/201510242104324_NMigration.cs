namespace Mhotivo.Implement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicYearDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TeacherStartDate = c.DateTime(),
                        TeacherEndDate = c.DateTime(),
                        Schedule = c.DateTime(),
                        Room = c.String(),
                        AcademicYear_Id = c.Long(),
                        Course_Id = c.Long(),
                        Teacher_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYears", t => t.AcademicYear_Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.People", t => t.Teacher_Id)
                .Index(t => t.AcademicYear_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.AcademicYears",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Section = c.String(),
                        Approved = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Grade_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Grades", t => t.Grade_Id)
                .Index(t => t.Grade_Id);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        EducationLevel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Area_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EducationLevels", t => t.Area_Id)
                .Index(t => t.Area_Id);
            
            CreateTable(
                "dbo.EducationLevels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
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
                        BirthDate = c.String(),
                        Nationality = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        Address = c.String(),
                        Photo = c.Binary(),
                        MyGender = c.Int(nullable: false),
                        Disable = c.Boolean(nullable: false),
                        Biography = c.String(),
                        StartDate = c.String(),
                        EndDate = c.String(),
                        BloodType = c.String(),
                        AccountNumber = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Tutor1_Id = c.Long(),
                        Tutor2_Id = c.Long(),
                        MyUser_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Tutor1_Id)
                .ForeignKey("dbo.People", t => t.Tutor2_Id)
                .ForeignKey("dbo.Users", t => t.MyUser_Id)
                .Index(t => t.Tutor1_Id)
                .Index(t => t.Tutor2_Id)
                .Index(t => t.MyUser_Id);
            
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
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(),
                        DisplayName = c.String(),
                        Password = c.String(),
                        DefaultPassword = c.String(),
                        IsUsingDefaultPassword = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Salt = c.String(),
                        Role_RoleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.Role_RoleId)
                .Index(t => t.Role_RoleId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NotificationName = c.String(),
                        IdGradeAreaUserGeneralSelected = c.Long(nullable: false),
                        GradeIdifNotificationTypePersonal = c.Long(nullable: false),
                        SendingEmail = c.Boolean(nullable: false),
                        UserCreatorId = c.Long(nullable: false),
                        UserCreatorName = c.String(),
                        Message = c.String(),
                        Created = c.DateTime(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        Section = c.String(),
                        NotificationCreator_Id = c.Long(),
                        NotificationType_Id = c.Long(),
                        TargetStudent_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.NotificationCreator_Id)
                .ForeignKey("dbo.NotificationTypes", t => t.NotificationType_Id)
                .ForeignKey("dbo.People", t => t.TargetStudent_Id)
                .Index(t => t.NotificationCreator_Id)
                .Index(t => t.NotificationType_Id)
                .Index(t => t.TargetStudent_Id);
            
            CreateTable(
                "dbo.NotificationComments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CommentText = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Parent_Id = c.Long(),
                        Notification_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Parent_Id)
                .ForeignKey("dbo.Notifications", t => t.Notification_Id)
                .Index(t => t.Parent_Id)
                .Index(t => t.Notification_Id);
            
            CreateTable(
                "dbo.NotificationTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        PrivilegeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.PrivilegeId);
            
            CreateTable(
                "dbo.Enrolls",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AcademicYear_Id = c.Long(),
                        Student_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYears", t => t.AcademicYear_Id)
                .ForeignKey("dbo.People", t => t.Student_Id)
                .Index(t => t.AcademicYear_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Homework",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        DeliverDate = c.DateTime(nullable: false),
                        Points = c.Single(nullable: false),
                        AcademicYearDetail_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYearDetails", t => t.AcademicYearDetail_Id)
                .Index(t => t.AcademicYearDetail_Id);
            
            CreateTable(
                "dbo.Pensums",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Course_Id = c.Long(),
                        Grade_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Grades", t => t.Grade_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Grade_Id);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        NotificationId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.NotificationId, t.UserId })
                .ForeignKey("dbo.Notifications", t => t.NotificationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.NotificationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PrivilegeRoles",
                c => new
                    {
                        Privilege_PrivilegeId = c.Int(nullable: false),
                        Role_RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Privilege_PrivilegeId, t.Role_RoleId })
                .ForeignKey("dbo.Privileges", t => t.Privilege_PrivilegeId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_RoleId, cascadeDelete: true)
                .Index(t => t.Privilege_PrivilegeId)
                .Index(t => t.Role_RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pensums", "Grade_Id", "dbo.Grades");
            DropForeignKey("dbo.Pensums", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Homework", "AcademicYearDetail_Id", "dbo.AcademicYearDetails");
            DropForeignKey("dbo.Enrolls", "Student_Id", "dbo.People");
            DropForeignKey("dbo.Enrolls", "AcademicYear_Id", "dbo.AcademicYears");
            DropForeignKey("dbo.AcademicYearDetails", "Teacher_Id", "dbo.People");
            DropForeignKey("dbo.People", "MyUser_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Role_RoleId", "dbo.Roles");
            DropForeignKey("dbo.PrivilegeRoles", "Role_RoleId", "dbo.Roles");
            DropForeignKey("dbo.PrivilegeRoles", "Privilege_PrivilegeId", "dbo.Privileges");
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.Notifications", "TargetStudent_Id", "dbo.People");
            DropForeignKey("dbo.People", "Tutor2_Id", "dbo.People");
            DropForeignKey("dbo.People", "Tutor1_Id", "dbo.People");
            DropForeignKey("dbo.Notifications", "NotificationType_Id", "dbo.NotificationTypes");
            DropForeignKey("dbo.Notifications", "NotificationCreator_Id", "dbo.Users");
            DropForeignKey("dbo.NotificationComments", "Notification_Id", "dbo.Notifications");
            DropForeignKey("dbo.NotificationComments", "Parent_Id", "dbo.People");
            DropForeignKey("dbo.ContactInformations", "People_Id", "dbo.People");
            DropForeignKey("dbo.AcademicYearDetails", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Courses", "Area_Id", "dbo.EducationLevels");
            DropForeignKey("dbo.AcademicYearDetails", "AcademicYear_Id", "dbo.AcademicYears");
            DropForeignKey("dbo.AcademicYears", "Grade_Id", "dbo.Grades");
            DropIndex("dbo.PrivilegeRoles", new[] { "Role_RoleId" });
            DropIndex("dbo.PrivilegeRoles", new[] { "Privilege_PrivilegeId" });
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropIndex("dbo.Pensums", new[] { "Grade_Id" });
            DropIndex("dbo.Pensums", new[] { "Course_Id" });
            DropIndex("dbo.Homework", new[] { "AcademicYearDetail_Id" });
            DropIndex("dbo.Enrolls", new[] { "Student_Id" });
            DropIndex("dbo.Enrolls", new[] { "AcademicYear_Id" });
            DropIndex("dbo.NotificationComments", new[] { "Notification_Id" });
            DropIndex("dbo.NotificationComments", new[] { "Parent_Id" });
            DropIndex("dbo.Notifications", new[] { "TargetStudent_Id" });
            DropIndex("dbo.Notifications", new[] { "NotificationType_Id" });
            DropIndex("dbo.Notifications", new[] { "NotificationCreator_Id" });
            DropIndex("dbo.Users", new[] { "Role_RoleId" });
            DropIndex("dbo.ContactInformations", new[] { "People_Id" });
            DropIndex("dbo.People", new[] { "MyUser_Id" });
            DropIndex("dbo.People", new[] { "Tutor2_Id" });
            DropIndex("dbo.People", new[] { "Tutor1_Id" });
            DropIndex("dbo.Courses", new[] { "Area_Id" });
            DropIndex("dbo.AcademicYears", new[] { "Grade_Id" });
            DropIndex("dbo.AcademicYearDetails", new[] { "Teacher_Id" });
            DropIndex("dbo.AcademicYearDetails", new[] { "Course_Id" });
            DropIndex("dbo.AcademicYearDetails", new[] { "AcademicYear_Id" });
            DropTable("dbo.PrivilegeRoles");
            DropTable("dbo.UserNotifications");
            DropTable("dbo.Pensums");
            DropTable("dbo.Homework");
            DropTable("dbo.Enrolls");
            DropTable("dbo.Privileges");
            DropTable("dbo.Roles");
            DropTable("dbo.NotificationTypes");
            DropTable("dbo.NotificationComments");
            DropTable("dbo.Notifications");
            DropTable("dbo.Users");
            DropTable("dbo.ContactInformations");
            DropTable("dbo.People");
            DropTable("dbo.EducationLevels");
            DropTable("dbo.Courses");
            DropTable("dbo.Grades");
            DropTable("dbo.AcademicYears");
            DropTable("dbo.AcademicYearDetails");
        }
    }
}
