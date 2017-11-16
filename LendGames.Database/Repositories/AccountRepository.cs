using LendGames.Database.Models;
using LendGames.Utils.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendGames.Database.Repositories
{
    public class AccountRepository : Repository<LendGamesContext, Account>
    {
        public AccountRepository(LendGamesContext context)
            : base(context)
        {
            BeforeInsert += AccountRepository_BeforeInsert;
            BeforeUpdate += AccountRepository_BeforeUpdate;            
        }

        #region Validations

        private void AccountRepository_BeforeUpdate(RepositoryEventArgs<Account> e)
        {
            if (Where(g => g.Username == e.Model.Username & g.Id != e.Model.Id).Any())
                throw new Exception("Já existe uma conta com este usuário.");

            if (Where(g => g.Email == e.Model.Email & g.Id != e.Model.Id).Any())
                throw new Exception("Já existe uma conta com este e-mail.");
        }

        private void AccountRepository_BeforeInsert(RepositoryEventArgs<Account> e)
        {
            if (Where(g => g.Username == e.Model.Username).Any())
                throw new Exception("Já existe uma conta com este usuário.");

            if (Where(g => g.Email == e.Model.Email).Any())
                throw new Exception("Já existe uma conta com este e-mail.");
        }

        #endregion

        public async Task CreateOrEditAsync(Account account)
        {
            if (account.Id == 0)
                Insert(account);
            else
            {
                // Senha e Status de ativo da conta devem possuem um método próprio para edição

                var existingAccount = await FindAsync(account.Id);
                existingAccount.Username = account.Username;
                existingAccount.Email = account.Email;
                existingAccount.Type = account.Type;

                Update(existingAccount);
            }

        }

        public async Task<Account> ConnectAsync(string username, string password)
        {
            var accounts = Where(g => g.Username == username);

            if (!await accounts.AnyAsync())
                throw new Exception("O usuário informado está incorreto.");

            password = password.Encrypt(Model.Key);
            var account = await accounts.FirstOrDefaultAsync(filter => filter.Password == password);

            if (account == null)
                throw new Exception("A senha informada está incorreta.");

            if (!account.Enabled)
                throw new Exception("Este usuário está desativado.");

            return account;
        }

        public async Task SetLastConnectionAsync(int id)
        {
            var account = await FindAsync(id);

            account.LastConnection = account.CurrentConnection;
            account.CurrentConnection = DateTime.Now;

            Update(account);
        }

        public async Task DisableAsync(int id)
        {
            var existingAccount = await FindAsync(id);

            if (existingAccount == null)
                return;

            existingAccount.Enabled = false;
            Update(existingAccount);
        }

        public async Task EnableAsync(int id)
        {
            var existingAccount = await FindAsync(id);

            if (existingAccount == null)
                return;

            existingAccount.Enabled = true;
            Update(existingAccount);
        }

        public async Task ChangePasswordAsync(int id, string currentPassword, string newPassword, string confirmPassword)
        {
            var account = await FindAsync(id);
            if (account == null)
                return;

            currentPassword = currentPassword.Encrypt(Model.Key);
            if (currentPassword != account.Password)
                throw new Exception("A senha atual informada está incorreta.");

            if (newPassword != confirmPassword)
                throw new Exception("A nova senha e a confirmação de senha não são iguais.");

            account.Password = newPassword.Encrypt(Model.Key);
            Update(account);
        }
    }
}
