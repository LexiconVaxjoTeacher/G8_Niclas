namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSubforumTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fora", "ParentForumID", c => c.Int(nullable: false));
            DropTable("dbo.SubForums");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubForums",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentForumID = c.Int(nullable: false),
                        SubForumID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Fora", "ParentForumID");
        }
    }
}
