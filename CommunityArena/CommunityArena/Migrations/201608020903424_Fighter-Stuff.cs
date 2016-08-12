namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FighterStuff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fighters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        HP = c.Int(nullable: false),
                        Strength = c.Int(nullable: false),
                        Skill = c.Int(nullable: false),
                        Defense = c.Int(nullable: false),
                        Speed = c.Int(nullable: false),
                        Luck = c.Int(nullable: false),
                        Constitution = c.Int(nullable: false),
                        Sense = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Experience = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Gold = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StatBoosted = c.String(),
                        Type = c.String(),
                        Amount = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Ownerships",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FighterID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ownerships");
            DropTable("dbo.Items");
            DropTable("dbo.Fighters");
        }
    }
}
