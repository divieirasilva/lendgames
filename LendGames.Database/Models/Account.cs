using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendGames.Database.Models
{
    public enum AccountType
    {
        [Display(Name = "Administrador")]
        Administrator,
        [Display(Name = "Comum")]
        Common
    }

    public class Account : Model
    {        
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }        

        public DateTime? CurrentConnection { get; set; }        
        public DateTime? LastConnection { get; set; }

        public AccountType Type { get; set; }
    }
}
