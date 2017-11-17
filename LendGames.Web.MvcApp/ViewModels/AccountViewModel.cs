using LendGames.Database.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LendGames.Web.MvcApp.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "O usuário deve ser informado.")]
        [MaxLength(128, ErrorMessage = "O usuário não deve ser maior que {1} letras.")]
        public string Username { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "A senha deve ser informado.")]
        [MaxLength(512, ErrorMessage = "A senha não deve ser maior que {1} letras.")]
        public string Password { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "O e-mail deve ser informado.")]
        [MaxLength(512, ErrorMessage = "O e-mail não deve ser maior que {1} letras.")]
        public string Email { get; set; }

        [Display(Name = "Ativo")]
        public bool Enabled { get; set; }

        [Display(Name = "Tipo")]
        public AccountType Type { get; set; }
    }
}