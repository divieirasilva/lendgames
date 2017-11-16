using LendGames.Database;
using LendGames.Database.Models;
using LendGames.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

/*
* ******************************************************
* 
* Este atributo é responsável pelo gerenciamento contas
* conectadas para cada Action do MVC, basta colocá-lo
* na declaração da Action e então ela passará a procurar
* pelo usuário conectado e irá verificar se ele pode ou
* não acessá-la.
* 
* ******************************************************
*/

namespace LendGames.Web.MvcApp
{
    public class RequireConnectionAttribute : ActionFilterAttribute
    {
        public AccountType[] RequireTypes = new AccountType[] { };
        public bool ThrowExceptions = false;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var db = new LendGamesContext();
            var accountRepository = new AccountRepository(db);

            if (filterContext.HttpContext.Session["ConnectedId"] == null)
            {
                filterContext.HttpContext.Session.Abandon();
                if (ThrowExceptions)
                    throw new HttpException("Não foi possível prosseguir, pois não há usuário conectado.");
                else
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "Action", "Index" } });
            }
            else
            {
                var id = (int?)filterContext.HttpContext.Session["ConnectedId"];

                var account = accountRepository.Find(id);
                if (account == null)
                {
                    if (ThrowExceptions)
                        throw new HttpException("A conta que está conectada é inválida.");
                    else
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "Action", "Index" } });
                }
                else if (RequireTypes.Any() ? !RequireTypes.Contains(account.Type) : false)
                {
                    if (ThrowExceptions)
                        throw new HttpException("O tipo de conta conectada não pode acessar a URL solicitada.");
                    else
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "Action", "PermissionRequired" } });
                }

            }
        }
    }
}