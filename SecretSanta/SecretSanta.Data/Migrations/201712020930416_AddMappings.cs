namespace SecretSanta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMappings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Presents", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Presents", "To_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "To_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Presents", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Requests", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Presents", "From_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "From_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Groups", new[] { "User_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Group_Id" });
            DropIndex("dbo.Presents", new[] { "To_Id" });
            DropIndex("dbo.Presents", new[] { "User_Id" });
            DropIndex("dbo.Requests", new[] { "To_Id" });
            DropIndex("dbo.Requests", new[] { "User_Id" });
            DropColumn("dbo.Presents", "To_Id");
            DropColumn("dbo.Requests", "To_Id");
            RenameColumn(table: "dbo.Presents", name: "User_Id", newName: "To_Id");
            RenameColumn(table: "dbo.Requests", name: "User_Id", newName: "To_Id");
            DropPrimaryKey("dbo.Groups");
            DropPrimaryKey("dbo.AspNetUsers");
            CreateTable(
                "dbo.GroupUsers",
                c => new
                    {
                        Group_Id = c.Guid(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.User_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.User_Id);
            
            AlterColumn("dbo.Groups", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Presents", "To_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Presents", "To_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Requests", "To_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Requests", "To_Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Groups", "Id");
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            CreateIndex("dbo.Presents", "To_Id");
            CreateIndex("dbo.Requests", "To_Id");
            AddForeignKey("dbo.Presents", "To_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "To_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Presents", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.Requests", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Presents", "From_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Requests", "From_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Groups", "OwnerId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Groups", "User_Id");
            DropColumn("dbo.AspNetUsers", "Group_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Group_Id", c => c.Guid());
            AddColumn("dbo.Groups", "User_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Groups", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "From_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Presents", "From_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Presents", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Requests", "To_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Presents", "To_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupUsers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.GroupUsers", new[] { "User_Id" });
            DropIndex("dbo.GroupUsers", new[] { "Group_Id" });
            DropIndex("dbo.Requests", new[] { "To_Id" });
            DropIndex("dbo.Presents", new[] { "To_Id" });
            DropPrimaryKey("dbo.AspNetUsers");
            DropPrimaryKey("dbo.Groups");
            AlterColumn("dbo.Requests", "To_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Requests", "To_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Presents", "To_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Presents", "To_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Groups", "Id", c => c.Guid(nullable: false));
            DropTable("dbo.GroupUsers");
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            AddPrimaryKey("dbo.Groups", "Id");
            RenameColumn(table: "dbo.Requests", name: "To_Id", newName: "User_Id");
            RenameColumn(table: "dbo.Presents", name: "To_Id", newName: "User_Id");
            AddColumn("dbo.Requests", "To_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Presents", "To_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "User_Id");
            CreateIndex("dbo.Requests", "To_Id");
            CreateIndex("dbo.Presents", "User_Id");
            CreateIndex("dbo.Presents", "To_Id");
            CreateIndex("dbo.AspNetUsers", "Group_Id");
            CreateIndex("dbo.Groups", "User_Id");
            AddForeignKey("dbo.Groups", "OwnerId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "From_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Presents", "From_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.Presents", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.Requests", "To_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Presents", "To_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Requests", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Presents", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.Groups", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
