namespace SecretSanta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRequestModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "From_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "From_Id" });
            AddColumn("dbo.Requests", "ReceivedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Requests", "State");
            DropColumn("dbo.Requests", "From_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "From_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Requests", "State", c => c.Int(nullable: false));
            DropColumn("dbo.Requests", "ReceivedAt");
            CreateIndex("dbo.Requests", "From_Id");
            AddForeignKey("dbo.Requests", "From_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
