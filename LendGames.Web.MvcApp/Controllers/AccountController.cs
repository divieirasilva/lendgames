using LendGames.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LendGames.Web.MvcApp.Controllers
{
    public class AccountController : Controller
    {
        private AccountRepository _accountRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            _accountRepository = new AccountRepository(db);
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

    }
}