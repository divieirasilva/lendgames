using LendGames.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendGames.Database
{
    public class LendGamesContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            BuildAccountMapping(builder);
            BuildGameMapping(builder);
            BuildFriendMapping(builder);
        }

        private void BuildAccountMapping(DbModelBuilder builder)
        {
            builder.Entity<Account>().Property(a => a.Username).HasMaxLength(128).IsRequired();
            builder.Entity<Account>().Property(a => a.Password).HasMaxLength(512).IsRequired();
            builder.Entity<Account>().Property(a => a.Email).HasMaxLength(1024).IsRequired();
            builder.Entity<Account>().Property(a => a.CurrentConnection);
            builder.Entity<Account>().Property(a => a.LastConnection);
            builder.Entity<Account>().Property(a => a.Type);
            builder.Entity<Account>().Property(a => a.Enabled);
        }

        private void BuildGameMapping(DbModelBuilder builder)
        {
            builder.Entity<Game>().Property(g => g.Title).HasMaxLength(1024).IsRequired();
        }

        private void BuildFriendMapping(DbModelBuilder builder)
        {
            builder.Entity<Friend>().Property(f => f.Email).HasMaxLength(1024).IsRequired();
            builder.Entity<Friend>().Property(f => f.Name).HasMaxLength(256).IsRequired();
        }
    }
}
