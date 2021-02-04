namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReservation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "ReservationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Reservations", "From");
            DropColumn("dbo.Reservations", "To");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "To", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reservations", "From", c => c.DateTime(nullable: false));
            DropColumn("dbo.Reservations", "ReservationDate");
        }
    }
}
