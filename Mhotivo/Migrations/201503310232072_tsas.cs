namespace Mhotivo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tsas : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.People", "Discriminator");
            //RenameColumn(table: "dbo.People", name: "Discriminator1", newName: "Discriminator");
            //AlterColumn("dbo.People", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "Discriminator", c => c.String());
            RenameColumn(table: "dbo.People", name: "Discriminator", newName: "Discriminator1");
            AddColumn("dbo.People", "Discriminator", c => c.String());
        }
    }
}
