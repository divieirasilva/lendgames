using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LendGames.Database.Models
{
    public class Game : Model
    {
        public string Title { get; set; }        

        public DateTime? LastLendDate { get; set; }

        public int? FriendId { get; set; }
        public virtual Friend Friend { get; set; }
        
        public string CoverFileName { get; set; }        
        public byte[] CoverFileData { get; set; }
        public string CoverFileType { get; set; }

        [NotMapped]
        public bool IsLended
        {
            get
            {
                return FriendId != null;
            }
        }
    }
}
