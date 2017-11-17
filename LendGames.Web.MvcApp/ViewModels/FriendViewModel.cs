using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LendGames.Web.MvcApp.ViewModels
{
    public class FriendViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome deve ser informado.")]
        [MaxLength(256, ErrorMessage = "O nome deve conter no máximo {1} letras.")]
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O e-mail deve ser informado.")]
        [MaxLength(1024, ErrorMessage = "O e-mail deve conter no máximo {1} letras.")]
        public string Email { get; set; }

        [Display(Name = "Jogos Emprestados")]
        public List<GameViewModel> LendedGames { get; set; }
    }
}