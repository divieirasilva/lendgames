namespace LendGames.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGameAndFriendEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Email = c.String(nullable: false, maxLength: 1024),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 1024),
                        LastLendDate = c.DateTime(),
                        FriendId = c.Int(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Friends", t => t.FriendId)
                .Index(t => t.FriendId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "FriendId", "dbo.Friends");
            DropIndex("dbo.Games", new[] { "FriendId" });
            DropTable("dbo.Games");
            DropTable("dbo.Friends");
        }
    }
}
