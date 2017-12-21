namespace SecretSanta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUnusedTables : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Presents", newName: "Connections");
            DropForeignKey("dbo.Requests", "To_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "To_Id" });
            AlterColumn("dbo.Requests", "To_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Requests", "To_Id");
            AddForeignKey("dbo.Requests", "To_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "To_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "To_Id" });
            AlterColumn("dbo.Requests", "To_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "To_Id");
            AddForeignKey("dbo.Requests", "To_Id", "dbo.AspNetUsers", "Id");
            RenameTable(name: "dbo.Connections", newName: "Presents");
        }
    }
}
