namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAttented1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservations", "IsAttended", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reservations", "IsAttended", c => c.Boolean());
        }
    }
}
