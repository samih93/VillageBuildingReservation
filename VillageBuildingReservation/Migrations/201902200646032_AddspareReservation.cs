namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddspareReservation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reservations", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "Active", c => c.Boolean(nullable: false));
        }
    }
}
