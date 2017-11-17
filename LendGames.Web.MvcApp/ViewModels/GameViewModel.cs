using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LendGames.Web.MvcApp.ViewModels
{
    public class GameViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "O título do jogo deve ser informado")]
        [MaxLength(128, ErrorMessage = "O título do jogo não deve ser maior que {1} letras.")]
        public string Title { get; set; }

        [Display(Name = "Emprestado")]
        public bool IsLended { get; set; }        
        public string LendedFor { get; set; }
        public int LendedForId { get; set; }

        [Display(Name = "Capa")]
        public string CoverFileName { get; set; }

        public HttpPostedFileBase CoverFile { get; set; }
    }
}