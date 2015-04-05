namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicYearDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeacherStartDate = c.DateTime(),
                        TeacherEndDate = c.DateTime(),
                        Schedule = c.DateTime(),
                        Room = c.String(),
                        AcademicYear_Id = c.Int(),
                        Course_Id = c.Int(),
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
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.DateTime(nullable: false),
                        Section = c.String(),
                        Approved = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Grade_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Grades", t => t.Grade_Id)
                .Index(t => t.Grade_Id);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EducationLevel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Area_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.Area_Id)
                .Index(t => t.Area_Id);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        UrlPicture = c.String(),
                        Photo = c.Binary(),
                        Gender = c.Boolean(nullable: false),
                        Disable = c.Boolean(nullable: false),
                        Biography = c.String(),
                        StartDate = c.String(),
                        EndDate = c.String(),
                        JustARandomColumn = c.String(),
                        Capacity = c.Int(),
                        StartDate1 = c.String(),
                        BloodType = c.String(),
                        AccountNumber = c.String(),
                        Biography1 = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        UserId_Id = c.Int(),
                        User_Id = c.Int(),
                        Benefactor_Id = c.Long(),
                        Tutor1_Id = c.Long(),
                        Tutor2_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.People", t => t.Benefactor_Id)
                .ForeignKey("dbo.People", t => t.Tutor1_Id)
                .ForeignKey("dbo.People", t => t.Tutor2_Id)
                .Index(t => t.UserId_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Benefactor_Id)
                .Index(t => t.Tutor1_Id)
                .Index(t => t.Tutor2_Id);
            
            CreateTable(
                "dbo.ContactInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        DisplayName = c.String(),
                        Password = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NotificationName = c.String(),
                        IdGradeAreaUserGeneralSelected = c.Int(nullable: false),
                        GradeIdifNotificationTypePersonal = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        SendingEmail = c.Boolean(nullable: false),
                        UserCreatorId = c.Int(nullable: false),
                        UserCreatorName = c.String(),
                        Message = c.String(),
                        Created = c.DateTime(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        NotificationCreator_Id = c.Int(),
                        NotificationType_NotificationTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.NotificationCreator_Id)
                .ForeignKey("dbo.NotificationTypes", t => t.NotificationType_NotificationTypeId)
                .Index(t => t.NotificationCreator_Id)
                .Index(t => t.NotificationType_NotificationTypeId);
            
            CreateTable(
                "dbo.NotificationComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        NotificationTypeId = c.Int(nullable: false, identity: true),
                        TypeDescription = c.String(),
                    })
                .PrimaryKey(t => t.NotificationTypeId);
            
            CreateTable(
                "dbo.AppointmentDiaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        DateTimeScheduled = c.DateTime(nullable: false),
                        StatusEnum = c.Int(nullable: false),
                        AppointmentLength = c.Int(nullable: false),
                        IsAproveed = c.Boolean(nullable: false),
                        Creator_Id = c.Int(),
                        AppointmentParticipants_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .ForeignKey("dbo.AppointmentParticipants", t => t.AppointmentParticipants_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.AppointmentParticipants_Id);
            
            CreateTable(
                "dbo.AppointmentParticipants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FKUserGroup = c.Long(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClassActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        Description = c.String(),
                        Value = c.Double(nullable: false),
                        AcademicYear_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYears", t => t.AcademicYear_Id)
                .Index(t => t.AcademicYear_Id);
            
            CreateTable(
                "dbo.ClassActivityGradings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Double(nullable: false),
                        Percentage = c.Double(nullable: false),
                        Comments = c.String(),
                        ClassActivity_Id = c.Int(),
                        Student_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClassActivities", t => t.ClassActivity_Id)
                .ForeignKey("dbo.People", t => t.Student_Id)
                .Index(t => t.ClassActivity_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Enrolls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AcademicYear_Id = c.Int(),
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
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        DeliverDate = c.DateTime(nullable: false),
                        Points = c.Single(nullable: false),
                        AcademicYearDetail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicYearDetails", t => t.AcademicYearDetail_Id)
                .Index(t => t.AcademicYearDetail_Id);
            
            CreateTable(
                "dbo.Pensums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Course_Id = c.Int(),
                        Grade_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Grades", t => t.Grade_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Grade_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRols",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Role_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.GroupUsers",
                c => new
                    {
                        Group_Id = c.Long(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.User_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        NotificationId = c.Long(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NotificationId, t.UserId })
                .ForeignKey("dbo.Notifications", t => t.NotificationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.NotificationId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRols", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserRols", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.Pensums", "Grade_Id", "dbo.Grades");
            DropForeignKey("dbo.Pensums", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Homework", "AcademicYearDetail_Id", "dbo.AcademicYearDetails");
            DropForeignKey("dbo.Enrolls", "Student_Id", "dbo.People");
            DropForeignKey("dbo.Enrolls", "AcademicYear_Id", "dbo.AcademicYears");
            DropForeignKey("dbo.ClassActivityGradings", "Student_Id", "dbo.People");
            DropForeignKey("dbo.ClassActivityGradings", "ClassActivity_Id", "dbo.ClassActivities");
            DropForeignKey("dbo.ClassActivities", "AcademicYear_Id", "dbo.AcademicYears");
            DropForeignKey("dbo.AppointmentDiaries", "AppointmentParticipants_Id", "dbo.AppointmentParticipants");
            DropForeignKey("dbo.AppointmentDiaries", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.AcademicYearDetails", "Teacher_Id", "dbo.People");
            DropForeignKey("dbo.People", "Tutor2_Id", "dbo.People");
            DropForeignKey("dbo.People", "Tutor1_Id", "dbo.People");
            DropForeignKey("dbo.People", "Benefactor_Id", "dbo.People");
            DropForeignKey("dbo.People", "User_Id", "dbo.Users");
            DropForeignKey("dbo.People", "UserId_Id", "dbo.Users");
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.Notifications", "NotificationType_NotificationTypeId", "dbo.NotificationTypes");
            DropForeignKey("dbo.Notifications", "NotificationCreator_Id", "dbo.Users");
            DropForeignKey("dbo.NotificationComments", "Notification_Id", "dbo.Notifications");
            DropForeignKey("dbo.NotificationComments", "Parent_Id", "dbo.People");
            DropForeignKey("dbo.GroupUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.GroupUsers", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.ContactInformations", "People_Id", "dbo.People");
            DropForeignKey("dbo.AcademicYearDetails", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Courses", "Area_Id", "dbo.Areas");
            DropForeignKey("dbo.AcademicYearDetails", "AcademicYear_Id", "dbo.AcademicYears");
            DropForeignKey("dbo.AcademicYears", "Grade_Id", "dbo.Grades");
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropIndex("dbo.GroupUsers", new[] { "User_Id" });
            DropIndex("dbo.GroupUsers", new[] { "Group_Id" });
            DropIndex("dbo.UserRols", new[] { "User_Id" });
            DropIndex("dbo.UserRols", new[] { "Role_Id" });
            DropIndex("dbo.Pensums", new[] { "Grade_Id" });
            DropIndex("dbo.Pensums", new[] { "Course_Id" });
            DropIndex("dbo.Homework", new[] { "AcademicYearDetail_Id" });
            DropIndex("dbo.Enrolls", new[] { "Student_Id" });
            DropIndex("dbo.Enrolls", new[] { "AcademicYear_Id" });
            DropIndex("dbo.ClassActivityGradings", new[] { "Student_Id" });
            DropIndex("dbo.ClassActivityGradings", new[] { "ClassActivity_Id" });
            DropIndex("dbo.ClassActivities", new[] { "AcademicYear_Id" });
            DropIndex("dbo.AppointmentDiaries", new[] { "AppointmentParticipants_Id" });
            DropIndex("dbo.AppointmentDiaries", new[] { "Creator_Id" });
            DropIndex("dbo.NotificationComments", new[] { "Notification_Id" });
            DropIndex("dbo.NotificationComments", new[] { "Parent_Id" });
            DropIndex("dbo.Notifications", new[] { "NotificationType_NotificationTypeId" });
            DropIndex("dbo.Notifications", new[] { "NotificationCreator_Id" });
            DropIndex("dbo.ContactInformations", new[] { "People_Id" });
            DropIndex("dbo.People", new[] { "Tutor2_Id" });
            DropIndex("dbo.People", new[] { "Tutor1_Id" });
            DropIndex("dbo.People", new[] { "Benefactor_Id" });
            DropIndex("dbo.People", new[] { "User_Id" });
            DropIndex("dbo.People", new[] { "UserId_Id" });
            DropIndex("dbo.Courses", new[] { "Area_Id" });
            DropIndex("dbo.AcademicYears", new[] { "Grade_Id" });
            DropIndex("dbo.AcademicYearDetails", new[] { "Teacher_Id" });
            DropIndex("dbo.AcademicYearDetails", new[] { "Course_Id" });
            DropIndex("dbo.AcademicYearDetails", new[] { "AcademicYear_Id" });
            DropTable("dbo.UserNotifications");
            DropTable("dbo.GroupUsers");
            DropTable("dbo.UserRols");
            DropTable("dbo.Roles");
            DropTable("dbo.Pensums");
            DropTable("dbo.Homework");
            DropTable("dbo.Enrolls");
            DropTable("dbo.ClassActivityGradings");
            DropTable("dbo.ClassActivities");
            DropTable("dbo.AppointmentParticipants");
            DropTable("dbo.AppointmentDiaries");
            DropTable("dbo.NotificationTypes");
            DropTable("dbo.NotificationComments");
            DropTable("dbo.Notifications");
            DropTable("dbo.Groups");
            DropTable("dbo.Users");
            DropTable("dbo.ContactInformations");
            DropTable("dbo.People");
            DropTable("dbo.Areas");
            DropTable("dbo.Courses");
            DropTable("dbo.Grades");
            DropTable("dbo.AcademicYears");
            DropTable("dbo.AcademicYearDetails");
        }
    }
}
