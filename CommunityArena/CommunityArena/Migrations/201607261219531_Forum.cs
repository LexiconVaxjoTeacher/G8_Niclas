namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forum : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fora",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ThreadID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        PostTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SubForums",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentForumID = c.Int(nullable: false),
                        SubForumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Threads",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ForumID = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.AspNetUsers", "CurrentForumID", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "CurrentThreadID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CurrentThreadID");
            DropColumn("dbo.AspNetUsers", "CurrentForumID");
            DropTable("dbo.Threads");
            DropTable("dbo.SubForums");
            DropTable("dbo.Posts");
            DropTable("dbo.Fora");
        }
    }
}
