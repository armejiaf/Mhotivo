namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ok : DbMigration
    {
        public override void Up()
        {
           // DropColumn("dbo.People", "Discriminator");
           // AlterColumn("dbo.People", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "Discriminator", c => c.String());
            RenameColumn(table: "dbo.People", name: "Discriminator", newName: "Discriminator1");
            AddColumn("dbo.People", "Discriminator", c => c.String());
        }
    }
}
