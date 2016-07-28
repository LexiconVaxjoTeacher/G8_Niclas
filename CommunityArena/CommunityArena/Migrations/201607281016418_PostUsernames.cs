namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostUsernames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Poster", c => c.String());
            DropColumn("dbo.Posts", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "UserID", c => c.Int(nullable: false));
            DropColumn("dbo.Posts", "Poster");
        }
    }
}
