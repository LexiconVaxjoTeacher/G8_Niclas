namespace CommunityArena.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class maxHP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fighters", "MaxHP", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fighters", "MaxHP");
        }
    }
}
