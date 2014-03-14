
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EntityLibrary;

namespace QLKhachSanWeb.Helper
{
    public class AuthAttribute : AuthorizeAttribute
    {
        private bool isLogin=false;
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //Invoke base authorization
           

            //If base authorization pass, do custom authorization
            isLogin = httpContext.Session["SessionUser"] != null;           

            return isLogin;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!isLogin)
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Write("nologin");
                    return;
                }
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.HttpContext.Response.Redirect(urlHelper.Action("Login","Login"));
                return;
            }
         
         
        }
    }

    //public class AuthAttribute : ActionFilterAttribute
    //{
    //    public int Role { get; set; }
    //    public int View { get; set; }

    //    public override void OnActionExecuting(ActionExecutingContextk filterContext)
    //    {
    //        bool isInRoles = false;

    //        UserInfo info = VisitContext.userInfo;

    //        if (info != null && info.Role.Length >= View)
    //        {
    //            if (View == 0)
    //            {
    //                isInRoles = true;
    //            }
    //            else
    //            {
    //                byte roleView = info.Role[View - 1];

    //                if (roleView >= Role)
    //                {
    //                    isInRoles = true;
    //                }
    //                //else
    //                //{
    //                //    //FormsAuthentication.SignOut();
    //                //}
    //            }
    //        }

    //        if (isInRoles)
    //        {
    //            base.OnActionExecuting(filterContext);
    //        }
    //        else
    //        {
    //            var urlHelper = new UrlHelper(filterContext.RequestContext);
    //            filterContext.Result = new RedirectResult(urlHelper.Action("Index", "Login"));
    //            //filterContext.HttpContext.Response.Redirect(urlHelper.Action("Forbidden", "Shared"));
    //        }

    //        //else
    //        //{
    //        //    FormsAuthentication.SignOut();
    //        //}

    //        //var groupRoleList = filterContext.Visitor.Roles;

    //        //if (groupRoleList != null && groupRoleList is IEnumerable<string>)
    //        //{
    //        //    foreach (string groupRole in groupRoleList)
    //        //    {
    //        //        string role = groupRole.Split('|').Last();

    //        //        if (!string.IsNullOrEmpty(role) && allowedRoles.Contains(role))
    //        //        {
    //        //            isInRoles = true;
    //        //            break;
    //        //        }
    //        //    }
    //        //}

    //        //if (isInRoles)
    //        //{
    //        //    base.OnActionExecuting(filterContext);
    //        //}
    //        //else
    //        //{
    //        //    var urlHelper = new UrlHelper(filterContext.RequestContext);
    //        //    filterContext.Result = new RedirectResult(urlHelper.Action("Forbidden", "Shared"));
    //        //    //filterContext.HttpContext.Response.Redirect(urlHelper.Action("Forbidden", "Shared"));
    //        //}
    //    }
    //}
}