using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;

namespace ChicStoreManagement.CustomAttributes
{

        public class BasicAuthAttribute : ActionFilterAttribute, IAuthenticationFilter

        {

            public void OnAuthentication(AuthenticationContext filterContext)

            {
            var user = filterContext.HttpContext.User;
            var name = filterContext.HttpContext.User.Identity.Name;
            if (user == null || !user.Identity.IsAuthenticated)

            {

                filterContext.Result = new HttpUnauthorizedResult();

            }
        }



            public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)

            {

            }

        }

    }

