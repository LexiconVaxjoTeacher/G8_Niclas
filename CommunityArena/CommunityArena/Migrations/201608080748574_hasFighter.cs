namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hasFighter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "HasFighter", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "HasFighter");
        }
    }
}
