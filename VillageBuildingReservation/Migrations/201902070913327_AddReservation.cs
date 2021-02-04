namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReservation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReservationName = c.String(),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReservationBlocks",
                c => new
                    {
                        Reservation_Id = c.Int(nullable: false),
                        Block_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reservation_Id, t.Block_Id })
                .ForeignKey("dbo.Reservations", t => t.Reservation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Blocks", t => t.Block_Id, cascadeDelete: true)
                .Index(t => t.Reservation_Id)
                .Index(t => t.Block_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReservationBlocks", "Block_Id", "dbo.Blocks");
            DropForeignKey("dbo.ReservationBlocks", "Reservation_Id", "dbo.Reservations");
            DropIndex("dbo.ReservationBlocks", new[] { "Block_Id" });
            DropIndex("dbo.ReservationBlocks", new[] { "Reservation_Id" });
            DropTable("dbo.ReservationBlocks");
            DropTable("dbo.Reservations");
        }
    }
}
