using LendGames.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendGames.Database.Repositories
{
    public class FriendRepository : Repository<LendGamesContext, Friend>
    {
        private GameRepository _gameRepository;

        public FriendRepository(LendGamesContext context)
            : base(context)
        {
            _gameRepository = new GameRepository(context);

            BeforeInsert += FriendRepository_BeforeInsert;
            BeforeUpdate += FriendRepository_BeforeUpdate;
            BeforeDelete += FriendRepository_BeforeDelete;
        }

        #region Validations

        private void FriendRepository_BeforeUpdate(RepositoryEventArgs<Friend> e)
        {
            if (Where(g => g.Email == e.Model.Email & g.Id != e.Model.Id).Any())
                throw new Exception("Você já possui um amigo com este mesmo e-mail.");
        }

        private void FriendRepository_BeforeInsert(RepositoryEventArgs<Friend> e)
        {
            if (Where(g => g.Email == e.Model.Email).Any())
                throw new Exception("Você já possui um amigo com este mesmo e-mail.");
        }

        private void FriendRepository_BeforeDelete(RepositoryEventArgs<Friend> e)
        {
            var hasGames = _gameRepository.Where(g => g.FriendId == e.Model.Id).Any();

            if (hasGames)
                throw new Exception("Você não pode remover este amigo, pois ele possui jogos emprestados.");
        }

        #endregion

        public async Task CreateOrEditAsync(Friend friend)
        {
            if (friend.Id == 0)
                Insert(friend);
            else
            {
                var existingFriend = await FindAsync(friend.Id);
                existingFriend.Name = friend.Name;
                existingFriend.Email = friend.Email;

                Update(existingFriend);
            }

        }

        public async Task<List<Game>> GetLendedGames(int id)
        {
            var games = await _gameRepository.Where(g => g.FriendId == id).ToListAsync();
            return games;
        }
    }
}
