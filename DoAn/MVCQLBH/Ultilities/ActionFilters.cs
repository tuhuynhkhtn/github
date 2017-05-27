using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCQLBH.Ultilities
{
    public class AuthActionFilter : FilterAttribute, IActionFilter
    {
        public int RequiredPermission { get; set; }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (AddHelpers.IsLogged(null) == false)
            //{
            //    filterContext.Result = new RedirectResult("~/Account/Login");
            //    return;
            //}

            //var ui = AddHelpers.GetUserInfo(null);

            //if (ui.Permission < RequiredPermission)
            //{
            //    filterContext.Result = new HttpUnauthorizedResult();
            //}
        }

    }
}