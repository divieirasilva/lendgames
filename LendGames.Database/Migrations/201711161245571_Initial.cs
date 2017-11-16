namespace LendGames.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Usuário = c.String(nullable: false, maxLength: 128),
                        Senha = c.String(nullable: false, maxLength: 512),
                        Email = c.String(name: "E-mail", nullable: false, maxLength: 1024),
                        Ativo = c.Boolean(nullable: false),
                        ConexãoAtual = c.DateTime(name: "Conexão Atual"),
                        ÚltimaConexão = c.DateTime(name: "Última Conexão"),
                        Tipo = c.Int(nullable: false),
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
