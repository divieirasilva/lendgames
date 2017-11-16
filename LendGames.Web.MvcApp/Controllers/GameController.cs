using LendGames.Database.Repositories;
using LendGames.Utils.Paging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LendGames.Web.MvcApp.Controllers
{
    public class GameController : Controller
    {
        private GameRepository _gameRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            _gameRepository = new GameRepository(db);
        }
        
        [RequireConnection] // Verifica se há uma conta conectada e se ela pode prosseguir com o request
        public async Task<ActionResult> Index(int page = 1)
        {
            int skip = (page - 1) * ItemsPerPage;
            var query = _gameRepository.Where();

            ViewBag.PagingData = new PagingData
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                PagesPerGroup = 3,
                TotalItems = await query.CountAsync()
            };

            var materials = await query
                .OrderBy(o => o.Title)
                .Skip(skip)
                .Take(ItemsPerPage)
                .ToListAsync();

            return View(materials);
        }
    }
}