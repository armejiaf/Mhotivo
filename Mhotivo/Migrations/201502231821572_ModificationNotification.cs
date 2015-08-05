namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "StudentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "StudentId");
        }
    }
}
