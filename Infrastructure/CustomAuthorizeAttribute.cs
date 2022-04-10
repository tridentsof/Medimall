using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Medimall.Models;

namespace Medimall.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userName = Convert.ToString(httpContext.Session["UserName"]);
            if (!string.IsNullOrEmpty(userName))
                using (var context = new MedimallEntities())
                {
                    var userRole = (from u in context.Admins
                                    join r in context.Roles on u.RoleId equals r.RoleId
                                    where u.UserName == userName
                                    select new
                                    {
                                        r.RoleName
                                    }).FirstOrDefault();
                    foreach (var role in allowedroles)
                    {
                        if (role == userRole.RoleName) return true;
                    }
                }


            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "AdminHome" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}