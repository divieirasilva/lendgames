namespace LendGames.Database.Migrations
{
    using LendGames.Database.Models;
    using LendGames.Utils.Security;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LendGames.Database.LendGamesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LendGamesContext context)
        {
            context.Accounts.AddOrUpdate(a =>
                a.Username,
                new Account
                {
                    Username = "admin",                 
                    Password = "P@ssw()rd".Encrypt(Model.Key),                    
                    Email = "admin@lendgames.com",
                    Enabled = true,                    
                    Type = AccountType.Administrator
                });

            context.SaveChanges();
        }
    }
}
