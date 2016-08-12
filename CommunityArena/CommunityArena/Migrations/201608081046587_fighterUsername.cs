namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fighterUsername : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fighters", "Username", c => c.String());
            DropColumn("dbo.Fighters", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fighters", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Fighters", "Username");
        }
    }
}
