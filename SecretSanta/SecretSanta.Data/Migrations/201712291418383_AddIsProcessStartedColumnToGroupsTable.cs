namespace SecretSanta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsProcessStartedColumnToGroupsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "IsProcessStarted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "IsProcessStarted");
        }
    }
}
