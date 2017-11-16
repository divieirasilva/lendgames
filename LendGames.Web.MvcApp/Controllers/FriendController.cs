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
    public class FriendController : Controller
    {
        private FriendRepository _friendRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            _friendRepository = new FriendRepository(db);
        }

        [RequireConnection] // Verifica se há uma conta conectada e se ela pode prosseguir com o request
        public ActionResult Index()
        {
            return View();
        }

        [RequireConnection]
        public async Task<ActionResult> FriendsList(string search, int page = 1)
        {
            var query = _friendRepository.Where(f => 
                string.IsNullOrEmpty(search) 
                | f.Name.Contains(search)
                | f.Email.Contains(search)
            );

            ViewBag.PagingData = BuildPagingData(
                page,
                await query.CountAsync(),
                out int skip
            );

            var friends = await query
                .OrderBy(f => f.Name)
                .Skip(skip)
                .Take(ItemsPerPage)
                .ToListAsync();            

            ViewBag.Search = search;
            return PartialView("_FriendsList", friends.Select(f => MapFriendViewModel(f)));
        }

        [RequireConnection]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var friend = await _friendRepository.FindAsync(id);
            return View(MapFriendViewModel(friend));
        }

        [HttpPost]
        [RequireConnection]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FriendViewModel friendViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var friend = MapFriend(friendViewModel);

                    await _friendRepository.CreateOrEditAsync(friend);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));
                }
            }

            return View(friendViewModel);
        }

        [RequireConnection]
        public async Task<ActionResult> Remove(int id)
        {
            var friend = await _friendRepository.FindAsync(id);
            if (friend == null)
                return HttpNotFound();

            return View(MapFriendViewModel(friend));
        }

        [RequireConnection]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Remove")]
        public async Task<ActionResult> RemoveConfirmed(int id)
        {
            var friend = await _friendRepository.FindAsync(id);
            if (friend == null)
                return HttpNotFound();

            try
            {
                _friendRepository.Delete(id);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));
            }

            return View(MapFriendViewModel(friend));
        }

        private FriendViewModel MapFriendViewModel(Friend friend)
        {
            var friendViewModel = new FriendViewModel();

            if (friend != null)
            {
                friendViewModel.Id = friend.Id;
                friendViewModel.Name = friend.Name;
                friendViewModel.Email = friend.Email;
            }

            return friendViewModel;
        }

        private Friend MapFriend(FriendViewModel friendViewModel)
        {
            var friend = new Friend
            {
                Id = friendViewModel.Id,
                Email = friendViewModel.Email,
                Name = friendViewModel.Name
            };

            return friend;
        }
    }
}