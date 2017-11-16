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
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            BuildAccountMapping(builder);
        }

        private void BuildAccountMapping(DbModelBuilder builder)
        {
            builder.Entity<Account>().Property(a => a.Username).HasMaxLength(128).IsRequired().HasColumnName("Usuário");
            builder.Entity<Account>().Property(a => a.Password).HasMaxLength(512).IsRequired().HasColumnName("Senha");
            builder.Entity<Account>().Property(a => a.Email).HasMaxLength(1024).IsRequired().HasColumnName("E-mail");
            builder.Entity<Account>().Property(a => a.CurrentConnection).HasColumnName("Conexão Atual");
            builder.Entity<Account>().Property(a => a.LastConnection).HasColumnName("Última Conexão");
            builder.Entity<Account>().Property(a => a.Type).HasColumnName("Tipo");
            builder.Entity<Account>().Property(a => a.Enabled).HasColumnName("Ativo");
        }
    }
}
