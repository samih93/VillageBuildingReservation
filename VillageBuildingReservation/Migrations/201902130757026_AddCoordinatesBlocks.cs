namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoordinatesBlocks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blocks", "Coordinates", c => c.String());
            AlterColumn("dbo.Reservations", "ReservationName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reservations", "ReservationName", c => c.String());
            DropColumn("dbo.Blocks", "Coordinates");
        }
    }
}
