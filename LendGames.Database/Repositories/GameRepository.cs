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

        #endregion

        public async Task CreateOrEditAsync(Game game)
        {
            if (game.Id == 0)
                Insert(game);
            else
            {
                var existingGame = await FindAsync(game.Id);
                existingGame.Title = game.Title;

                Update(existingGame);
            }

        }        
    }
}
