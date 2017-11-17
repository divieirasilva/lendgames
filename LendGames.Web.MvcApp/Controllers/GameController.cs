using LendGames.Database.Models;
using LendGames.Database.Repositories;
using LendGames.Web.MvcApp.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Index()
        {
            return View();
        }

        [RequireConnection] 
        public async Task<ActionResult> GamesList(string search, int page = 1)
        {            
            var query = _gameRepository.Where(g => string.IsNullOrEmpty(search) | g.Title.Contains(search));

            ViewBag.PagingData = BuildPagingData(
                page, 
                await query.CountAsync(), 
                out int skip
            );

            var games = await query                
                .OrderBy(g => g.Title)
                .Skip(skip)
                .Take(ItemsPerPage)
                .Include(i => i.Friend)
                .ToListAsync();

            ViewBag.Search = search;
            return PartialView("_GamesList", games.Select(g => ModelMapper.MapGameViewModel(g)));
        }        

        [RequireConnection]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var game = await _gameRepository.FindAsync(id);
            return View(ModelMapper.MapGameViewModel(game));
        }

        [HttpPost]
        [RequireConnection]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GameViewModel gameViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var game = ModelMapper.MapGame(gameViewModel);

                    await _gameRepository.CreateOrEditAsync(game);
                    await db.SaveChangesAsync();

                    ViewBag.Success = "Os dados do jogo foram salvos com sucesso.";
                    TransportViewData();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));
                }             
            }

            return View(gameViewModel);
        }

        [RequireConnection]
        public async Task<ActionResult> Remove(int id)
        {
            var game = await _gameRepository.FindAsync(id);
            if (game == null)
                return HttpNotFound();

            return View(ModelMapper.MapGameViewModel(game));
        }

        [RequireConnection]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Remove")]    
        public async Task<ActionResult> RemoveConfirmed(int id)
        {
            var game = await _gameRepository.FindAsync(id);
            if (game == null)
                return HttpNotFound();

            try
            {
                _gameRepository.Delete(id);
                await db.SaveChangesAsync();

                ViewBag.Success = "O jogo foi removido com sucesso.";
                TransportViewData();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));                
            }

            return View(ModelMapper.MapGameViewModel(game));
        }

        [RequireConnection]
        public async Task<ActionResult> RenderCover(int id)
        {
            var game = await _gameRepository.FindAsync(id);
            if (game == null || game.CoverFileData == null)
                return File("/Assets/NoCover.jpg", "image/jpeg");

            return File(game.CoverFileData, game.CoverFileType);
        }
    }
}