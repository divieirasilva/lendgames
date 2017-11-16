using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LendGames.Web.MvcApp.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        [Required(ErrorMessage = "A nova senha deve ser informada.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Required(ErrorMessage = "Por favor, confirme a senha.")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação de senha não são iguais.")]
        public string ConfirmPassword { get; set; }
    }
}