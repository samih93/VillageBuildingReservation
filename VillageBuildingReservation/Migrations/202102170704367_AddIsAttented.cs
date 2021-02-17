namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAttented : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "IsAttended", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "IsAttended");
        }
    }
}
