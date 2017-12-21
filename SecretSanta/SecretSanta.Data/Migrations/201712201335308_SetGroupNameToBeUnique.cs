namespace SecretSanta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetGroupNameToBeUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Groups", "Name", c => c.String(maxLength: 100));
            CreateIndex("dbo.Groups", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Groups", new[] { "Name" });
            AlterColumn("dbo.Groups", "Name", c => c.String());
        }
    }
}
