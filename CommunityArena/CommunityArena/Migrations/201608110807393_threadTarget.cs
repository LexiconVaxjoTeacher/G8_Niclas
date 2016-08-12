namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class threadTarget : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Threads", "Target", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Threads", "Target");
        }
    }
}
