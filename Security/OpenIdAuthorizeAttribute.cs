using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using Mvc4.OpenId.Sample.Models;

namespace Mvc4.OpenId.Sample.Security
{
    public class OpenIdAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (isAuthorized)
            {
                var authenticatedCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authenticatedCookie != null)
                {
                    var authenticatedCookieValue = authenticatedCookie.Value.ToString();
                    if (!string.IsNullOrWhiteSpace(authenticatedCookieValue))
                    {
                        var decryptedTicket = FormsAuthentication.Decrypt(authenticatedCookieValue);
                        var user = new OpenIdUser(decryptedTicket.UserData);
                        var openIdIdentity = new OpenIdIdentity(user);
                        httpContext.User = new GenericPrincipal(openIdIdentity, null);
                    }
                }
            }
            return isAuthorized;
        }
    }
}