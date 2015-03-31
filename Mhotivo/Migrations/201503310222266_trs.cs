namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trs : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.People", name: "Discriminator", newName: "Discriminator1");
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
            
            AddColumn("dbo.People", "UserId_Id", c => c.Int());
            AddColumn("dbo.Notifications", "GradeIdifNotificationTypePersonal", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "StudentId", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "UserCreatorId", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "UserCreatorName", c => c.String());
            AddColumn("dbo.Notifications", "Approved", c => c.Boolean(nullable: false));
            AlterColumn("dbo.People", "Discriminator", c => c.String());
            CreateIndex("dbo.People", "UserId_Id");
            AddForeignKey("dbo.People", "UserId_Id", "dbo.Users", "Id");
            DropColumn("dbo.People", "Photo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "Photo", c => c.Binary());
            DropForeignKey("dbo.People", "UserId_Id", "dbo.Users");
            DropForeignKey("dbo.NotificationComments", "Notification_Id", "dbo.Notifications");
            DropForeignKey("dbo.NotificationComments", "Parent_Id", "dbo.People");
            DropIndex("dbo.NotificationComments", new[] { "Notification_Id" });
            DropIndex("dbo.NotificationComments", new[] { "Parent_Id" });
            DropIndex("dbo.People", new[] { "UserId_Id" });
            AlterColumn("dbo.People", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Notifications", "Approved");
            DropColumn("dbo.Notifications", "UserCreatorName");
            DropColumn("dbo.Notifications", "UserCreatorId");
            DropColumn("dbo.Notifications", "StudentId");
            DropColumn("dbo.Notifications", "GradeIdifNotificationTypePersonal");
            DropColumn("dbo.People", "UserId_Id");
            DropTable("dbo.NotificationComments");
            RenameColumn(table: "dbo.People", name: "Discriminator1", newName: "Discriminator");
        }
    }
}
