namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveToreservation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reservations", "IsSpareReservation", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "IsSpareReservation");
            DropColumn("dbo.Reservations", "Active");
        }
    }
}
