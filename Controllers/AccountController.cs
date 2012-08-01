using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Mvc4.OpenId.Sample.Models;
using Mvc4.OpenId.Sample.Security;
using DotNetOpenAuth.Messaging;

namespace Mvc4.OpenId.Sample.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly OpenIdMembershipService openidemembership;

        public AccountController()
        {
            openidemembership = new OpenIdMembershipService();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            var user = openidemembership.GetUser();
            
            if (user != null)
            {
                var cookie = openidemembership.CreateFormsAuthenticationCookie(user);
                HttpContext.Response.Cookies.Add(cookie);

                return new RedirectResult(Request.Params["ReturnUrl"] ?? "/");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string openid_identifier)
        {
            var response = openidemembership.ValidateAtOpenIdProvider(openid_identifier);

            if (response != null)
            {
                return response.RedirectingResponse.AsActionResult();
            }

            return View();
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
