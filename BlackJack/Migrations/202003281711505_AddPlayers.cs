namespace BlackJack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlayers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Player_PlayerID", c => c.Int());
            CreateIndex("dbo.Players", "Player_PlayerID");
            AddForeignKey("dbo.Players", "Player_PlayerID", "dbo.Players", "PlayerID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "Player_PlayerID", "dbo.Players");
            DropIndex("dbo.Players", new[] { "Player_PlayerID" });
            DropColumn("dbo.Players", "Player_PlayerID");
        }
    }
}
