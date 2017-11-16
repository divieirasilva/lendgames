using LendGames.Database.Models;
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

        [RequireConnection(RequireTypes = new AccountType[] { AccountType.Administrator })] // Apenas administradores podem acessar esta action
        public ActionResult Index()
        {
            return View();
        }

        [RequireConnection(RequireTypes = new AccountType[] { AccountType.Administrator })]
        public async Task<ActionResult> AccountsList(string search, int page = 1)
        {
            var query = _accountRepository.Where(a =>
                string.IsNullOrEmpty(search)
                | a.Username.Contains(search)
                | a.Email.Contains(search)
            );

            ViewBag.PagingData = BuildPagingData(
                page,
                await query.CountAsync(),
                out int skip
            );

            var accounts = await query
                .OrderBy(a => a.Username)
                .Skip(skip)
                .Take(ItemsPerPage)
                .ToListAsync();

            ViewBag.Search = search;
            return PartialView("_AccountsList", accounts.Select(a => MapAccountViewModel(a)));
        }

        [RequireConnection(RequireTypes = new AccountType[] { AccountType.Administrator })]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var account = await _accountRepository.FindAsync(id);
            return View(MapAccountViewModel(account));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireConnection(RequireTypes = new AccountType[] { AccountType.Administrator })]
        public async Task<ActionResult> Edit(AccountViewModel accountViewModel)
        {
            // Se for uma edição, não deve validar a senha, pois existe
            // uma action e métodos específico para este tipo de alteração.
            if (accountViewModel.Id != 0)
                ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                try
                {
                    var account = MapAccount(accountViewModel);

                    await _accountRepository.CreateOrEditAsync(account);
                    await db.SaveChangesAsync();

                    ViewBag.Success = "Os dados da conta foram salvos com sucesso.";
                    TransportViewData();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));
                }
            }

            return View(accountViewModel);
        }

        [RequireConnection(RequireTypes = new AccountType[] { AccountType.Administrator })]
        public async Task<ActionResult> Disable(int id)
        {
            var account = await _accountRepository.FindAsync(id);
            if (account == null)
                return HttpNotFound();

            return View(MapAccountViewModel(account));
        }

        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Disable")]
        [RequireConnection(RequireTypes = new AccountType[] { AccountType.Administrator })]
        public async Task<ActionResult> DisableConfirmed(int id)
        {
            var account = await _accountRepository.FindAsync(id);
            if (account == null)
                return HttpNotFound();

            try
            {
                await _accountRepository.DisableAsync(id);
                await db.SaveChangesAsync();

                ViewBag.Success = "A conta foi desativada com sucesso.";
                TransportViewData();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));
            }

            return View(MapAccountViewModel(account));
        }

        [RequireConnection(RequireTypes = new AccountType[] { AccountType.Administrator })]
        public async Task<ActionResult> Enable(int id)
        {
            var account = await _accountRepository.FindAsync(id);
            if (account == null)
                return HttpNotFound();

            return View(MapAccountViewModel(account));
        }

        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Enable")]
        [RequireConnection(RequireTypes = new AccountType[] { AccountType.Administrator })]
        public async Task<ActionResult> EnableConfirmed(int id)
        {
            var account = await _accountRepository.FindAsync(id);
            if (account == null)
                return HttpNotFound();

            try
            {
                await _accountRepository.EnableAsync(id);
                await db.SaveChangesAsync();

                ViewBag.Success = "A conta foi ativada com sucesso.";
                TransportViewData();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));
            }

            return View(MapAccountViewModel(account));
        }

        [RequireConnection]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [RequireConnection]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _accountRepository.ChangePasswordAsync(
                        ConnectedId,
                        changePasswordViewModel.CurrentPassword,
                        changePasswordViewModel.NewPassword,
                        changePasswordViewModel.ConfirmPassword
                    );

                    await db.SaveChangesAsync();

                    ViewBag.Success = "A sua senha foi alterada com sucesso.";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));
                }
            }

            return View(changePasswordViewModel);
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

        private AccountViewModel MapAccountViewModel(Account account)
        {
            var accountViewModel = new AccountViewModel();

            if (account != null)
            {
                accountViewModel.Id = account.Id;
                accountViewModel.Username = account.Username;
                accountViewModel.Email = account.Email;
                accountViewModel.Type = account.Type;
                accountViewModel.Enabled = account.Enabled;
            }

            return accountViewModel;
        }

        private Account MapAccount(AccountViewModel accountViewModel)
        {
            var account = new Account
            {
                Id = accountViewModel.Id,
                Email = accountViewModel.Email,
                Username = accountViewModel.Username,
                Enabled = accountViewModel.Enabled,
                Password = accountViewModel.Password,
                Type = accountViewModel.Type
            };

            return account;
        }
    }
}