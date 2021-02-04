namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ZoneBlocks", "Zone_Id", "dbo.Zones");
            DropForeignKey("dbo.ZoneBlocks", "Block_Id", "dbo.Blocks");
            DropIndex("dbo.ZoneBlocks", new[] { "Zone_Id" });
            DropIndex("dbo.ZoneBlocks", new[] { "Block_Id" });
            CreateIndex("dbo.Blocks", "ZoneId");
            AddForeignKey("dbo.Blocks", "ZoneId", "dbo.Zones", "Id", cascadeDelete: true);
            DropTable("dbo.ZoneBlocks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ZoneBlocks",
                c => new
                    {
                        Zone_Id = c.Int(nullable: false),
                        Block_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Zone_Id, t.Block_Id });
            
            DropForeignKey("dbo.Blocks", "ZoneId", "dbo.Zones");
            DropIndex("dbo.Blocks", new[] { "ZoneId" });
            CreateIndex("dbo.ZoneBlocks", "Block_Id");
            CreateIndex("dbo.ZoneBlocks", "Zone_Id");
            AddForeignKey("dbo.ZoneBlocks", "Block_Id", "dbo.Blocks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ZoneBlocks", "Zone_Id", "dbo.Zones", "Id", cascadeDelete: true);
        }
    }
}
