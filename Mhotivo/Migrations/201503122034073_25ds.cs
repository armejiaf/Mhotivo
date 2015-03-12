namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _25ds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "NotificationCreator_Id", c => c.Int());
            CreateIndex("dbo.Notifications", "NotificationCreator_Id");
            AddForeignKey("dbo.Notifications", "NotificationCreator_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "NotificationCreator_Id", "dbo.Users");
            DropIndex("dbo.Notifications", new[] { "NotificationCreator_Id" });
            DropColumn("dbo.Notifications", "NotificationCreator_Id");
        }
    }
}
