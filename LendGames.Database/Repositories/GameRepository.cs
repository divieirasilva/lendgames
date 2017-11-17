using LendGames.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendGames.Database.Repositories
{
    public class GameRepository : Repository<LendGamesContext, Game>
    {
        public GameRepository(LendGamesContext context)
            : base(context)
        {
            BeforeInsert += GameRepository_BeforeInsert;
            BeforeUpdate += GameRepository_BeforeUpdate;
            BeforeDelete += GameRepository_BeforeDelete;
        }

        #region Validations

        private void GameRepository_BeforeUpdate(RepositoryEventArgs<Game> e)
        {
            if (Where(g => g.Title == e.Model.Title & g.Id != e.Model.Id).Any())
                throw new Exception("Você já possui um jogo com este mesmo título.");
        }

        private void GameRepository_BeforeInsert(RepositoryEventArgs<Game> e)
        {
            if (Where(g => g.Title == e.Model.Title).Any())
                throw new Exception("Você já possui um jogo com este mesmo título.");
        }

        private void GameRepository_BeforeDelete(RepositoryEventArgs<Game> e)
        {
            var game = Find(e.Model.Id);

            if (game?.IsLended == true)
                throw new Exception("Este jogo está emprestado e não pode ser removido.");
        }

        #endregion

        public async Task CreateOrEditAsync(Game game)
        {
            if (game.Id == 0)
                Insert(game);
            else
            {
                var existingGame = await FindAsync(game.Id);
                existingGame.Title = game.Title;
                existingGame.CoverFileData = game.CoverFileData;
                existingGame.CoverFileName = game.CoverFileName;
                existingGame.CoverFileType = game.CoverFileType;

                Update(existingGame);
            }

        }

        public async Task Lend(int id, int friendId)
        {
            var game = await FindAsync(id);
            var friendRepository = new FriendRepository(context);

            var friend = await friendRepository.FindAsync(friendId);

            if (game == null)
                return;

            if (game.IsLended)
                throw new Exception("Este jogo já está emprestado.");

            if (friend == null)
                throw new Exception("Você deve selecionar um amigo para emprestar o jogo.");

            game.FriendId = friendId;
            game.LastLendDate = DateTime.Now;

            Update(game);
        }

        public async Task GiveBack(int id)
        {
            var game = await FindAsync(id);

            if (game == null)
                return;

            game.FriendId = null;
            Update(game);
        }
    }
}
