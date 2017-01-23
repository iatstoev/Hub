namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentSectionID = c.Int(),
                        HtmlContent = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sections", t => t.ParentSectionID)
                .Index(t => t.ParentSectionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sections", "ParentSectionID", "dbo.Sections");
            DropIndex("dbo.Sections", new[] { "ParentSectionID" });
            DropTable("dbo.Sections");
        }
    }
}
