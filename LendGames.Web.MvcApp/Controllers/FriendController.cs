﻿using LendGames.Database.Models;
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
            return PartialView("_FriendsList", friends.Select(f => ModelMapper.MapFriendViewModel(f)));
        }

        [RequireConnection]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var friend = await _friendRepository.FindAsync(id);
            return View(ModelMapper.MapFriendViewModel(friend));
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
                    var friend = ModelMapper.MapFriend(friendViewModel);

                    await _friendRepository.CreateOrEditAsync(friend);
                    await db.SaveChangesAsync();

                    ViewBag.Success = "Os dados do amigo foram salvos com sucesso.";
                    TransportViewData();
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

            return View(ModelMapper.MapFriendViewModel(friend));
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

                ViewBag.Success = "O amigo foi removido com sucesso.";
                TransportViewData();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ExtractEntityMessage(ex));
            }

            return View(ModelMapper.MapFriendViewModel(friend));
        }

        [RequireConnection]
        public async Task<ActionResult> FindMany(string query)
        {
            if (string.IsNullOrEmpty(query))
                return Json("", JsonRequestBehavior.AllowGet);

            var friends = await _friendRepository
                .Where(f =>
                    f.Email.Contains(query)
                    | f.Name.Contains(query)
                    | f.Id.ToString() == query
                ).Select(f => new
                {
                    Id = f.Id,
                    Name = f.Name,
                    Email = f.Email
                }).ToListAsync();

            if (friends.Count < 1)
                return Json("", JsonRequestBehavior.AllowGet);

            return Json(friends, JsonRequestBehavior.AllowGet);
        }
    }
}