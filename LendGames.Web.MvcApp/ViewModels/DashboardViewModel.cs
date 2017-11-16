using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendGames.Web.MvcApp.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalGames { get; set; }
        public int TotalFriends { get; set; }
        public int TotalLendedGames { get; set; }
        public string AccountName { get; set; }
    }
}