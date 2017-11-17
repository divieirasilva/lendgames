using LendGames.Database.Models;
using LendGames.Web.MvcApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LendGames.Web.MvcApp
{
    public static class ModelMapper
    {        
        public static FriendViewModel MapFriendViewModel(Friend friend)
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

        public static Friend MapFriend(FriendViewModel friendViewModel)
        {
            var friend = new Friend
            {
                Id = friendViewModel.Id,
                Email = friendViewModel.Email,
                Name = friendViewModel.Name
            };

            return friend;
        }

        public static AccountViewModel MapAccountViewModel(Account account)
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

        public static Account MapAccount(AccountViewModel accountViewModel)
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

        public static GameViewModel MapGameViewModel(Game game)
        {
            var gameViewModel = new GameViewModel();

            if (game != null)
            {
                gameViewModel.Id = game.Id;
                gameViewModel.Title = game.Title;
                gameViewModel.IsLended = game.IsLended;
                gameViewModel.CoverFileName = game.CoverFileName;
                gameViewModel.LendedFor = game.Friend?.Name ?? string.Empty;
                gameViewModel.LendedForId = game.FriendId ?? 0;
            }

            return gameViewModel;
        }

        public static Game MapGame(GameViewModel gameViewModel)
        {
            var game = new Game
            {
                Id = gameViewModel.Id,
                Title = gameViewModel.Title
            };

            if (gameViewModel.CoverFile != null)
            {
                game.CoverFileName = Path.GetFileName(gameViewModel.CoverFile.FileName);
                game.CoverFileType = gameViewModel.CoverFile.ContentType;
                game.CoverFileData = new byte[gameViewModel.CoverFile.ContentLength];
                gameViewModel.CoverFile.InputStream.Read(game.CoverFileData, 0, gameViewModel.CoverFile.ContentLength);                
            }

            return game;
        }
    }
}