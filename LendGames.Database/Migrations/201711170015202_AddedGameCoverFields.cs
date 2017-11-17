namespace LendGames.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGameCoverFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "CoverFileName", c => c.String());
            AddColumn("dbo.Games", "CoverFileData", c => c.Binary());
            AddColumn("dbo.Games", "CoverFileType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "CoverFileType");
            DropColumn("dbo.Games", "CoverFileData");
            DropColumn("dbo.Games", "CoverFileName");
        }
    }
}
