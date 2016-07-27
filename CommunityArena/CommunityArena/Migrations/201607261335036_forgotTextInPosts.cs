namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forgotTextInPosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Text");
        }
    }
}
