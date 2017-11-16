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
            : base(context) { }

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
    }
}
