namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class publicThreads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Threads", "Public", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Threads", "Public");
        }
    }
}
