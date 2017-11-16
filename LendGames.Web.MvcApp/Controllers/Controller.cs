using LendGames.Database;
using LendGames.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*
* *************************************************************
* 
* Esta classe abstrata será a base de todos os controllers, 
* com ela centralizaremos alguns objetos e métodos que serão
* reutilizados na maioria dos controllers do sistema, como o
* atual usuário conectado, por exemplo. Aqui também será feita
* a conexão com o banco de dados.
* 
* *************************************************************
*/

namespace LendGames.Web.MvcApp.Controllers
{
    public abstract class Controller : System.Web.Mvc.Controller
    {
        public Controller()
            : base()
        {
            db = new LendGamesContext();
            db.Configuration.ValidateOnSaveEnabled = false;
        }

        #region Public Properties

        /// <summary>
        /// Id do atual usuário conectado no sistema.
        /// </summary>
        protected int ConnectedId
        {
            get
            {
                int connectedId = 0;

                if (Session != null && Session["ConnectedId"] != null)
                    int.TryParse(Session["ConnectedId"].ToString(), out connectedId);

                return connectedId;
            }
            set
            {
                Session["ConnectedId"] = value;
            }
        }

        /// <summary>
        /// Objeto do atual usuário conectado no sistema
        /// </summary>
        protected Account ConnectedAccount
        {
            get
            {
                if (Session != null && Session["ConnectedAccount"] != null)
                    return Session["ConnectedAccount"] as Account;

                return null;
            }
            set
            {
                Session["ConnectedAccount"] = value;
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Conexão ativa com o banco de dados.
        /// </summary>
        protected LendGamesContext db { get; set; }

        /// <summary>
        /// Quantidade de itens que será mostrada em tabelas com paginação.
        /// </summary>
        protected int ItemsPerPage = 8;

        #endregion

        #region Protected Methods

        protected string ExtractEntityMessage(Exception exception)
        {
            if (exception == null)
                return string.Empty;

            if (exception.GetType() == typeof(DbEntityValidationException))
            {

                string message = string.Empty;
                foreach (var validationErrors in ((DbEntityValidationException)exception).EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        message += validationError.ErrorMessage + Environment.NewLine;

                return message;
            }

            if (exception.InnerException != null)
                return $"{exception.Message}{Environment.NewLine}{ExtractEntityMessage(exception.InnerException)}";
            else
                return exception.Message;
        }

        /// <summary>
        /// O pipeline do MVC impede que a ViewData seja mantida após redirecionamento
        /// Este método auxilia, transportando a ViewData entre um request e outro, após um redirecionamento
        /// </summary>
        protected void TransportViewData()
        {
            TempData["ViewData"] = ViewData;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (TempData["ViewData"] != null)
                ViewData = (ViewDataDictionary)TempData["ViewData"];
        }

        #endregion
    }
}