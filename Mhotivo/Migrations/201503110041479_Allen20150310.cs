namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Allen20150310 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserNotifications", newName: "NotificationUsers");
            DropForeignKey("dbo.Users", "Role_Id", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "Role_Id" });
            RenameColumn(table: "dbo.People", name: "StartDate", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.People", name: "Biography", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.People", name: "Biography1", newName: "Biography");
            RenameColumn(table: "dbo.People", name: "StartDate1", newName: "StartDate");
            RenameColumn(table: "dbo.People", name: "__mig_tmp__0", newName: "StartDate1");
            RenameColumn(table: "dbo.People", name: "__mig_tmp__1", newName: "Biography1");
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
            
            AddColumn("dbo.AcademicYears", "Section", c => c.String());
            AddColumn("dbo.Notifications", "StudentId", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "Approved", c => c.Boolean(nullable: false));
            AddColumn("dbo.People", "User_Id", c => c.Int());
            CreateIndex("dbo.People", "User_Id");
            AddForeignKey("dbo.People", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.Users", "Role_Id");
            DropColumn("dbo.People", "Photo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "Photo", c => c.Binary());
            AddColumn("dbo.Users", "Role_Id", c => c.Int());
            DropForeignKey("dbo.UserRols", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserRols", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.Homework", "AcademicYearDetail_Id", "dbo.AcademicYearDetails");
            DropForeignKey("dbo.AcademicYearDetails", "Teacher_Id", "dbo.People");
            DropForeignKey("dbo.People", "User_Id", "dbo.Users");
            DropForeignKey("dbo.NotificationComments", "Notification_Id", "dbo.Notifications");
            DropForeignKey("dbo.NotificationComments", "Parent_Id", "dbo.People");
            DropForeignKey("dbo.AcademicYearDetails", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.AcademicYearDetails", "AcademicYear_Id", "dbo.AcademicYears");
            DropIndex("dbo.UserRols", new[] { "User_Id" });
            DropIndex("dbo.UserRols", new[] { "Role_Id" });
            DropIndex("dbo.Homework", new[] { "AcademicYearDetail_Id" });
            DropIndex("dbo.NotificationComments", new[] { "Notification_Id" });
            DropIndex("dbo.NotificationComments", new[] { "Parent_Id" });
            DropIndex("dbo.People", new[] { "User_Id" });
            DropIndex("dbo.AcademicYearDetails", new[] { "Teacher_Id" });
            DropIndex("dbo.AcademicYearDetails", new[] { "Course_Id" });
            DropIndex("dbo.AcademicYearDetails", new[] { "AcademicYear_Id" });
            DropColumn("dbo.People", "User_Id");
            DropColumn("dbo.Notifications", "Approved");
            DropColumn("dbo.Notifications", "StudentId");
            DropColumn("dbo.AcademicYears", "Section");
            DropTable("dbo.UserRols");
            DropTable("dbo.Homework");
            DropTable("dbo.NotificationComments");
            DropTable("dbo.AcademicYearDetails");
            RenameColumn(table: "dbo.People", name: "Biography1", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.People", name: "StartDate1", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.People", name: "StartDate", newName: "StartDate1");
            RenameColumn(table: "dbo.People", name: "Biography", newName: "Biography1");
            RenameColumn(table: "dbo.People", name: "__mig_tmp__1", newName: "Biography");
            RenameColumn(table: "dbo.People", name: "__mig_tmp__0", newName: "StartDate");
            CreateIndex("dbo.Users", "Role_Id");
            AddForeignKey("dbo.Users", "Role_Id", "dbo.Roles", "Id");
            RenameTable(name: "dbo.NotificationUsers", newName: "UserNotifications");
        }
    }
}
