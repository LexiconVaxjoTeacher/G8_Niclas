namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alerts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alerts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ThreadID = c.Int(nullable: false),
                        User = c.String(),
                        Viewed = c.Boolean(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Alerts");
        }
    }
}
