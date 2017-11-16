using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendGames.Database.Repositories
{
    public class RepositoryEventArgs<TModel> : EventArgs
         where TModel : class
    {
        public TModel Model { get; set; }
        public IEnumerable<TModel> Models { get; set; }
    }
}
