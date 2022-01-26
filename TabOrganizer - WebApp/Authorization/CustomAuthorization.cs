using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabOrganizer___WebApp.Authorization
{
    public class CustomAuthorization : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userName = context.HttpContext.User.Identity.Name;

            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
