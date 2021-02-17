namespace VillageBuildingReservation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlag : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservations", "IsAttended", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reservations", "IsAttended", c => c.Boolean(nullable: false));
        }
    }
}
