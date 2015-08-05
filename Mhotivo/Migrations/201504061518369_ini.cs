namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ini : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Photo", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Photo");
        }
    }
}
