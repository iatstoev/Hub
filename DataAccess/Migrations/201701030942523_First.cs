namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Sections", "Test");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sections", "Test", c => c.Int(nullable: false));
        }
    }
}
