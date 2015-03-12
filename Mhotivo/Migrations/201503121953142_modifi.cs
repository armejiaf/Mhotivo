namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "NotificationCreator_Id", "dbo.Users");
            DropIndex("dbo.Notifications", new[] { "NotificationCreator_Id" });
            AddColumn("dbo.Users", "Notification_Id", c => c.Long());
            AddColumn("dbo.Notifications", "UserCreatorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "Notification_Id");
            AddForeignKey("dbo.Users", "Notification_Id", "dbo.Notifications", "Id");
            DropColumn("dbo.Notifications", "NotificationCreator_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "NotificationCreator_Id", c => c.Int());
            DropForeignKey("dbo.Users", "Notification_Id", "dbo.Notifications");
            DropIndex("dbo.Users", new[] { "Notification_Id" });
            DropColumn("dbo.Notifications", "UserCreatorId");
            DropColumn("dbo.Users", "Notification_Id");
            CreateIndex("dbo.Notifications", "NotificationCreator_Id");
            AddForeignKey("dbo.Notifications", "NotificationCreator_Id", "dbo.Users", "Id");
        }
    }
}
