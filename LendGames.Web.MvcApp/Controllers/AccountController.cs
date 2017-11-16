using LendGames.Database.Repositories;
using LendGames.Web.MvcApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LendGames.Web.MvcApp.Controllers
{
    public class AccountController : Controller
    {
        private AccountRepository _accountRepository;
        private GameRepository _gameRepository;
        private FriendRepository _friendRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            _accountRepository = new AccountRepository(db);
            _gameRepository = new GameRepository(db);
            _friendRepository = new FriendRepository(db);
        }

        [HttpPost]
        public async Task<ActionResult> Connect(string username, string password)
        {
            try
            {
                ConnectedAccount = await _accountRepository.ConnectAsync(username, password);
                ConnectedId = ConnectedAccount?.Id ?? 0;

                await _accountRepository.SetLastConnectionAsync(ConnectedId);
                await db.SaveChangesAsync();

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));

                TransportViewData();
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Disconnect()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [RequireConnection] // Verifica se há uma conta conectada e se ela pode prosseguir com o request
        public ActionResult Dashboard()
        {
            return View();
        }

        [RequireConnection]
        public async Task<ActionResult> DashboardInfo()
        {
            var games = await _gameRepository.Where().ToListAsync();
            var friends = await _friendRepository.Where().ToListAsync();
            var lendedGames = games.Where(g => g.IsLended).ToList();
            var connectedAccount = await _accountRepository.FindAsync(ConnectedId);

            var dashboardData = new DashboardViewModel
            {
                TotalFriends = friends.Count,
                TotalGames = games.Count,
                TotalLendedGames = lendedGames.Count,
                AccountName = connectedAccount.Username
            };

            return PartialView("_DashboardInfo", dashboardData);
        }
    }
}