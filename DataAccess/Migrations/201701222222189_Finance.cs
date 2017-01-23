namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Finance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BankName = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Depots",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BankName = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DepotID = c.Int(nullable: false),
                        Description = c.String(),
                        CurrentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Depots", t => t.DepotID, cascadeDelete: true)
                .Index(t => t.DepotID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Positions", "DepotID", "dbo.Depots");
            DropIndex("dbo.Positions", new[] { "DepotID" });
            DropTable("dbo.Positions");
            DropTable("dbo.Depots");
            DropTable("dbo.Accounts");
        }
    }
}
