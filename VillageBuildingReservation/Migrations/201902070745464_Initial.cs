namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Name = c.String(),
                        ZoneId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ZoneBlocks",
                c => new
                    {
                        Zone_Id = c.Int(nullable: false),
                        Block_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Zone_Id, t.Block_Id })
                .ForeignKey("dbo.Zones", t => t.Zone_Id, cascadeDelete: true)
                .ForeignKey("dbo.Blocks", t => t.Block_Id, cascadeDelete: true)
                .Index(t => t.Zone_Id)
                .Index(t => t.Block_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZoneBlocks", "Block_Id", "dbo.Blocks");
            DropForeignKey("dbo.ZoneBlocks", "Zone_Id", "dbo.Zones");
            DropIndex("dbo.ZoneBlocks", new[] { "Block_Id" });
            DropIndex("dbo.ZoneBlocks", new[] { "Zone_Id" });
            DropTable("dbo.ZoneBlocks");
            DropTable("dbo.Zones");
            DropTable("dbo.Blocks");
        }
    }
}
