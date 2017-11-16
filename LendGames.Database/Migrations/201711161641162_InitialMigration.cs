namespace LendGames.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false, maxLength: 512),
                        Email = c.String(nullable: false, maxLength: 1024),
                        Enabled = c.Boolean(nullable: false),
                        CurrentConnection = c.DateTime(),
                        LastConnection = c.DateTime(),
                        Type = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Accounts");
        }
    }
}
